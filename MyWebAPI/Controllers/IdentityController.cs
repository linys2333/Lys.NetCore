using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Stores.Entity;
using System.Linq;

namespace MyWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Identity")]
    public class IdentityController : BazaController
    {
        public IdentityController(BazaDbContext dbContext) : base(dbContext)
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}