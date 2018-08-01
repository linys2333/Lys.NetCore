using AutoMapper;
using LysCore.Common.Extensions;
using LysCore.Controllers;
using LysCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CrDomain = Domain.CallRecords;

namespace Host.Controllers.CallRecord
{
    public class CallRecordController : LysAuthController
    {
        private readonly LazyService<CrDomain.CallRecordManager> m_CallRecordManager = new LazyService<CrDomain.CallRecordManager>();

        /// <summary>
        /// 新增通话记录
        /// </summary>
        /// <remarks>
        /// 示例：（具体请求格式请用postman模拟查看）
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
            var callRecord = Mapper.Map<CrDomain.CallRecord>(callRecordDto);
            callRecord.OwnerId = userId;

            var fileBytes = IOExt.StreamToBytes(file?.OpenReadStream());
            var fileId = await m_CallRecordManager.Instance.SaveFileAsync(callRecord, file?.FileName, fileBytes);
            
            callRecord.FileId = fileId;
            await m_CallRecordManager.Instance.CreateAsync(callRecord);
        }
    }
}
