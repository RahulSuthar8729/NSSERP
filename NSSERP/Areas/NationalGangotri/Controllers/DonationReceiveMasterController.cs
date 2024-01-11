﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSSERP.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NSSERP.Areas.Masters.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using NSSERP.Areas.NationalGangotri.Models;
using NSSERP.DbFunctions;
using Newtonsoft.Json;
using static NSSERP.Areas.NationalGangotri.Models.ReceiptDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System.Net;
using Mono.TextTemplating;
using System.Text;
using Azure.Core;
using System.Text.Json;
namespace NSSERP.Areas.NationalGangotri.Controllers
{
    [Authorize]
    [Area("NationalGangotri")]
    public class DonationReceiveMasterController : Controller
    {
        private readonly string _connectionString;
        private readonly DbClass _dbFunctions;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public DonationReceiveMasterController(DbClass dbFunctions, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _dbFunctions = dbFunctions;
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Home(int id)
        {


            var response = await _apiClient.GetAsync($"api/DonationReceiveMaster/ViewDetails?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    // Handle other error cases if needed
                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var detail = json != null ? JsonConvert.DeserializeObject<DonationReceiveMaster>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);

        }
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetEvents1(string q)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var events = connection.Query<Events>("GetEvents", commandType: CommandType.StoredProcedure);

                    if (!string.IsNullOrEmpty(q))
                    {
                        events = events.Where(e => e.EventName.Contains(q, StringComparison.OrdinalIgnoreCase));
                    }

                    return Json(events.ToList());
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving events.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStatesByCountry(int countryId)
        {

            try
            {
                string apiUrl = $"api/DonationReceiveMaster/GetStatesByCountry?countryId={countryId}";

                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return Json(new { data = json });
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {

                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDistrictsByState(int stateId)
        {
            try
            {
                string apiUrl = $"api/DonationReceiveMaster/GetDistrictsByState?stateId={stateId}";

                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return Json(new { data = json });
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {

                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesByDistrictID(int districtId)
        {
            try
            {
                string apiUrl = $"api/DonationReceiveMaster/GetCitiesByDistrictID?districtId={districtId}";

                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return Json(new { data = json });
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {

                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSubHeadByHead(string HeadID, string DataFlag)
        {
            try
            {
                // Construct the API endpoint URL with the pincode parameter
                string apiUrl = $"api/DonationReceiveMaster/GetSubHeadByHead?HeadID={HeadID}&DataFlag={DataFlag}";

                // Make a GET request to the API endpoint
                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON content from the response
                    var json = await response.Content.ReadAsStringAsync();

                    var subHeadList = JsonConvert.DeserializeObject<List<SubHeadMaster>>(json);


                    // Return JsonResult with structured data
                    return Json(new { data = subHeadList });
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Handle the case where the resource was not found
                    return NotFound();
                }
                else
                {
                    // Handle other error cases if needed
                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetQtyAmtBySubHead(string YojnaID, string DataFlag)
        {
            try
            {
                // Construct the API endpoint URL with the pincode parameter
                string apiUrl = $"api/DonationReceiveMaster/GetQtyAmtBySubHead?YojnaID={Convert.ToInt32(YojnaID)}&DataFlag={DataFlag}";

                // Make a GET request to the API endpoint
                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON content from the response
                    var json = await response.Content.ReadAsStringAsync();

                    // Return JsonResult with structured data
                    return Json(new { data = json });
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Handle the case where the resource was not found
                    return NotFound();
                }
                else
                {
                    // Handle other error cases if needed
                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationDetailsByPinCode(string pincode)
        {
            try
            {
                // Construct the API endpoint URL with the pincode parameter
                string apiUrl = $"api/DonationReceiveMaster/GetLocationDetailsByPinCode?pincode={pincode}";

                // Make a GET request to the API endpoint
                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON content from the response
                    var json = await response.Content.ReadAsStringAsync();

                    // Return JsonResult instead of a raw JSON string
                    return Json(new { data = json });
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Handle the case where the resource was not found
                    return NotFound();
                }
                else
                {
                    // Handle other error cases if needed
                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDataByDonorID(string DonorID)
        {
            try
            {
                string apiUrl = $"api/DonationReceiveMaster/GetDataByDonorID?DonorID={DonorID}";

                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return Json(new { data = json });
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {

                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("An error occurred while retrieving location details.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Home(DonationReceiveMaster model)
        {
            bool? isDifferent = model.IsReceiveHeadDiffrent;
            string ReceiveDepartment = isDifferent == true ? model.ReceiveHeadName : User.FindFirst("Department")?.Value;
            decimal Amount = model.PaymentModeName == "CASH" ? model.Amount : model.TotalAmount;
            DateTime dob = model.DateOfBirth ?? DateTime.MinValue;
            DateTime provdate = model.ProvDate ?? DateTime.MinValue;
            string FinYear = User.FindFirst("FinYear")?.Value ?? string.Empty;
            string UserID = User.FindFirst("UserID")?.Value ?? string.Empty;

            string Doc1 = string.Empty;
            string Doc2 = string.Empty;
            string Doc3 = string.Empty;

            if (model.DocProvisonal != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.DocProvisonal.FileName);
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "DocDonationReceive");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Doc1 = fileName;
                    model.DocProvisonal.CopyTo(stream);
                }
            }
            if (model.DocCheque != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.DocCheque.FileName);
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "DocDonationReceive");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Doc2 = fileName;
                    model.DocCheque.CopyTo(stream);
                }
            }
            if (model.DocPayInSlip != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.DocPayInSlip.FileName);
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "DocDonationReceive");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Doc3 = fileName;
                    model.DocPayInSlip.CopyTo(stream);
                }
            }

            var requestData = new
            {
                IsDifferent = isDifferent,
                ReceiveDepartment = ReceiveDepartment,
                FinYear = FinYear,
                UserID = UserID,
                ReceiveDate = model.ReceiveDate,
                ReceiveHeadID = model.ReceiveHeadID,
                UserName = User.FindFirst(ClaimTypes.Name)?.Value,
                ID = model.ID,
                EventName = model.EventName,
                CampaignID = model.CampaignID,
                CampaignName = model.CampaignName,
                DonorID = model.DonorID,
                InMemory = model.InMemory,
                NamePrefix = model.NamePrefix,
                FullName = model.FullName,
                PrefixToFullName = model.PrefixToFullName,
                RelationToFullName = model.RelationToFullName,
                DateOfBirth = dob,
                Company = model.Company,
                FullAddress = model.FullAddress,
                PinCode = model.PinCode,
                CountryId = model.CountryId,
                CountryName = model.CountryName,
                StateID = model.StateID,
                StateName = model.StateName,
                DistrictID = model.DistrictID,
                DistrictName = model.DistrictName,
                CityID = model.CityID,
                CityName = model.CityName,
                IfUpdationInAddress = model.IfUpdationInAddress,
                IsPermanentAddressDiffrent = model.IsPermanentAddressDiff,
                ifdetailsNotComplete = model.IfDetailsNotComplete,
                P_FullAddress = model.P_FullAddress,
                P_PinCode = model.P_PinCode,
                P_CountryID = model.P_CountryID,
                P_CountryName = model.P_CountryName,
                P_StateID = model.P_StateID,
                P_StateName = model.P_StateName,
                P_DistrictID = model.P_DistrictID,
                P_DistrictName = model.P_DistrictName,
                P_CityID = model.P_CityID,
                P_CityName = model.P_CityName,
                EmailID = model.EmailID,
                StdCode = model.StdCode,
                PhoneR = model.PhoneR,
                ProvNo = model.ProvNo,
                provdate = provdate,
                PersonName = model.PersonName,
                paymentModeID = model.PaymentModeID,
                PaymentModeName = model.PaymentModeName,
                CurrencyID = model.CurrencyID,
                CurrencyCode = model.CurrencyCode,
                Amount = Amount,
                MaterialDepositID = model.MaterialDepositID,
                Material = model.Material,
                IsManavaFormulaRequire = model.IsManavaFormulaRequire,
                IsPatientsPhotoRequire = model.IsPatientsPhotoRequire,
                IfDiffrentAddressForDispatch = model.IfDiffrentAddressForDispatch,
                IfAnnounceDueInFuture = model.IfAnnounceDueInFuture,
                Doc1 = Doc1,
                Doc2 = Doc2,
                Doc3 = Doc3

            };


            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(requestData);
                string apiUrl = "api/DonationReceiveMaster/InsertData";
                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

                requestContent.Headers.Add("MobileList", model.MobileList);
                requestContent.Headers.Add("IdentityList", model.IdentityList);
                requestContent.Headers.Add("BankDetailsList", model.BankDetailsList);
                requestContent.Headers.Add("receiptdetailslist", model.receiptdetailslist);
                requestContent.Headers.Add("AnnounceDetsilsList", model.AnnounceDetsilsList);
                requestContent.Headers.Add("donorInstructionjsonList", model.donorInstructionjsonList);
                requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    var modeldata = modelget != null ? JsonConvert.DeserializeObject<DonationReceiveMaster>(modelget) : null;
                    return View(modeldata);
                }

                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }


            }
            catch (Exception ex)
            {
                // Handle other exceptions, such as database connection issues
                ViewBag.emsg = $"An error occurred: {ex.Message}";
            }

            return View();
        }

        public async Task<IActionResult> PrintProvisionalReceipt(int refid)
        {
            var response = await _apiClient.GetAsync($"api/DonationReceiveMaster/GetProvisionalReceipt?refid={refid}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    // Handle other error cases if needed
                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
            var json = await response.Content.ReadAsStringAsync();
            var detail = json != null ? JsonConvert.DeserializeObject<ProvisionalReceiptModel>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);

        }
    }
}