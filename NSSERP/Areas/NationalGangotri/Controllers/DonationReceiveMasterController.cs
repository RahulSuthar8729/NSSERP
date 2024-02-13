using Microsoft.AspNetCore.Mvc;
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
using Rotativa.AspNetCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
                    Response.WriteAsJsonAsync(response);
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> GetSubHeadByHead(string HeadID, string DataFlag, string CurrencyID)
        {
            try
            {
                // Construct the API endpoint URL with the pincode parameter
                string apiUrl = $"api/DonationReceiveMaster/GetSubHeadByHead?HeadID={HeadID}&DataFlag={DataFlag}&CurrencyId={CurrencyID}";

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
        public async Task<IActionResult> GetQtyAmtBySubHead(string YojnaID, string DataFlag, string CurrencyID)
        {
            try
            {
                // Construct the API endpoint URL with the pincode parameter
                string apiUrl = $"api/DonationReceiveMaster/GetQtyAmtBySubHead?YojnaID={Convert.ToInt32(YojnaID)}&DataFlag={DataFlag}&CurrencyId={CurrencyID}";

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

        [HttpGet]
        public async Task<IActionResult> SearchData(string searchType, string searchData)
        {
            try
            {
                string apiUrl = $"api/Search/SearchDonorDataByPara?searchType={searchType}&searchData={searchData}";

                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {                    
                    string responseData = await response.Content.ReadAsStringAsync();
                    return Json(new { data = responseData });
                   // return Content($"{{\"data\": {responseData}}}", "application/json");
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
            decimal Amount = model.PaymentModeName == "CASH" ? model.Amount : model.TotalAmount.GetValueOrDefault();
            DateTime dob = model.DateOfBirth ?? DateTime.MinValue;
            DateTime provdate = model.ProvDate ?? DateTime.MinValue;
            string FinYear = User.FindFirst("FinYear")?.Value ?? string.Empty;
            string UserID = User.FindFirst("UserID")?.Value ?? string.Empty;
            string DataFlag = User.FindFirst("DataFlag")?.Value ?? string.Empty;


            string Doc1 = string.Empty;
            string Doc2 = string.Empty;
            string Doc3 = string.Empty;
            List<BankDetails> bankDetailslist = string.IsNullOrEmpty(model.BankDetailsList) ? new List<BankDetails>() : JsonConvert.DeserializeObject<List<BankDetails>>(model.BankDetailsList);

            if (bankDetailslist != null)
            {
                foreach (var bankDetail in bankDetailslist)
                {
                    string docFile = bankDetail.TempDoc;
                    var fileName = Path.GetFileName(docFile);

                    var tempFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "TempDocDonationReceive");
                    var mainFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "DocDonationReceive");

                    var tempFilePath = Path.Combine(tempFolderPath, fileName);
                    var mainFilePath = Path.Combine(mainFolderPath, fileName);

                    if (System.IO.File.Exists(tempFilePath))
                    {
                        // Move the file from the temporary folder to the main folder
                        System.IO.File.Move(tempFilePath, mainFilePath);
                    }
                }
            }

            if (model.DocProvisonal != null && model.DocProvisonal.Count > 0)
            {

                var fileNamesBuilder = new StringBuilder();

                foreach (var file in model.DocProvisonal)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "DocDonationReceive");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var filePath = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {

                            if (fileNamesBuilder.Length > 0)
                            {
                                fileNamesBuilder.Append(",");
                            }

                            fileNamesBuilder.Append(fileName);

                            file.CopyTo(stream);
                        }
                    }
                }

                Doc1 = fileNamesBuilder.ToString();
            }

            var requestData = new
            {
                IsReceiveHeadDiffrent = isDifferent,
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
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                PrefixToFullName = model.PrefixToFullName,
                RelationToFullName = model.RelationToFullName,
                DateOfBirth = dob,
                Company = model.Company,
                FullAddress = model.FullAddress,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
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
                IsPermanentAddressDiff = model.IsPermanentAddressDiff,
                ifdetailsNotComplete = model.IfDetailsNotComplete,
                P_FullAddress = model.P_FullAddress,
                P_Address1 = model.P_Address1,
                P_Address2 = model.P_Address2,
                P_Address3 = model.P_Address3,
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
                OrderTypeID = model.OrderTypeID,
                OrderTypeName = model.OrderTypeName,
                OrderNumber = model.OrderNumber,
                CurrencyID = model.CurrencyID,
                CurrencyCode = model.CurrencyCode,
                Amount = Amount,
                MaterialDepositID = model.MaterialDepositID,
                Material = model.Material,
                IsManavaFormulaRequire = model.IsManavaFormulaRequire,
                IsPatientsPhotoRequire = model.IsPatientsPhotoRequire,
                IfDiffrentAddressForDispatch = model.IfDiffrentAddressForDispatch,
                DifferentAddressToDispatch = model.DifferentAddressToDispatch,
                IfAnnounceDueInFuture = model.IfAnnounceDueInFuture,
                Doc1 = Doc1,
                Doc2 = Doc2,
                Doc3 = Doc3,
                DataFlag = DataFlag
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
                    TempData["msg"] = modeldata.msg;
                    return RedirectToAction("Index", "DonationReceiveMaterDetails", new { model = modeldata });
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

        [HttpPost]
        public async Task<IActionResult> Edit(DonationReceiveMaster model)
        {
            bool? isDifferent = model.IsReceiveHeadDiffrent;
            string ReceiveDepartment = isDifferent == true ? model.ReceiveHeadName : User.FindFirst("Department")?.Value;
            decimal Amount = model.PaymentModeName == "CASH" ? model.Amount : model.TotalAmount.GetValueOrDefault();
            DateTime dob = model.DateOfBirth ?? DateTime.MinValue;
            DateTime provdate = model.ProvDate ?? DateTime.MinValue;
            string FinYear = User.FindFirst("FinYear")?.Value ?? string.Empty;
            string UserID = User.FindFirst("UserID")?.Value ?? string.Empty;

            string Doc1 = string.Empty;
            string Doc2 = string.Empty;
            string Doc3 = string.Empty;
            List<BankDetails> bankDetailslist = string.IsNullOrEmpty(model.BankDetailsList) ? new List<BankDetails>() : JsonConvert.DeserializeObject<List<BankDetails>>(model.BankDetailsList);

            if (bankDetailslist != null)
            {
                foreach (var bankDetail in bankDetailslist)
                {
                    string docFile = bankDetail.TempDoc;
                    var fileName = Path.GetFileName(docFile);

                    var tempFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "TempDocDonationReceive");
                    var mainFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "DocDonationReceive");

                    var tempFilePath = Path.Combine(tempFolderPath, fileName);
                    var mainFilePath = Path.Combine(mainFolderPath, fileName);

                    // Check if the file exists in the temporary folder before attempting to move it
                    if (System.IO.File.Exists(tempFilePath))
                    {
                        // Move the file from the temporary folder to the main folder
                        System.IO.File.Move(tempFilePath, mainFilePath);
                    }
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
                ReceiveID = model.ReceiveID,
                IsReceiveHeadDiffrent = isDifferent,
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
                DifferentAddressToDispatch = model.DifferentAddressToDispatch,
                IfAnnounceDueInFuture = model.IfAnnounceDueInFuture,
                Doc1 = Doc1,
                Doc2 = Doc2,
                Doc3 = Doc3

            };


            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(requestData);
                string apiUrl = "api/DonationReceiveMaster/UpdateData";
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
                    TempData["msg"] = modeldata.msg;
                    return RedirectToAction("Index", "DonationReceiveMaterDetails", new { model = modeldata });
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

            var json = response.Content.ReadAsStringAsync().Result;
            var detail = JsonConvert.DeserializeObject<List<ProvisionalReceiptModel>>(json);

            if (detail == null)
            {
                return NotFound();
            }

            // Return the view as PDF directly
            return new ViewAsPdf("PrintProvisionalReceipt", detail)
            {
                FileName = "ProvisionalReceipt" + refid + ".pdf"
            };
        }

        public async Task<IActionResult> DeleteReceiveID(int refid)
        {
            var response = await _apiClient.GetAsync($"api/DonationReceiveMaster/DeleteReceiveID?refid={refid}");

            if (response.IsSuccessStatusCode)
            {
                var modelget = await response.Content.ReadAsStringAsync();
                var modeldata = modelget != null ? JsonConvert.DeserializeObject<DonationReceiveMaster>(modelget) : null;
                TempData["msg"] = modeldata.msg;
                return RedirectToAction("Index", "DonationReceiveMaterDetails", new { model = modeldata });
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

        [HttpPost]
        public async Task<JsonResult> TemporaryUploadFile(DonationReceiveMaster model)
        {
            try
            {
                if (model.DocCheque != null && model.DocCheque.Length > 0)
                {
                    var temporaryFolder = Path.Combine(_webHostEnvironment.WebRootPath, "TempDocDonationReceive");

                    if (!Directory.Exists(temporaryFolder))
                    {
                        Directory.CreateDirectory(temporaryFolder);
                    }

                    // Generate a unique temporary path with file extension
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.DocCheque.FileName);
                    var temporaryPath = Path.Combine(temporaryFolder, uniqueFileName);

                    // Save the file to the temporary location asynchronously
                    using (var stream = new FileStream(temporaryPath, FileMode.Create))
                    {
                        await model.DocCheque.CopyToAsync(stream);
                    }

                    // Return the temporary path
                    return Json(new { uniqueFileName });
                }

                return Json(new { error = "No file received." });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

    }
}
