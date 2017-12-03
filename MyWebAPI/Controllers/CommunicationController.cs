using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Managers;
using MyWebAPI.Stores.Entity;
using System.Threading.Tasks;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CommunicationController : BazaController
    {
        public CommunicationController(BazaDbContext dbContext) : base(dbContext)
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
            var list = await GetSever<CommunicationManager>().GetListAsync(id);

            return Json(true, string.Empty, list);
        }

        [HttpPost]
        public void Create([FromBody]string value)
        {
        }
    }
}
