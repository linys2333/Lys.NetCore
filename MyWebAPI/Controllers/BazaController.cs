using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyService.Managers;
using MyService.Stores.Entity;
using MyWebAPI.Common;

namespace MyWebAPI.Controllers
{
    [Authorize]
    public class BazaController : Controller
    {
        private readonly BazaDbContext m_dbContext;
        
        public BazaController(BazaDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        protected T GetSever<T>() where T : BazaService, new()
        {
            var service = new T
            {
                DbContext = m_dbContext
            };
            return service;
        }

        protected JsonResult Json(bool succeeded, string message = null, object data = null)
        {
            return base.Json(new AjaxResponse<object>(succeeded, message, data));
        }
    }
}