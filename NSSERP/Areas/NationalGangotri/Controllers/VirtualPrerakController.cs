using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.NationGangotri.Controllers
{
    [Area("NationalGangotri")]
    public class VirtualPrerakController : Controller
    {
        public IActionResult VirtualPrerakList()
        {
            return View();
        }
        public IActionResult VirtualPrerakMaster()
        {
            return View();

        }

    }
}
