using Microsoft.AspNetCore.Mvc;

namespace NSSERP.Controllers
{
    public class AdministratorController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
