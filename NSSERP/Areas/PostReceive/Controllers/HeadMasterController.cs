using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.PostReceive.Controllers
{
    [Authorize]
    [Area("PostReceive")]
    public class HeadMasterController : Controller
    {
        public IActionResult HeadMaster()
        {
            return View();
        }
    }
}
