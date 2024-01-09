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
    public class BankMasterController : Controller
    {
        private readonly string _connectionString;

        public BankMasterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var banks = connection.Query<BankMaster>("GetAllBankMasters", commandType: CommandType.StoredProcedure);

                return View(banks);
            }
        }
        public IActionResult Create()
        {
            ViewBag.Countries = GetCountries();
            return View();
        }
        [HttpPost]
        public IActionResult Create(BankMaster bankMaster)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@BankName", bankMaster.BankName);
                parameters.Add("@BranchName", bankMaster.BranchName);
                parameters.Add("@IFSCCode", bankMaster.IFSCCode);
                parameters.Add("@CountryID", bankMaster.CountryID);
                parameters.Add("@IsActive", bankMaster.IsActive);
                parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("InsertBankMaster", parameters, commandType: CommandType.StoredProcedure);

                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    ViewBag.msg = "Bank details inserted successfully.";

                }
                else if (resultCode == -1)
                {
                    ViewBag.emsg = "Bank with the provided IFSC Code already exists.";
                }
                else
                {
                    ViewBag.emsg = "Error while inserting bank details.";
                }
            }
            ViewBag.Countries = GetCountries();
            return View(bankMaster);
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { BankID = id };
                var bank = connection.QueryFirstOrDefault<BankMaster>("GetBankMasterById", parameters, commandType: CommandType.StoredProcedure);

                if (bank == null)
                {
                    return NotFound();
                }

                ViewBag.Countries = GetCountries();

                return View(bank);
            }
        }

        [HttpPost]
        public IActionResult Edit(BankMaster model)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@BankID", model.BankID);
                parameters.Add("@BankName", model.BankName);
                parameters.Add("@BranchName", model.BranchName);
                parameters.Add("@IFSCCode", model.IFSCCode);
                parameters.Add("@CountryID", model.CountryID);
                parameters.Add("@IsActive", model.IsActive);              
                parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("UpdateBankMaster", parameters, commandType: CommandType.StoredProcedure);

                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    ViewBag.msg = "BankMaster Updated Successfully";
                }
                else if (resultCode == 0)
                {
                    ViewBag.emsg = "Error While Update";
                }
                else if (resultCode == -1)
                {
                    ViewBag.emsg = "BankMaster ID not found or invalid";
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

                    var parameters = new { BankID = id };
                    connection.Execute("DeleteBankMasterById", parameters, commandType: CommandType.StoredProcedure);

                    ViewBag.msg = "BankMaster Deleted Successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.emsg = "Error While Deleting BankMaster";
                // Log the exception if needed
                return RedirectToAction("Index");
            }
        }

    }
}
