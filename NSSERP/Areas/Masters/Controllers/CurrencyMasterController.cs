using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class CurrencyMasterController : Controller
    {
        private readonly string _connectionString;

        public CurrencyMasterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var currencies = dbConnection.Query<CurrencyMaster, Countrys, CurrencyMaster>(
                    "GetCurrencyListWithCountry",
                    (currency, country) =>
                    {
                        currency.CountryMaster = country;
                        return currency;
                    },
                    splitOn: "CountryID",
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return View(currencies);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = GetCountries();
            return View();
        }
        [HttpPost]
        public IActionResult create(CurrencyMaster model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@CurrencyCode", model.CurrencyCode);
                    parameters.Add("@CurrencyName", model.CurrencyName);
                    parameters.Add("@Symbol", model.Symbol);
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@CountryID", model.CountryID);
                    parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("InsertCurrency", parameters, commandType: CommandType.StoredProcedure);

                    int resultCode = parameters.Get<int>("@ResultCode");

                    ViewBag.Countries = GetCountries();
                    // Handle the result code
                    if (resultCode == 1)
                    {
                        ViewBag.msg = "Currency Inserted Successfully";
                    }
                    else if (resultCode == 0)
                    {
                        ViewBag.emsg = "Error While Insert";
                    }
                    else if (resultCode == -1)
                    {
                        ViewBag.emsg = "Currency Code already exists";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View();
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

        public IActionResult Edit(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { CurrencyID = id };
                var currency = connection.QueryFirstOrDefault<CurrencyMaster>("GetCurrencyDetailsById", parameters, commandType: CommandType.StoredProcedure);

                // Check if currency is not found
                if (currency == null)
                {
                    return NotFound();
                }

                ViewBag.Countries = GetCountries();

                return View(currency);
            }
        }

        [HttpPost]
        public IActionResult Edit(CurrencyMaster model)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Create DynamicParameters to pass parameters to the stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@CurrencyID", model.CurrencyID);
                parameters.Add("@CurrencyCode", model.CurrencyCode);
                parameters.Add("@CurrencyName", model.CurrencyName);
                parameters.Add("@Symbol", model.Symbol);
                parameters.Add("@CountryID", model.CountryID);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Call the stored procedure to update currency details
                connection.Execute("UpdateCurrencyMaster", parameters, commandType: CommandType.StoredProcedure);

                // Get the result code from the output parameter
                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    ViewBag.msg = "Currency Updated Successfully";
                }
                else if (resultCode == 0)
                {
                    ViewBag.emsg = "Error While Update";
                }
                else if (resultCode == -1)
                {
                    ViewBag.emsg = "Currency ID not found or invalid";
                }

            }
            var countries = GetCountries();
            ViewBag.Countries = countries;

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new { CurrencyID = id };
                    connection.Execute("DeleteCurrencyById", parameters, commandType: CommandType.StoredProcedure);

                    ViewBag.msg = "Currency Deleted Successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.emsg = "Error While Deleting Currency";
                // Log the exception if needed
                return RedirectToAction("Index");
            }
        }


    }
}
