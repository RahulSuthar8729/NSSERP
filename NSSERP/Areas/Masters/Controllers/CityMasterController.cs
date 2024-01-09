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
using NSSERP.DbFunctions;

namespace NSSERP.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class CityMasterController : Controller
    {
        private readonly string _connectionString;
        private readonly DbClass _dbFunctions;
        public CityMasterController(DbClass dbFunctions, IConfiguration configuration)
        {
            _dbFunctions = dbFunctions;
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var cities = GetCitiesWithCountryAndState();
            return View(cities);
        }
        public IActionResult Create()
        {
            var citymodel = new CityMaster
            {
                Countries = GetCountries(),
                States = GetStates()
            };

            return View(citymodel);
        }
        [HttpPost]
        public IActionResult Create(CityMaster city)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Add an output parameter to indicate success or failure
                    var parameters = new DynamicParameters();
                    parameters.Add("@CityName", city.CityName);
                    parameters.Add("@CountryID", city.CountryID);
                    parameters.Add("@StateID", city.StateID);
                    parameters.Add("@DistrictID", city.DistrictID);
                    parameters.Add("@IsActive", city.IsActive);
                    parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    // Execute stored procedure to insert city
                    connection.Execute("InsertCity", parameters, commandType: CommandType.StoredProcedure);

                    int result = parameters.Get<int>("@Result");
                    var citymodel = new CityMaster
                    {
                        Countries = GetCountries()
                    };

                    if (result == -1)
                    {
                        // City already exists, handle accordingly
                        ViewBag.emsg = "City with the same name, country, and state already exists.";
                        return View(citymodel);
                    }
                    else if (result == 1)
                    {
                        // City inserted successfully
                        ViewBag.msg = "City inserted successfully.";
                        return View(citymodel);
                    }
                    else
                    {
                        // Other failure, handle accordingly
                        ViewBag.emsg = "Failed to insert the city.";
                        return View(citymodel);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors, log, or show a user-friendly message
                ViewBag.ErrorMessage = ex.Message;
                return View(city);
            }
        }

        [HttpGet]
        public IActionResult Edit(int? cityId)
        {
            // Check if cityId is null or not
            if (!cityId.HasValue)
            {
                // Handle the case where cityId is not provided
                return RedirectToAction("Index");
            }

            // Convert cityId to non-nullable int before passing to GetCityById
            var city = GetCityById(cityId.Value);

            if (city == null)
            {
                // Handle the case where the city is not found
                return RedirectToAction("Index");
            }

            var editModel = new CityMaster
            {
                CityID = city.CityID,
                CityName = city.CityName,
                CountryID = city.CountryID,
                StateID = city.StateID,
                IsActive = city.IsActive
            };

            // Populate dropdowns with countries and states
            editModel.Countries = GetCountries();
            editModel.States = GetStates();
            editModel.Districts = GetDistricts();

            return View(editModel);
        }

        private CityMaster GetCityById(int cityId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Adjust your SQL query to retrieve city by ID
                return connection.QueryFirstOrDefault<CityMaster>("SELECT * FROM CityMaster WHERE CityID = @CityID", new { CityID = cityId });
            }
        }
        
        [HttpPost]
        public IActionResult Edit(CityMaster model)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Add an output parameter to indicate success or failure
                    var parameters = new DynamicParameters();
                    parameters.Add("@CityID", model.CityID);
                    parameters.Add("@CityName", model.CityName);
                    parameters.Add("@CountryID", model.CountryID);
                    parameters.Add("@StateID", model.StateID);
                    parameters.Add("@DistrictID", model.DistrictID);
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@ModifiedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    // Execute stored procedure to update city using Dapper
                    connection.Execute("UpdateCity", parameters, commandType: CommandType.StoredProcedure);

                    int result = parameters.Get<int>("@Result");

                    if (result == 1)
                    {
                        // City updated successfully

                        // Fetch the updated data
                        var updatedCity = GetCityById(model.CityID);

                        // Populate dropdowns with countries and states
                        updatedCity.Countries = GetCountries();
                        updatedCity.States = GetStates();
                        updatedCity.Districts = GetDistricts();
                        ViewBag.msg = "City updated successfully.";
                        return View(updatedCity); // Return the view with the updated data
                    }
                    else if (result == -1)
                    {
                        // Another city with the same name, country, and state already exists
                        ViewBag.emsg = "Another city with the same name, country, and state already exists.";
                    }
                    else
                    {
                        // Other failure, handle accordingly
                        ViewBag.emsg = "Failed to update the city.";
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle errors, log, or show a user-friendly message
                ViewBag.ErrorMessage = ex.Message;
            }

            // If ModelState is not valid or an error occurred, reload the view with the model
            model.Countries = GetCountries();
            model.States = GetStates();

            return View(model);
        }

        public IActionResult Delete(int? cityId)
        {
            if (!cityId.HasValue)
            {
                // Handle the case where cityId is not provided
                return RedirectToAction("Index");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Add an output parameter to capture the result
                    var parameters = new DynamicParameters();
                    parameters.Add("@CityID", cityId);
                    parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    // Execute the stored procedure to delete the city
                    connection.Execute("DeleteCity", parameters, commandType: CommandType.StoredProcedure);

                    // Check the result
                    int result = parameters.Get<int>("@Result");

                    if (result == 1)
                    {
                        // City deleted successfully
                        ViewBag.msg = "City deleted successfully.";
                    }
                    else if (result == -1)
                    {
                        // City not found
                        ViewBag.emsg = "City not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors, log, or show a user-friendly message
                ViewBag.ErrorMessage = ex.Message;
            }

            // Redirect to the index page
            return RedirectToAction("Index");
        }

        private IEnumerable<CityMaster> GetCitiesWithCountryAndState()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.Query<CityMaster>("GetCitiesWithCountryAndState", commandType: CommandType.StoredProcedure);
            }
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
        private List<DistrictMaster> GetDistricts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Execute stored procedure to get states
                return connection.Query<DistrictMaster>("GetDistricts", commandType: CommandType.StoredProcedure).AsList();
            }
        }
        [HttpGet]
        public IActionResult GetDistrictsByState(int stateId)
        {
            try
            {               
                var districts = _dbFunctions.GetDistrictsByState(stateId); 

                // Return districts as JSON
                return Json(districts);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving districts.");
            }
        }
    }
}
