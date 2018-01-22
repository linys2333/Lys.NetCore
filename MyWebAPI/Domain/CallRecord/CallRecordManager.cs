using Common;
using Common.Configuration;
using Common.Interfaces;
using LysCore.Common;
using LysCore.Common.Extensions;
using LysCore.Exceptions;
using LysCore.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CallRecord
{
    public class CallRecordManager : LysDomain
    {
        private readonly FileServiceConfig m_FileServiceConfig;
        private readonly LazyService<IFFmpeg> m_FFmpeg = new LazyService<IFFmpeg>();
        private readonly LazyService<ICallRecordRepository> m_CallRecordRepository = new LazyService<ICallRecordRepository>();

        public CallRecordManager(IOptions<FileServiceConfig> fileServiceConfig)
        {
            m_FileServiceConfig = fileServiceConfig.Value;
        }

        public async Task CreateAsync(CallRecord callRecord)
        {
            Requires.NotNull(callRecord, nameof(callRecord));
            
            callRecord.CreatorId = callRecord.OwnerId;
            callRecord.UpdaterId = callRecord.OwnerId;

            await m_CallRecordRepository.Instance.CreateAsync(callRecord);
        }

        public async Task<int> CountMyTodayCallsAsync(Guid userId)
        {
            Requires.NotNullGuid(userId, nameof(userId));
            var count = await m_CallRecordRepository.Instance.CountMyTodayCallsAsync(userId);
            return count;
        }

        public async Task<Guid?> SaveFileAsync(CallRecord callRecord, byte[] amrBytes)
        {
            Requires.NotNull(callRecord, nameof(callRecord));
            Requires.NotNull(amrBytes, nameof(amrBytes));

            if (amrBytes != null && amrBytes.Any())
            {
                var amrName = $"{callRecord.Id}.amr";
                var mp3Name = $"{callRecord.Id}.mp3";
                var amrPath = Path.Combine(MyConstants.Paths.FFmpegFolder, amrName);
                var mp3Path = Path.Combine(MyConstants.Paths.FFmpegFolder, mp3Name);

                await File.WriteAllBytesAsync(amrPath, amrBytes);

                m_FFmpeg.Instance.Convert(amrName, mp3Name);

                if (!File.Exists(mp3Path))
                {
                    throw new BusinessException("录音转码失败", MyConstants.Errors.CallFileError);
                }

                var mp3Bytes = await File.ReadAllBytesAsync(mp3Path);
                if (mp3Bytes != null && mp3Bytes.Any())
                {
                    var fileId = await UploadFileServerAsync(callRecord.OwnerId, mp3Bytes, "mp3");
                    return fileId;
                }
            }

            return null;
        }

        /// <summary>
        /// 上传至文件服务器
        /// </summary>
        private async Task<Guid> UploadFileServerAsync(Guid ownerId, byte[] fileBytes, string fileExt = null)
        {
            Requires.NotNullGuid(ownerId, nameof(ownerId));
            Requires.NotNull(fileBytes, nameof(fileBytes));

            var fileId = SequentialGuid.NewGuid();

            var file = new
            {
                FileId = fileId,
                FileName = string.IsNullOrEmpty(fileExt) ? fileId.ToString() : $"{fileId}.{fileExt}",
                Data = fileBytes
            };
            
            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/json");

            var errMsg = string.Empty;
            try
            {
                var response = await client.PostAsync($"{m_FileServiceConfig.BaseUrl}file/upload", content);
                if (!response.IsSuccessStatusCode)
                {
                    errMsg = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.GetMessage();
            }

            if (!string.IsNullOrEmpty(errMsg))
            {
                throw new BusinessException(errMsg, MyConstants.Errors.CallFileError);
            }

            return fileId;
        }
    }
}
