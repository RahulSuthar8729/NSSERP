using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSSERP.Models;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
namespace NSSERP.Controllers.Donation
{
    [Authorize]
    public class DonationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DonationController> _logger;

        public DonationController(IConfiguration configuration, ILogger<DonationController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }      
        public IActionResult DonationMaster()
        {
            return View();
        }
        public IActionResult WelcomeDomnationReceiveMaster()
        {
            return View();
        }
        public IActionResult CallCenterDonationReceiveMaster()
        {
            return View();
        }
        public IActionResult ForwordAddressingToDonation()
        {
            return View();
        }
        public IActionResult DonationEntry()
        {
            return View();
        }
        public IActionResult DonationList()
        {
            return View();
        }
        public IActionResult ReceiptDispatchMaster()
        {
            return View();
        }
        public IActionResult ForwardToPostReceive()
        {
            return View();
        }
        public IActionResult SankalpSiddhiMaster()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCountryList()
        {
            List<SelectListItem> countryList = new List<SelectListItem>();

            string connectionString = _configuration.GetConnectionString("Constr");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, Name FROM Countries";

                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);

                        countryList.Add(new SelectListItem { Value = id.ToString(), Text = name });
                    }
                }

                // Close the connection
                connection.Close();
            }

            return Json(countryList);
        }

        public IActionResult VirtualPerakList()
        {
            return View();
        }

        public IActionResult testsidebar()
        {
            return View();
        }
    }
}
