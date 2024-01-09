using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSSERP.Areas.Masters.Models;
using NuGet.Protocol.Core.Types;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace NSSERP.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class PaymentModeMasterController : Controller
    {
        private readonly string _connectionString;

        public PaymentModeMasterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            var paymentModes = GetPaymentModes();
            return View(paymentModes);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(PaymentModeMaster paymentMode)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@PaymentModeName", paymentMode.PaymentModeName);
                parameters.Add("@Description", paymentMode.Description);
                parameters.Add("@IsActive", paymentMode.IsActive);
                parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Execute the stored procedure
                connection.Execute("InsertPaymentMode", parameters, commandType: CommandType.StoredProcedure);

                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    ViewBag.msg = "Payment Mode Inserted Successfully";
                    return View();
                }
                else if (resultCode == 0)
                {
                    ViewBag.emsg = "Error While Inserting Payment Mode";
                }
                else if (resultCode == -1)
                {
                    ViewBag.emsg = "Payment Mode Name already exists";
                }
            }

            return View(paymentMode);
        }
        public IEnumerable<PaymentModeMaster> GetPaymentModes()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<PaymentModeMaster>("GetPaymentModes", commandType: CommandType.StoredProcedure);
            }
        }

        public IActionResult Edit(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { PaymentModeID = id };
                var paymentMode = connection.QueryFirstOrDefault<PaymentModeMaster>("GetPaymentModeDetailsById", parameters, commandType: CommandType.StoredProcedure);

                // Check if payment mode is not found
                if (paymentMode == null)
                {
                    return NotFound();
                }

                return View(paymentMode);
            }
        }

        [HttpPost]
        public IActionResult Edit(PaymentModeMaster model)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Create DynamicParameters to pass parameters to the stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@PaymentModeID", model.PaymentModeID);
                parameters.Add("@PaymentModeName", model.PaymentModeName);
                parameters.Add("@Description", model.Description);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Call the stored procedure to update payment mode details
                connection.Execute("UpdatePaymentModeMaster", parameters, commandType: CommandType.StoredProcedure);

                // Get the result code from the output parameter
                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    ViewBag.msg = "Payment Mode Updated Successfully";
                }
                else if (resultCode == 0)
                {
                    ViewBag.emsg = "Error While Update";
                }
                else if (resultCode == -1)
                {
                    ViewBag.emsg = "Payment Mode ID not found or invalid";
                }
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@PaymentModeID", id);
                parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Call the stored procedure to delete payment mode
                connection.Execute("DeletePaymentModeMaster", parameters, commandType: CommandType.StoredProcedure);

                int resultCode = parameters.Get<int>("@ResultCode");

                if (resultCode == 1)
                {
                    ViewBag.msg = "Payment Mode Deleted Successfully";
                }
                else if (resultCode == -1)
                {
                    ViewBag.emsg = "Payment Mode ID not found or invalid";
                }
            }

            // Redirect to the index page or any other appropriate action
            return RedirectToAction("Index");
        }

    }
}
