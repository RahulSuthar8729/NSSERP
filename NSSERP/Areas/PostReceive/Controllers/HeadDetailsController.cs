using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.PostReceive.Controllers
{
    [Authorize]
    [Area("PostReceive")]
    public class HeadDetailsController : Controller
    {
        public IActionResult HeadDetails()
        {
            return View();
        }
    }
}
