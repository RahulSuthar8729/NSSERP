using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Controllers.Inventory
{
    [Authorize]
    [Route("Inventory/[controller]")]
    public class ModuleController : Controller
    {
        [HttpGet("")]
        public IActionResult Home()
        {
            return View();
        }
    }
}
