using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.Searching.Controllers
{
    [Authorize]
    [Area("Searching")]
    public class TransactionSearchingController : Controller
    {
        public IActionResult TnxSearch()
        {
            return View();
        }
    }
}
