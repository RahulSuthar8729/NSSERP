using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.Searching.Controllers
{
    [Authorize]
    [Area("Searching")]
    public class AdvanceSearchingController : Controller
    {
        public IActionResult AdvSearch()
        {
            return View();
        }
    }
}
