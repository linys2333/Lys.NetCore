using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyService.Data;
using MyService.Services;
using MyWebAPI.Common;

namespace MyWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class BaseController : Controller
    {
        private readonly MyDbContext _dbContext;
        
        public BaseController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected T GetSever<T>() where T : BaseService, new()
        {
            var service = new T
            {
                DbContext = _dbContext
            };
            return service;
        }

        protected JsonResult Json(bool succeeded, string message = null, object data = null)
        {
            return base.Json(new AjaxResponse<object>(succeeded, message, data));
        }
    }
}