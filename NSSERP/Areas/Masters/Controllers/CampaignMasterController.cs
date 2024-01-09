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
    public class CampaignMasterController : Controller
    {
        private readonly string _connectionString;
        private readonly DbClass _dbfunctions;
        public CampaignMasterController(IConfiguration configuration, DbClass dbClass)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _dbfunctions = dbClass;
        }

        public IActionResult Index()
        {
            var modellist = _dbfunctions.GetallCampaigns();
            return View(modellist);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CampaignMaster model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Use Dapper's DynamicParameters to define both input and output parameters
                    var parameters = new DynamicParameters();
                    parameters.Add("@CampaignName", model.CampaignName);
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                    parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    // Execute the stored procedure with parameters
                    connection.Execute("InsertCampaign", parameters, commandType: CommandType.StoredProcedure);

                    // Access the output parameter after the execution
                    var resultMessage = parameters.Get<string>("@ResultMessage");

                    if (!string.IsNullOrEmpty(resultMessage))
                    {
                        ViewBag.msg = resultMessage;
                    }
                    else
                    {
                        ViewBag.emsg = "An error occurred while creating the campaign.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the campaign.";
            }

            return View();
        }


        [HttpPost]
        public IActionResult Edit(CampaignMaster model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Use Dapper's DynamicParameters to define both input and output parameters
                var parameters = new DynamicParameters();
                parameters.Add("@REF_ID", model.REF_ID);
                parameters.Add("@CampaignName", model.CampaignName);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                // Execute the stored procedure with parameters
                connection.Execute("UpdateCampaign", parameters, commandType: CommandType.StoredProcedure);

                // Access the output parameter after the execution
                var resultMessage = parameters.Get<string>("@ResultMessage");

                if (!string.IsNullOrEmpty(resultMessage))
                {
                    ViewBag.msg = resultMessage;
                }
                else
                {
                    ViewBag.emsg = "An error occurred while updating the campaign.";
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var model = _dbfunctions.GetCampaignsById(id);
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving donor instruction details.";
                return View();
            }
        }


        public IActionResult Delete(int refId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@REF_ID", refId);
                parameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                connection.Execute("DeleteCampaignById", parameters, commandType: CommandType.StoredProcedure);

                var resultMessage = parameters.Get<string>("@ResultMessage");

                if (!string.IsNullOrEmpty(resultMessage))
                {
                    // Handle the result message as needed
                }
            }
            return RedirectToAction("Index");
        }


    }
}