using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyService.Data;
using MyService.Services;
using System.IO;
using System.Threading.Tasks;

namespace MyWebAPI.Controllers
{
    public class TestController : BaseController
    {
        public TestController(MyDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取待沟通列表
        /// </summary>
        /// <remarks>
        /// 示例：
        /// 
        ///     GET api/Communication/00000000-0000-0000-0000-000000000000
        /// </remarks>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<JsonResult> GetList(string id)
        {
            var list = await GetSever<TestService>().GetListAsync(id);

            return Json(true, string.Empty, list);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<JsonResult> Create(IFormFile file)
        {
            if (file == null)
            {
                return Json(true);
            }
            var fileStream = file.OpenReadStream();

            var a = StreamToBytes(fileStream);
            var basePath = Directory.GetCurrentDirectory();

            await System.IO.File.WriteAllBytesAsync($"{basePath}/1.amr", a);
            await GetSever<TestService>().CreateAsync();

            return Json(true);
        }

        private byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}
