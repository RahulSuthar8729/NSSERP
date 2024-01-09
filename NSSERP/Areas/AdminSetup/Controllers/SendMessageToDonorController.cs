using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.AdminSetup.Controllers
{
    [Authorize]
    [Area("AdminSetup")]
    public class SendMessageToDonorController : Controller
    {
        public IActionResult SendMsgandMail()
        {
            return View();
        }
    }
}
