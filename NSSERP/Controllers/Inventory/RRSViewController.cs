using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using NSSERP.Models;


namespace NSSERP.Controllers.Inventory
{
    public class RRSViewController : Controller
    {
        private readonly GetAllUsersDetails _allUsersDetailsmodel;

        public RRSViewController(GetAllUsersDetails getAllUsersDetails)
        {
            _allUsersDetailsmodel = getAllUsersDetails;
        }
        public IActionResult RRsView()
        {
            ViewBag.PdfUrl = "path_to_your_pdf_file.pdf";
            return View();
        }      


    }
}
