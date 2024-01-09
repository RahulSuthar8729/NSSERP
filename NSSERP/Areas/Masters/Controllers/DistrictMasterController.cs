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
using System.Diagnostics;

namespace NSSERP.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class DistrictMasterController : Controller
    {
        private readonly string _connectionString;

        public DistrictMasterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var Districts = GetDistrictwithcountryandState();
            return View(Districts);
        }
        public IActionResult Create()
        {
            var Districtmodel = new DistrictMaster
            {
                Countrys = GetCountries(),
                States = GetStates(),
                //Citys = GetActiveCities()
            };

            return View(Districtmodel);
        }

        [HttpPost]
        public IActionResult Create(DistrictMaster district)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@DistrictName", district.DistrictName);
                   // parameters.Add("@CityID", district.CityID);
                    parameters.Add("@StateID", district.StateID);
                    parameters.Add("@CountryID", district.CountryID);
                    parameters.Add("@IsActive", district.IsActive);
                    parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("InsertDistrict", parameters, commandType: CommandType.StoredProcedure);

                    int resultCode = parameters.Get<int>("@ResultCode");

                    var districtModel = new DistrictMaster
                    {
                        Countrys = GetCountries(),
                        States = GetStates(),
                       // Citys = GetActiveCities()
                    };

                    if (resultCode == 1)
                    {
                        ViewBag.msg = "District Inserted Successfully";
                        return View(districtModel);
                    }
                    else if (resultCode == 0)
                    {
                        ViewBag.emsg = "Error While Insert";
                        return View(districtModel);
                    }
                    else if (resultCode == -1)
                    {
                        ViewBag.emsg = "District already exists";
                        return View(districtModel);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors, log, or show a user-friendly message
                ViewBag.ErrorMessage = ex.Message;
                var districtModel = new DistrictMaster
                {
                    Countrys = GetCountries(),
                    States = GetStates(),
                    //Citys = GetActiveCities()
                };

            }
            return View();
        }

        public IActionResult Edit(int id)
        {

            var districtMaster = GetDistrictById(id);

            if (districtMaster == null)
            {
                return NotFound(); // or handle accordingly
            }

            var editModel = new DistrictMaster
            {
                DistrictID = districtMaster.DistrictID,
                DistrictName = districtMaster.DistrictName,
                CountryID = districtMaster.CountryID,
                StateID = districtMaster.StateID,
                CityID = districtMaster.CityID,
                IsActive = districtMaster.IsActive
            };
            editModel.Countrys = GetCountries();
            editModel.States = GetStates();
            editModel.Citys = GetActiveCities();
            return View(editModel);

        }

        [HttpPost]
        public IActionResult Edit(DistrictMaster model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Add an output parameter to indicate success or failure
                    var parameters = new DynamicParameters();
                    parameters.Add("@DistrictID", model.DistrictID);
                    parameters.Add("@DistrictName", model.DistrictName);
                    parameters.Add("@CountryID", model.CountryID);
                    parameters.Add("@StateID", model.StateID);
                  //  parameters.Add("@CityID", model.CityID);  // Add CityID parameter
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    // Execute stored procedure to update district using Dapper
                    connection.Execute("UpdateDistrict", parameters, commandType: CommandType.StoredProcedure);

                    int result = parameters.Get<int>("@Result");

                    if (result == 1)
                    {
                        var updatedDistrict = GetDistrictById(model.DistrictID);
                        updatedDistrict.Countrys = GetCountries();
                        updatedDistrict.States = GetStates();
                       // updatedDistrict.Citys = GetActiveCities();  // Add a method to get cities

                        ViewBag.msg = "District updated successfully.";
                        return View(updatedDistrict); // Return the view with the updated data
                    }
                    else if (result == -1)
                    {
                        // Another district with the same name, country, state, and city already exists
                        ViewBag.emsg = "Another district with the same name, country, state, and city already exists.";
                    }
                    else
                    {
                        // Other failure, handle accordingly
                        ViewBag.emsg = "Failed to update the district.";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors, log, or show a user-friendly message
                ViewBag.ErrorMessage = ex.Message;
            }

            // If ModelState is not valid or an error occurred, reload the view with the model
            model.Countrys = GetCountries();
            model.States = GetStates();
           // model.Citys = GetActiveCities();

            return View(model);
        }

        // In your Controller

        [HttpGet]
        public IActionResult DeleteDistrict(int id)
        {
            bool isDeleted = DeleteDistrictbyid(id);

            // Set TempData based on the result of the Delete operation
            if (isDeleted)
            {
                ViewBag.msg = "District deleted successfully.";
            }
            else
            {
                ViewBag.emsg = "Failed to delete the district.";
            }
            return RedirectToAction("Index");
        }

        private bool DeleteDistrictbyid(int id)
        {
            bool msg;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DeleteDistrictProcedure", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DistrictID", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected >= 0)
                        {
                            ViewBag.SuccessMessage = "Deleted Successfully";
                            msg = true;
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Error While Deleting";
                            msg = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or show a user-friendly message
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                msg = false;
            }

            return msg;
        }

        [HttpGet]
        public IActionResult GetStatesByCountryJson(int countryId)
        {
            var states = GetStatesByCountry(countryId);
            return Json(states);
        }
        private IEnumerable<StateMaster> GetStatesByCountry(int countryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Use Dapper to call the stored procedure
                return connection.Query<StateMaster>("GetStatesByCountry", new { CountryID = countryId }, commandType: CommandType.StoredProcedure);
            }
        }

        public IActionResult GetCityInJSon(int stateId)
        {
            var cities = GetCitiesbyStateID(stateId);
            return Json(cities);
        }
        private IEnumerable<CityMaster> GetCitiesbyStateID(int stateId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<CityMaster>("GetCitiesByState", new { StateID = stateId }, commandType: CommandType.StoredProcedure);
            }
        }

        private IEnumerable<DistrictMaster> GetDistrictwithcountryandState()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<DistrictMaster>("GetDistrictsWithCountryAndState", commandType: CommandType.StoredProcedure);
            }
        }

        private DistrictMaster GetDistrictById(int districtId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Adjust your SQL query to retrieve district by ID
                return connection.QueryFirstOrDefault<DistrictMaster>("SELECT * FROM DistrictMaster WHERE DistrictID = @DistrictID", new { DistrictID = districtId });
            }
        }
        private List<Countrys> GetCountries()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get countries
                return connection.Query<Countrys>("GetCountries", commandType: CommandType.StoredProcedure).AsList();
            }
        }
        private List<StateMaster> GetStates()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get states
                return connection.Query<StateMaster>("GetStates", commandType: CommandType.StoredProcedure).AsList();
            }
        }
        private List<CityMaster> GetActiveCities()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Execute stored procedure to get active cities
                return connection.Query<CityMaster>("GetActiveCities", commandType: CommandType.StoredProcedure).AsList();
            }
        }

    }
}
