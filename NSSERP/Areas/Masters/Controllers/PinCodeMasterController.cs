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
    public class PinCodeMasterController : Controller
    {
        private readonly string _connectionString;

        public PinCodeMasterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var pinCodeDetails = connection.Query<PinCodeMaster, CityMaster, StateMaster, Countrys, PinCodeMaster>(
    "GetPinCodesWithDetails",
    (pinCode, city, state, country) =>
    {
        pinCode.citys = new List<CityMaster> { city };
        pinCode.StateName = state?.State_Name;
        pinCode.CountryName = country?.Country_Name;
        return pinCode;
    },
    splitOn: "CityID,StateID,CountryID", // Adjust these based on your actual column names
    commandType: CommandType.StoredProcedure
);



                    return View(pinCodeDetails);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or display an error message
                ViewBag.ErrorMessage = ex.Message;
                return View(new List<PinCodeMaster>());
            }
        }

        public IActionResult Create()
        {
            var City = new PinCodeMaster
            {
                citys = GetActiveCities()
            };

            return View(City);
        }
        [HttpPost]
        public IActionResult Create(PinCodeMaster model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Pincode", model.Pincode);
                    parameters.Add("@CityID", model.CityID);
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("InsertPincode", parameters, commandType: CommandType.StoredProcedure);

                    int resultCode = parameters.Get<int>("@ResultCode");

                    var getmodel = new PinCodeMaster
                    {
                        citys = GetActiveCities()
                    };
                    if (resultCode == 1)
                    {
                        ViewBag.msg = "Pincode Inserted Successfully";
                        return View(getmodel);
                    }
                    else if (resultCode == 0)
                    {
                        ViewBag.emsg = "Error While Insert";
                        return View(getmodel);
                    }
                    else if (resultCode == -1)
                    {
                        ViewBag.emsg = "Pincode already exists";
                        return View(getmodel);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors, log, or show a user-friendly message
                ViewBag.ErrorMessage = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { PincodeID = id };
                var pinCodeDetails = connection.QueryFirstOrDefault<PinCodeMaster>("GetPinCodeDetailsById", parameters, commandType: CommandType.StoredProcedure);

                // Assuming you have a method to get active cities
                var cities = GetActiveCities();

                var model = new PinCodeMaster
                {
                    PincodeID = id,
                    Pincode = pinCodeDetails.Pincode,
                    CityID = pinCodeDetails.CityID,
                    IsActive = pinCodeDetails.IsActive,
                    citys = cities
                };

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Edit(PinCodeMaster model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@PincodeID", model.PincodeID);
                    parameters.Add("@Pincode", model.Pincode);
                    parameters.Add("@CityID", model.CityID);
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("UpdatePinCodeMaster", parameters, commandType: CommandType.StoredProcedure);

                    int resultCode = parameters.Get<int>("@ResultCode");

                    // Handle the result code
                    if (resultCode == 1)
                    {
                        ViewBag.msg = "Updated Successfully";
                    }
                    else if (resultCode == 0)
                    {
                        ViewBag.msg = "No Chnage ";
                    }
                    else
                    {
                        ViewBag.emsg = "Error";
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle errors, log, or show a user-friendly message
                ViewBag.ErrorMessage = ex.Message;
            }

            // If something went wrong, return to the Edit view with the model data
            model.citys = GetActiveCities();
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            bool isDeleted = deletepincode(id);

            // Set TempData based on the result of the Delete operation
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "PinCode deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete the PinCode.";
            }
            return RedirectToAction("Index");
        }
        private bool deletepincode(int id)
        {
            bool msg;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM PinCodeMaster WHERE PincodeID = @id", connection))
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
