using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSSERP.Areas.Masters.Models;
using NSSERP.Models;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Security.Claims;

namespace NSSERP.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class CountryMasterController : Controller
    {

        private readonly string _connectionString;

        public CountryMasterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM CountryMaster";
                var countries = connection.Query<Countrys>(query);

                return View(countries);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Countrys country)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Use DynamicParameters to define both input and output parameters
                    var parameters = new DynamicParameters();
                    parameters.Add("@Country_Name", country.CountryName);
                    parameters.Add("@Country_Code", country.CountryCode);
                    parameters.Add("@Status", country.IsActive);
                    parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    // Execute the stored procedure with parameters
                    connection.Execute("InsertNewCountry", parameters, commandType: CommandType.StoredProcedure);

                    // Access the output parameter after the execution
                    var resultMessage = parameters.Get<string>("@ResultMessage");

                    if (!string.IsNullOrEmpty(resultMessage))
                    {
                        ViewBag.SuccessMessage = resultMessage;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "An error occurred while creating the country.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the country.";
            }

            return View();
        }


        public IActionResult Edit(string CountryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM CountryMaster WHERE CountryId = @CountryId";
                var country = connection.QueryFirstOrDefault<Countrys>(query, new { CountryId = CountryId });

                if (country == null)
                {
                    return NotFound();
                }

                return View(country);
            }
        }

        [HttpPost]
        public IActionResult Edit(Countrys country)
        {

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Use DynamicParameters to define both input and output parameters
                    var parameters = new DynamicParameters();
                    parameters.Add("@CountryId", country.CountryId);
                    parameters.Add("@CountryName", country.CountryName);
                    parameters.Add("@CountryCode", country.CountryCode);
                    parameters.Add("@IsActive", country.IsActive);
                    parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    // Execute the stored procedure with parameters
                    connection.Execute("UpdateCountry", parameters, commandType: CommandType.StoredProcedure);

                    // Access the output parameter after the execution
                    var resultMessage = parameters.Get<string>("@ResultMessage");

                    if (!string.IsNullOrEmpty(resultMessage))
                    {
                        ViewBag.SuccessMessage = resultMessage;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "An error occurred while updating the country.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while updating the country.";
            }


            return View(country);
        }


        [HttpGet]
        public IActionResult Delete(string countryId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Use DynamicParameters to define both input and output parameters
                    var parameters = new DynamicParameters();
                    parameters.Add("@CountryId", countryId);
                    parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    // Execute the stored procedure with parameters
                    connection.Execute("DeleteCountry", parameters, commandType: CommandType.StoredProcedure);

                    // Access the output parameter after the execution
                    var resultMessage = parameters.Get<string>("@ResultMessage");

                    if (!string.IsNullOrEmpty(resultMessage))
                    {
                        ViewBag.ErrorMessage = resultMessage;

                    }
                    else
                    {
                        ViewBag.ErrorMessage = resultMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM CountryMaster";
                var countries = connection.Query<Countrys>(query);

                return View("Index", countries);
            }

        }

    }
}
