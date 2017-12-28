using LysCore.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LysCore.Controllers
{
    [Produces("application/json")]
    [ProducesResponseType(typeof(AjaxResponse<object>), 200)]
    [Route("[controller]")]
    public class LysController : ControllerBase
    {
    }

    [Authorize]
    [ParameterNotNullOrEmpty("userId")]
    public class LysAuthController : LysController
    {
    }
}