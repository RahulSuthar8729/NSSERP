using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.PostReceive.Controllers
{
    [Authorize]
    [Area("PostReceive")]
    public class AccountVerificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
