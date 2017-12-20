using LysCore.Common.Web;
using LysCore.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LysCore.Controllers
{
    [Produces("application/json")]
    [ProducesResponseType(typeof(AjaxResponse<object>), 200)]
    [Route("[controller]")]
    public class LysController : ControllerBase
    {
        protected virtual T GetService<T>() where T : LysDomain
        {
            var service = ActivatorUtilities.GetServiceOrCreateInstance<T>(HttpContext.RequestServices);
            return service;
        }
    }

    [Authorize]
    public class LysAuthController : LysController
    {
    }
}