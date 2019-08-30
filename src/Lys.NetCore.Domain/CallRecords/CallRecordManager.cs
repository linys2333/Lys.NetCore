using Lys.NetCore.Domain.CallRecords.Models;
using Lys.NetCore.Infrastructure.Extensions;
using Lys.NetCore.Infrastructure.Services;
using Lys.NetCore.Infrastructure.Web;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lys.NetCore.Infrastructure.FFmpeg;
using static Lys.NetCore.Infrastructure.Extensions.MyConstants;

namespace Lys.NetCore.Domain.CallRecords
{
    public class CallRecordManager : ServiceBase
    {
        private readonly ILogger m_Logger;
        private readonly IHttpClientFactory m_HttpClientFactory;

        private readonly LazyService<IFFmpeg> m_FFmpeg = new LazyService<IFFmpeg>();
        private readonly LazyService<ICallRecordRepository> m_CallRecordRepository = new LazyService<ICallRecordRepository>();

        public CallRecordManager(ILogger logger, IHttpClientFactory httpClientFactory)
        {
            m_Logger = logger;
            m_HttpClientFactory = httpClientFactory;
        }

        public async Task CreateAsync(CallRecord callRecord)
        {
            Requires.NotNull(callRecord, nameof(callRecord));
            
            callRecord.CreatorId = callRecord.OwnerId;
            callRecord.UpdaterId = callRecord.OwnerId;

            await m_CallRecordRepository.Instance.CreateAsync(callRecord);
        }
        
        /// <summary>
        /// 保存录音文件
        /// </summary>
        /// <param name="callRecord"></param>
        /// <param name="fileName"></param>
        /// <param name="amrBytes"></param>
        /// <returns></returns>
        public async Task<SimplyResult<Guid>> SaveFileAsync(CallRecord callRecord, string fileName, byte[] amrBytes)
        {
            Requires.NotNull(callRecord, nameof(callRecord));
            Requires.NotNullOrEmpty(fileName, nameof(fileName));

            if (!amrBytes?.Any() ?? true)
            {
                return SimplyResult.Fail(nameof(Errors.EmptyFile), Errors.EmptyFile, Guid.Empty);
            }

            var amrName = $"{callRecord.Id}_{fileName}";
            var mp3Name = $"{callRecord.Id}.mp3";
            var amrPath = Path.Combine(Paths.FFmpegFolder, amrName);
            var mp3Path = Path.Combine(Paths.FFmpegFolder, mp3Name);

            await File.WriteAllBytesAsync(amrPath, amrBytes);

            m_FFmpeg.Instance.Convert(amrName, mp3Name);

            if (File.Exists(mp3Path))
            {
                var mp3Bytes = await File.ReadAllBytesAsync(mp3Path);
                if (mp3Bytes?.Any() ?? false)
                {
                    return await UploadFileServerAsync(mp3Bytes, "mp3");
                }
            }

            return SimplyResult.Fail(nameof(Errors.ConvertFileError), Errors.ConvertFileError, Guid.Empty);
        }

        /// <summary>
        /// 上传至文件服务器
        /// </summary>
        private async Task<SimplyResult<Guid>> UploadFileServerAsync(byte[] fileBytes, string fileExt = null)
        {
            var fileId = SequentialGuid.NewGuid();

            var file = new
            {
                FileId = fileId,
                FileName = string.IsNullOrEmpty(fileExt) ? fileId.ToString() : $"{fileId}.{fileExt}",
                Data = fileBytes
            };
            
            try
            {
                var httpClient = m_HttpClientFactory.CreateClient("FileClient");
                var response = await httpClient.PostAsJsonAsync("file/upload", file);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<WebApiResponse>(responseBody);
                if (!result.Success)
                {
                    throw new Exception(result.Error.Description);
                }

                return SimplyResult.Ok(fileId);
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex, "上传文件失败");
                return SimplyResult.Fail(nameof(Errors.RequestFileAPIFail), Errors.RequestFileAPIFail, Guid.Empty);
            }
        }
    }
}
