using AutoMapper;
using Lys.NetCore.Domain.CallRecords;
using Lys.NetCore.Domain.CallRecords.Models;
using Lys.NetCore.Infrastructure.Extensions;
using Lys.NetCore.Infrastructure.Services;
using Lys.NetCore.Infrastructure.Web;
using Lys.NetCore.Portal.DTOs;
using Lys.NetCore.Portal.Examples;
using Lys.NetCore.Portal.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace Lys.NetCore.Portal.Controllers
{
    [Route("[controller]")]
    public class CallRecordController : WebApiController
    {
        private readonly IMapper m_Mapper;

        private readonly LazyService<CallRecordManager> m_CallRecordManager = new LazyService<CallRecordManager>();

        public CallRecordController(IMapper mapper)
        {
            m_Mapper = mapper;
        }

        /// <summary>
        /// 上传通话记录
        /// </summary>
        /// <remarks>
        /// 示例：（具体请求格式请用postman模拟查看）
        /// 
        ///     POST /CallRecord/xxx
        ///     Content-Type: multipart/form-data
        ///     FormData: 
        ///       Mobile: xxx
        ///       Duration: xxx
        ///       file: ByteArrayContent
        ///         Content-Disposition: form-data; name="file"; filename="xxx.amr"
        ///         Content-Type: application/octet-stream
        /// </remarks>
        /// <param name="userId">用户Id</param>
        /// <param name="request">通话信息</param>
        /// <param name="file">录音文件</param>
        /// <returns></returns>
        [HttpPost("{userId:guid}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(WebApiResponse<Guid>), (int)HttpStatusCode.OK)]
        [SwaggerRequestExample(typeof(CallRecordRequest), typeof(CallRecordRequestExample))]
        public async Task<IActionResult> Create(Guid userId, [FromForm]CallRecordRequest request, [Required]IFormFile file)
        {
            var callRecord = m_Mapper.Map<CallRecord>(request);
            callRecord.OwnerId = userId;

            var fileBytes = IOExtensions.StreamToBytes(file?.OpenReadStream());
            var saveResult = await m_CallRecordManager.Instance.SaveFileAsync(callRecord, file?.FileName, fileBytes);
            if (saveResult.IsSuccess)
            {
                callRecord.FileId = saveResult.Data;
                await m_CallRecordManager.Instance.CreateAsync(callRecord);
            }

            return Json(saveResult);
        }
    }
}
