using LysCore.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LysCore.Controllers
{
    /// <summary>
    /// Controller基类
    /// </summary>
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [Route("[controller]")]
    public class LysController : ControllerBase
    {
    }

    /// <summary>
    /// 需要校验身份的Controller基类
    /// </summary>
    [Authorize]
    [ParameterNotNullOrEmpty("userId")]
    public class LysAuthController : LysController
    {
    }
}