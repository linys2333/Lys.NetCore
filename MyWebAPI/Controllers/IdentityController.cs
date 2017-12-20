using Microsoft.AspNetCore.Mvc;
using MyService.Data;
using System.Linq;

namespace MyWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Identity")]
    public class IdentityController : BaseController
    {
        public IdentityController(MyDbContext dbContext) : base(dbContext)
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}