using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSSERP.Models; // Replace with your actual namespace for models
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Authorization;

namespace NSSERP.Controllers.Gangotri
{
    [Authorize]
    public class GangotriController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GangotriController> _logger;

        public GangotriController(IConfiguration configuration, ILogger<GangotriController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Home()
        {
            ViewBag.ShowSidebar = true;
            return View();
        }
        public IActionResult Announce()
        {
            // Query the database to get a list of countries
            //using (var connection = new SqlConnection(_configuration.GetConnectionString("Constr")))
            //{
            //    connection.Open();
            //    var countries = connection.Query<SelectCountry>("SELECT Id, Name FROM dbo.Country").ToList();

            //    // Pass the list to the view
            //    ViewBag.Countries = new SelectList(countries, "Id", "Name");
            //}
            var countries = new List<SelectCountry>
            {
    new SelectCountry { Id = 1, Name = "India" },
    new SelectCountry { Id = 2, Name = "United States" },
    new SelectCountry { Id = 3, Name = "United Kingdom" }

             };


            ViewBag.Countries = new SelectList(countries, "Id", "Name", 0); // 0 is the ID of the item you want to be selected

            // state

            var State = new List<SelectState>
            {
    new SelectState { Id = 1, Name = "Rajastahn" },
    new SelectState { Id = 2, Name = "Panjab" },
    new SelectState { Id = 3, Name = "Gujrat" }

             };


            ViewBag.State = new SelectList(State, "Id", "Name", 0);

            // City

            var District = new List<SelectDistrict>
            {
    new SelectDistrict { Id = 1, Name = "Jaipur" },
    new SelectDistrict { Id = 2, Name = "Udaipur" },
    new SelectDistrict { Id = 3, Name = "Sikar" }

             };


            ViewBag.District = new SelectList(District, "Id", "Name", 0);


            // City

            var City = new List<SelectCity>
            {
    new SelectCity { Id = 1, Name = "Jaipur" },
    new SelectCity { Id = 2, Name = "Udaipur" },
    new SelectCity { Id = 3, Name = "Sikar" }

             };


            ViewBag.City = new SelectList(City, "Id", "Name", 0);



            return View();
        }
        public IActionResult BasicSearch() 
        {
            var City = new List<SelectCity>
            {
    new SelectCity { Id = 1, Name = "Jaipur" },
    new SelectCity { Id = 2, Name = "Udaipur" },
    new SelectCity { Id = 3, Name = "Sikar" }

             };


            ViewBag.City = new SelectList(City, "Id", "Name", 0);


            var countries = new List<SelectCountry>
            {
    new SelectCountry { Id = 1, Name = "India" },
    new SelectCountry { Id = 2, Name = "United States" },
    new SelectCountry { Id = 3, Name = "United Kingdom" }

             };


            ViewBag.Countries = new SelectList(countries, "Id", "Name", 0); // 0 is the ID of the item you want to be selected

            // state

            var State = new List<SelectState>
            {
    new SelectState { Id = 1, Name = "Rajastahn" },
    new SelectState { Id = 2, Name = "Panjab" },
    new SelectState { Id = 3, Name = "Gujrat" }

             };


            ViewBag.State = new SelectList(State, "Id", "Name", 0);

            // City

            var District = new List<SelectDistrict>
            {
    new SelectDistrict { Id = 1, Name = "Jaipur" },
    new SelectDistrict { Id = 2, Name = "Udaipur" },
    new SelectDistrict { Id = 3, Name = "Sikar" }

             };


            ViewBag.District = new SelectList(District, "Id", "Name", 0);

            //tehsil
            var Tehsil = new List<SelectTehsil>
            {
    new SelectTehsil { Id = 1, Name = "Jaipur" },
    new SelectTehsil { Id = 2, Name = "Udaipur" },
    new SelectTehsil { Id = 3, Name = "Sikar" }

             };


            ViewBag.Tehsil = new SelectList(Tehsil, "Id", "Name", 0);


            return View(); 
        }
        public IActionResult DetailSearch()
        {
          return View();
        }
        public IActionResult DonationPendingDetails()
        {
            return View();
        }
        public IActionResult ForwordPhotoToChecking()
        {
            return View();
        }
        public IActionResult ForwordCheckingToPrinting()
        {
            return View();
        }
        public IActionResult AppShreeKaPurnVivran()
        {
            return View();
        }
        public IActionResult ManavFormulaToDispatch()
        {
            return View();
        }
        public IActionResult ForwordCardPrintingToDispatch()
        {
            return View();
        }
        public IActionResult PrintEnvelope()
        {
            return View();
        }
        public IActionResult DispatchPendingDetails()
        {
            return View();
        }
        public IActionResult WelcomeDonationReceiveDetails()
        {
            return View();
        }        
        public IActionResult CallCenterDonationReceiveDetails()        
        {
        return View();
        }      
        public IActionResult ForwordRelation()
        {
            return View();
        }
        public IActionResult testtheme()
        {
            return View();
        }

    }
}
