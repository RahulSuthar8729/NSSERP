using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSSERP.Areas.Masters.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace NSSERP.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class StateMasterController : Controller
    {

        private readonly string _connectionString;

        public StateMasterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()

        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            else if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();               
                var states = connection.Query<StateMaster, Countrys, StateMaster>(
                    "GetStatesWithCountries",
                    (state, country) =>
                    {
                        state.CountryMaster = country;
                        return state;
                    },
                    splitOn: "CountryId",
                    commandType: CommandType.StoredProcedure
                );

                return View(states);
            }

        }
        public IActionResult Create()
        {
            ViewBag.Countries = GetCountries();
            return View();
        }

        [HttpPost]
        public IActionResult Create(StateMaster state)
        {
            bool isSuccess = InsertState(state);

            if (isSuccess)
            {

            }
            else
            {
                // Handle the case where insertion was not successful
                ModelState.AddModelError(string.Empty, "Failed to insert the state.");
            }

            ViewBag.Countries = GetCountries();

            return View(state);
        }

        public IActionResult Edit(int id)
        {
            // Fetch the state by ID
            var state = GetStateById(id);

            if (state == null)
            {
                return NotFound(); // or handle accordingly
            }

            ViewBag.Countries = GetCountries();

            // Render the edit view with the pre-filled data
            return View("Edit", state);
        }

        [HttpPost]
        public IActionResult Edit(StateMaster state)
        {
            bool isSuccess = UpdateState(state);

            if (isSuccess)
            {


            }
            else
            {

            }


            ViewBag.Countries = GetCountries();
            return View(state);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            bool isDeleted = DeleteState(id);

            // Set TempData based on the result of the Delete operation
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "State deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete the state.";
            }
            return RedirectToAction("Index");
        }

        public List<Countrys> GetCountries()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var countries = connection.Query<Countrys>("GetCountries", commandType: CommandType.StoredProcedure);
                return countries.ToList();
            }
        }


        private StateMaster GetStateById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM StateMaster WHERE StateID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StateMaster
                            {
                                StateID = Convert.ToInt32(reader["StateID"]),
                                StateName = reader["StateName"].ToString(),
                                IsActive = Convert.ToChar(reader["IsActive"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                CountryId = reader["CountryID"] is DBNull ? null : (int?)reader["CountryID"]
                            };
                        }
                        return null;
                    }
                }
            }
        }

        private bool InsertState(StateMaster state)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new
                    {
                        StateName = state.StateName,
                        IsActive = state.IsActive,
                        CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value,
                        CountryID = state.CountryId,
                        StateID = 0 // Initialize the output parameter
                    };

                    // Add the output parameter to the Dapper DynamicParameters
                    var dynamicParameters = new DynamicParameters(parameters);
                    dynamicParameters.Add("StateID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("InsertState", dynamicParameters, commandType: CommandType.StoredProcedure);

                    int insertedStateID = dynamicParameters.Get<int>("StateID");
                    if (insertedStateID > 0)
                    {
                        ViewBag.SuccessMessage = "Successfully Inserted";
                    }
                    else if (insertedStateID == -1)
                    {
                        ViewBag.ErrorMessage = "State Already Exists";
                    }
                    return insertedStateID > 0; // If insertedStateID > 0, the insertion was successful
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return false;
            }
        }


        private bool UpdateState(StateMaster state)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new
                    {
                        StateID = state.StateID,
                        StateName = state.StateName,
                        IsActive = state.IsActive,
                        CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value,
                        CountryID = state.CountryId ?? (object)DBNull.Value
                    };

                    var affectedRows = connection.Execute("UpdateStateProcedure", parameters, commandType: CommandType.StoredProcedure);
                    if (affectedRows > 0)
                    {
                        ViewBag.SuccessMessage = "State Update Successfully";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error While Updating";
                    }
                    return affectedRows > 0; // If affectedRows > 0, the update was successful
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return false;
            }
        }


        private bool DeleteState(int id)
        {
            bool msg;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM StateMaster WHERE StateID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int a = command.ExecuteNonQuery();
                    if (a > 0)
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
            return msg;
        }
    }
}
