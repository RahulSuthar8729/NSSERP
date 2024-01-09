using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.AdminSetup.Controllers
{

    [Authorize]
    [Area("AdminSetup")]
    public class BankStatementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
