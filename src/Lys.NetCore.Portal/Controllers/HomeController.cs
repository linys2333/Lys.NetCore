using Microsoft.AspNetCore.Mvc;

namespace Lys.NetCore.Portal.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult<string> Index()
        {
            return "Hello World!";
        }
    }
}
