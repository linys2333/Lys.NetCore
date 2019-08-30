using Lys.NetCore.Infrastructure.Extensions;
using Lys.NetCore.Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;

namespace Lys.NetCore.Portal.Web
{
    [Produces("application/json")]
    [ApiController]
    [BuildSession]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme, Policy = "WebApiAccess")]
    public abstract class WebApiController : Controller
    {
        protected JsonResult Ok<T>(T data)
        {
            return Json(WebApiResponse.Ok(data));
        }

        protected JsonResult Fail(string errorCode, string errorMessage)
        {
            var error = new ErrorDescriber { Code = errorCode, Description = errorMessage };
            return Json(WebApiResponse.Fail(error));
        }

        protected JsonResult Fail<T>(string errorCode, string errorMessage, T result)
        {
            var error = new ErrorDescriber { Code = errorCode, Description = errorMessage };
            return Json(WebApiResponse.Fail(error, result));
        }

        protected JsonResult Json<T>(IEnumerable<SimplyResult<T>> results)
        {
            return Json(results.Select(r => r.IsSuccess
                ? WebApiResponse.Ok(r.Data)
                : WebApiResponse.Fail(new ErrorDescriber { Code = r.ErrorCode, Description = r.ErrorMessage }, r.Data)));
        }

        protected JsonResult Json(IEnumerable<SimplyResult> results)
        {
            return Json(results.Select(r => r.IsSuccess
                ? WebApiResponse.Ok()
                : WebApiResponse.Fail(new ErrorDescriber { Code = r.ErrorCode, Description = r.ErrorMessage })));
        }

        protected JsonResult Json<T>(SimplyResult<T> result)
        {
            return result.IsSuccess
                ? Ok(result.Data)
                : Fail(result.ErrorCode, result.ErrorMessage, result.Data);
        }

        protected JsonResult Json(SimplyResult result)
        {
            return result.IsSuccess
                ? Json(WebApiResponse.Ok())
                : Fail(result.ErrorCode, result.ErrorMessage);
        }
    }
}