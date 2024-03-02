using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Areas.NationalGangotri.Controllers
{
    [Authorize]
    [Area("NationalGangotri")]
    public class DonorDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
