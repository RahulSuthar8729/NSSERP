using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSSERP.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NSSERP.Areas.Masters.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using NSSERP.Areas.NationalGangotri.Models;
using NSSERP.DbFunctions;
using Newtonsoft.Json;
using static NSSERP.Areas.NationalGangotri.Models.ReceiptDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;

namespace NSSERP.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class DonorInstructionMasterController : Controller
    {
        private readonly string _connectionString;
        private readonly DbClass _dbfunctions;
        public DonorInstructionMasterController(IConfiguration configuration, DbClass dbClass)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _dbfunctions = dbClass;
        }

        public IActionResult Index()
        {
            var modellist = _dbfunctions.GetDonorINstructionsMaster();
            return View(modellist);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DonorInstructionMaster model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Use DynamicParameters to define both input and output parameters
                    var parameters = new DynamicParameters();
                    parameters.Add("@InstructionName", model.InstructionName);
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    // Execute the stored procedure with parameters
                    connection.Execute("InsertDonorInstruction", parameters, commandType: CommandType.StoredProcedure);

                    // Access the output parameter after the execution
                    var resultMessage = parameters.Get<string>("@ResultMessage");

                    if (!string.IsNullOrEmpty(resultMessage))
                    {
                        ViewBag.msg = resultMessage;
                    }
                    else
                    {
                        ViewBag.emsg = "An error occurred while creating the country.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the country.";
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var model = _dbfunctions.GetDonorInstructionsbyid(id);
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving donor instruction details.";
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(DonorInstructionMaster model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Use DynamicParameters to define both input and output parameters
                    var parameters = new DynamicParameters();
                    parameters.Add("@REF_ID", model.REF_ID);
                    parameters.Add("@InstructionName", model.InstructionName);
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    // Execute the stored procedure with parameters
                    connection.Execute("UpdateDonorInstruction", parameters, commandType: CommandType.StoredProcedure);

                    // Access the output parameter after the execution
                    var resultMessage = parameters.Get<string>("@ResultMessage");

                    if (!string.IsNullOrEmpty(resultMessage))
                    {
                        ViewBag.msg = resultMessage;
                    }
                    else
                    {
                        ViewBag.emsg = "An error occurred while updating the donor instruction.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while updating the donor instruction.";
            }

            return View();
        }


        public IActionResult Delete(int refId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("DeleteDonorInstruction", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@REF_ID", refId));

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index", "DonorInstructionMaster");
        }
    }
}
