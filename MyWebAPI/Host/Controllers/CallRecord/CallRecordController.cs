using AutoMapper;
using Common;
using LysCore.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CallRecordDomain = Domain.CallRecord;

namespace Host.Controllers.CallRecord
{
    public class CallRecordController : LysAuthController
    {
        /// <summary>
        /// 新增通话记录
        /// </summary>
        /// <remarks>
        /// 示例：
        /// 
        ///     POST api/CallRecord/Save
        ///     Content-Type: multipart/form-data
        ///     FormData: 
        ///       Duration: xxx
        ///       file: ByteArrayContent
        ///         Content-Disposition: form-data; name="file"; filename="xxx.amr"
        ///         Content-Type: application/octet-stream
        /// </remarks>
        /// <param name="callRecordDto">通话信息</param>
        /// <param name="file">录音文件</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task Create([FromForm]CallRecordDto callRecordDto, IFormFile file, [FromHeader]Guid userId)
        {
            var callRecordManager = GetService<CallRecordDomain.CallRecordManager>();
            var callRecord = Mapper.Map<CallRecordDomain.CallRecord>(callRecordDto);
            callRecord.OwnerId = userId;

            var fileBytes = IOExtensions.StreamToBytes(file?.OpenReadStream());
            var fileId = await callRecordManager.SaveFileAsync(callRecord, fileBytes);
            
            callRecord.FileId = fileId;
            await callRecordManager.CreateAsync(callRecord);
        }
    }
}
