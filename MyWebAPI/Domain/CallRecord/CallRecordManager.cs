﻿using Common;
using Common.Configuration;
using Common.Interfaces;
using LysCore.Common;
using LysCore.Common.Extensions;
using LysCore.Domain;
using LysCore.Exceptions;
using LysCore.FileService;
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
        private readonly IFFmpeg m_FFmpeg;
        private readonly ICallRecordRepository m_CallRecordRepository;

        public CallRecordManager(IOptions<FileServiceConfig> fileServiceConfig, IFFmpeg ffmpeg, ICallRecordRepository callRecordRepository)
        {
            m_FileServiceConfig = fileServiceConfig.Value;
            m_FFmpeg = ffmpeg;
            m_CallRecordRepository = callRecordRepository;
        }

        public async Task CreateAsync(CallRecord callRecord)
        {
            Requires.NotNull(callRecord, nameof(callRecord));
            
            callRecord.CreatorId = callRecord.OwnerId;
            callRecord.UpdaterId = callRecord.OwnerId;

            await m_CallRecordRepository.CreateAsync(callRecord);
        }

        public async Task<int> CountMyTodayCallsAsync(Guid userId)
        {
            Requires.NotNullGuid(userId, nameof(userId));
            var count = await m_CallRecordRepository.CountMyTodayCallsAsync(userId);
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

                m_FFmpeg.Convert(amrName, mp3Name);

                if (!File.Exists(mp3Path))
                {
                    throw new BusinessException(MyConstants.Errors.CallFileError, "录音转码失败");
                }

                var mp3Bytes = await File.ReadAllBytesAsync(mp3Path);
                if (mp3Bytes != null && mp3Bytes.Any())
                {
                    var fileId = await SaveToOSSAsync(callRecord.OwnerId, mp3Bytes);
                    return fileId;
                }
            }

            return null;
        }

        /// <summary>
        /// 上传至阿里云
        /// </summary>
        private async Task<Guid> SaveToOSSAsync(Guid ownerId, byte[] fileBytes)
        {
            Requires.NotNullGuid(ownerId, nameof(ownerId));
            Requires.NotNull(fileBytes, nameof(fileBytes));

            var fileId = SequentialGuid.NewGuid();

            var file = new AliOssFile
            {
                AppName = m_FileServiceConfig.AppName,
                Key = fileId.ToString(),
                Data = fileBytes,
                OwnerId = ownerId.ToString(),
                FileName = fileId.ToString(),
                CodePage = 936,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                FileStatus = FileStatus.Normal,
                Remark = "电话机录音文件"
            };
            
            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/json");

            var errMsg = string.Empty;
            try
            {
                var response = await client.PostAsync($"{m_FileServiceConfig.BaseUrl}file/save", content);
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
                throw new BusinessException(MyConstants.Errors.CallFileError, errMsg);
            }

            return fileId;
        }
    }
}
