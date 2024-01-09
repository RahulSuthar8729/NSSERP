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
        public async Task<IActionResult> Home()
        {
            int id = Convert.ToInt32(TempData["DonationDetails"]);

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
        public async Task<IActionResult> GetSubHeadByHead(string HeadID,string DataFlag)
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
        public async Task<IActionResult> GetQtyAmtBySubHead(string YojnaID,string DataFlag)
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
        public IActionResult Home(DonationReceiveMaster model)
        {
            bool? isDifferent = model.IsReceiveHeadDiffrent;
            string ReceiveDepartment = string.Empty;
            if (isDifferent == true)
            {
                ReceiveDepartment = model.ReceiveHeadName;
            }
            if (isDifferent == false)
            {
                ReceiveDepartment = User.FindFirst("Department")?.Value;
            }
            decimal Amount = 0;
            if (model.PaymentModeName == "CASH")
            {
                Amount = model.Amount.GetValueOrDefault();
            }
            else
            {
                Amount = model.TotalAmount.GetValueOrDefault();
            }



            DateTime dob = Convert.ToDateTime(model.DateOfBirth);
            DateTime provdate = model.ProvDate.GetValueOrDefault();
            string FinYear = User.FindFirst("FinYear")?.Value;
            string UserID = User.FindFirst("UserID")?.Value;
            int maxReceiveID = 0;
            List<MobileDetails> MobileList = string.IsNullOrEmpty(model.MobileList) ? new List<MobileDetails>() : JsonConvert.DeserializeObject<List<MobileDetails>>(model.MobileList);

            List<IdentityDetails> IdentityList = string.IsNullOrEmpty(model.IdentityList) ? new List<IdentityDetails>() : JsonConvert.DeserializeObject<List<IdentityDetails>>(model.IdentityList);

            List<BankDetails> bankDetailslist = string.IsNullOrEmpty(model.BankDetailsList) ? new List<BankDetails>() : JsonConvert.DeserializeObject<List<BankDetails>>(model.BankDetailsList);

            List<ReceiptDetail> Receiptdetailslist = string.IsNullOrEmpty(model.receiptdetailslist) ? new List<ReceiptDetail>() : JsonConvert.DeserializeObject<List<ReceiptDetail>>(model.receiptdetailslist);

            List<AnnounceDetails> announcelist = string.IsNullOrEmpty(model.AnnounceDetsilsList) ? new List<AnnounceDetails>() : JsonConvert.DeserializeObject<List<AnnounceDetails>>(model.AnnounceDetsilsList);

            List<DonorInstructionList> donorInstructionLists = string.IsNullOrEmpty(model.donorInstructionjsonList) ? new List<DonorInstructionList>() : JsonConvert.DeserializeObject<List<DonorInstructionList>>(model.donorInstructionjsonList);


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
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Start a transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add("@AppStatus", "");
                            parameters.Add("@FinYear", FinYear);
                            parameters.Add("@ReceiveDate", model.ReceiveDate);
                            parameters.Add("@IsReceiveHeadDifferent", model.IsReceiveHeadDiffrent);
                            parameters.Add("@ReceiveHeadID", model.ReceiveHeadID);
                            parameters.Add("@ReceiveHeadName", ReceiveDepartment);
                            parameters.Add("@UserID", UserID);
                            parameters.Add("@UserName", User.FindFirst(ClaimTypes.Name)?.Value);
                            parameters.Add("@ReceiveInEventID", model.ID);
                            parameters.Add("@ReceiveInEvent", model.EventName);
                            parameters.Add("@InMemory", model.InMemory);
                            parameters.Add("@DonorID", model.DonorID);
                            parameters.Add("@NamePrefix", model.NamePrefix);
                            parameters.Add("@FullName", model.FullName);
                            parameters.Add("@PrefixToFullName", model.PrefixToFullName);
                            parameters.Add("@RelationToFullName", model.RelationToFullName);
                            parameters.Add("@DateOfBirth", dob);
                            parameters.Add("@Company", model.Company);
                            parameters.Add("@FullAddress", model.FullAddress);
                            parameters.Add("@PinCode", model.PinCode);
                            parameters.Add("@CountryID", model.CountryId);
                            parameters.Add("@CountryName", model.CountryName);
                            parameters.Add("@StateID", model.StateID);
                            parameters.Add("@StateName", model.StateName);
                            parameters.Add("@DistrictID", model.DistrictID);
                            parameters.Add("@DistrictName", model.DistrictName);
                            parameters.Add("@CityID", model.CityID);
                            parameters.Add("@CityName", model.CityName);
                            parameters.Add("@IfUpdationInAddress", model.IfUpdationInAddress);
                            parameters.Add("@IsPermanentAddressDiff", model.IsPermanentAddressDiff);
                            parameters.Add("@IfDetailsNotComplete", model.IfDetailsNotComplete);
                            parameters.Add("@P_FullAddress", model.P_FullAddress);
                            parameters.Add("@P_PinCode", model.P_PinCode);
                            parameters.Add("@P_CountryID", model.P_CountryID);
                            parameters.Add("@P_CountryName", model.P_CountryName);
                            parameters.Add("@P_StateID", model.P_StateID);
                            parameters.Add("@P_StateName", model.P_StateName);
                            parameters.Add("@P_DistrictID", model.P_DistrictID);
                            parameters.Add("@P_DistrictName", model.P_DistrictName);
                            parameters.Add("@P_CityID", model.P_CityID);
                            parameters.Add("@P_CityName", model.P_CityName);
                            parameters.Add("@EmailID", model.EmailID);
                            parameters.Add("@StdCode", model.StdCode);
                            parameters.Add("@PhoneR", model.PhoneR);
                            parameters.Add("@ProvNo", model.ProvNo);
                            parameters.Add("@ProvDate", provdate);
                            parameters.Add("@DonPersonName", model.PersonName);
                            parameters.Add("@DonEventID", model.ID);
                            parameters.Add("@DonEventName", model.EventName);
                            parameters.Add("@PaymentModeID", model.PaymentModeID);
                            parameters.Add("@PaymentModeName", model.PaymentModeName);
                            parameters.Add("@CurrencyID", model.CurrencyID);
                            parameters.Add("@CurrencyCode", model.CurrencyCode);
                            parameters.Add("@Amount", Amount);
                            parameters.Add("@MaterialDepositID", model.MaterialDepositID);
                            parameters.Add("@Material", model.Material);
                            parameters.Add("@IsManavaFormulaRequire", model.IsManavaFormulaRequire);
                            parameters.Add("@IsPatientsPhotoRequire", model.IsPatientsPhotoRequire);
                            parameters.Add("@IfDiffrentAddressForDispatch", model.IfDiffrentAddressForDispatch);
                            parameters.Add("@DifferentAddressToDispatch", model.DifferentAddressToDispatch);
                            parameters.Add("@Instructions", model.Instructions);
                            parameters.Add("@ReceiptRemarks", model.ReceiptRemarks);
                            parameters.Add("@IfAnnounceDueInFuture", model.IfAnnounceDueInFuture);
                            parameters.Add("@DocProvisonal", Doc1);
                            parameters.Add("@DocCheque", Doc2);
                            parameters.Add("@DocPayInSlip", Doc3);
                            parameters.Add("@CreatedBy", User.FindFirst(ClaimTypes.Name)?.Value);
                            parameters.Add("@TotalAmount", Amount);

                            connection.Execute("InsertDonationReceiveMaster", parameters, transaction, commandType: CommandType.StoredProcedure);

                            maxReceiveID = connection.QueryFirstOrDefault<int>("SelectMaxReceiveID", new { UserID, FinYear }, transaction, commandType: CommandType.StoredProcedure);


                            if (MobileList != null)
                            {
                                foreach (var mobileNumber in MobileList)
                                {
                                    var mobileParams = new DynamicParameters();
                                    mobileParams.Add("@REF_NO", maxReceiveID);
                                    mobileParams.Add("@CountryCode", mobileNumber.CountryCode);
                                    mobileParams.Add("@MobileNo", mobileNumber.MobileNumber);
                                    mobileParams.Add("@CreatedBy", UserID);
                                    connection.Execute("InsertMultiMobileInDonationReceiveMaster", mobileParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            if (IdentityList != null)
                            {
                                foreach (var identity in IdentityList)
                                {
                                    var identityParams = new DynamicParameters();
                                    identityParams.Add("@REF_NO", maxReceiveID);
                                    identityParams.Add("@IdentityType", identity.IdentityType);
                                    identityParams.Add("@IdentityNumber", identity.IdentityNumber);
                                    identityParams.Add("@CreatedBy", UserID);

                                    connection.Execute("InsertMultiIdentityInDonationReceiveMaster", identityParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            if (bankDetailslist != null)
                            {
                                foreach (var bankDetail in bankDetailslist)
                                {
                                    var bankParams = new DynamicParameters();
                                    bankParams.Add("@REF_NO", maxReceiveID);
                                    bankParams.Add("@BankID", bankDetail.BankID);
                                    bankParams.Add("@BankName", bankDetail.BankName);
                                    bankParams.Add("@ChequeOrDraftDate", bankDetail.ChequeDate);
                                    bankParams.Add("@ChequeOrDraftNo", bankDetail.ChequeNo);
                                    bankParams.Add("@DepositeBankID", bankDetail.DepositBankID);
                                    bankParams.Add("@DepositeBankName", bankDetail.DepositBank);
                                    bankParams.Add("@DepositeDate", bankDetail.DepositDate);
                                    bankParams.Add("@IsPdcCheque1", bankDetail.PdcCheque);
                                    bankParams.Add("@CreatedBy", UserID);
                                    bankParams.Add("@DonationMode", bankDetail.DonationMode);
                                    bankParams.Add("@CurrencyCode", bankDetail.Currency);
                                    bankParams.Add("@Amount", bankDetail.Amount);

                                    connection.Execute("InsertBankDetailsInDonationReceiveMultiBank", bankParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }

                            if (Receiptdetailslist != null)
                            {
                                foreach (var receiptDetail in Receiptdetailslist)
                                {
                                    var rparameters = new DynamicParameters();
                                    rparameters.Add("@REF_NO", maxReceiveID);
                                    rparameters.Add("@HeadID", receiptDetail.HeadID);
                                    rparameters.Add("@Campaign", receiptDetail.Campaign);
                                    rparameters.Add("@HeadName", receiptDetail.Head);
                                    rparameters.Add("@SubHeadID", receiptDetail.SubHeadID);
                                    rparameters.Add("@SubHeadName", receiptDetail.Purpose);
                                    rparameters.Add("@Purpose", receiptDetail.Purpose);
                                    rparameters.Add("@Quantity", receiptDetail.Quantity);
                                    rparameters.Add("@Amount", receiptDetail.Amount);
                                    rparameters.Add("@CreatedOn", DateTime.Now);
                                    rparameters.Add("@CreatedBy", UserID);


                                    connection.Execute("InsertDonationReceiveMultiHead", rparameters, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            if (donorInstructionLists != null)
                            {
                                foreach (var instruction in donorInstructionLists)
                                {
                                    var instructionParams = new DynamicParameters();
                                    instructionParams.Add("@REF_NO", maxReceiveID);
                                    instructionParams.Add("@InstructionID", instruction.InstructionId);
                                    instructionParams.Add("@InstructionName", instruction.InstructionName);
                                    instructionParams.Add("@Remarks", instruction.Remarks);
                                    instructionParams.Add("@CreatedBy", UserID);

                                    connection.Execute("InsertDonationReceiveMultiDonorInstruc", instructionParams, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }


                            if (announcelist != null)
                            {
                                foreach (var announce in announcelist)
                                {
                                    var Announcepara = new DynamicParameters();
                                    Announcepara.Add("@REF_NO", maxReceiveID);
                                    Announcepara.Add("@TotalPurposeAmount", announce.TotalPurposeAmount);
                                    Announcepara.Add("@ReceiveAmount", announce.ReceiveAmount);
                                    Announcepara.Add("@DueAmount", announce.DueAmount);
                                    Announcepara.Add("@AnnunceID", announce.AnnounceId);
                                    Announcepara.Add("@Amount", announce.Amount);
                                    Announcepara.Add("@Date", announce.Date);
                                    Announcepara.Add("@CreatedBy", UserID);

                                    connection.Execute("InsertDonationReceiveAnnunceDue", Announcepara, transaction, commandType: CommandType.StoredProcedure);
                                }
                            }
                            transaction.Commit();

                            ViewBag.msg = "Receive ID:" + maxReceiveID + " is Generated Successfully";
                        }

                        catch (Exception)
                        {
                            // If an exception occurs, roll back the transaction
                            transaction.Rollback();
                            ViewBag.emsg = "An error occurred during the transaction.";

                            return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions, such as database connection issues
                ViewBag.emsg = $"An error occurred: {ex.Message}";
            }

            var AddressModel = new DonationReceiveMaster
            {
                CountryList = _dbFunctions.GetCountries(),
                paymentModeList = _dbFunctions.GetPaymentModes(),
                currenciesList = _dbFunctions.GetCurrencyListWithCountry(),
                bankmasterlist = _dbFunctions.GetAllBankMasters(),
                SubHeadList = _dbFunctions.getSubHeads(),
                ReceiveHeadList = _dbFunctions.getReceiveHeads(),
                ReceiveInEventList = _dbFunctions.GetEvents(),
                campaignlist = _dbFunctions.GetallCampaigns(),
                donorInstructionList = _dbFunctions.GetDonorINstructionsMaster()

            };

            return View(AddressModel);
        }

        public async Task<IActionResult> ViewDetails(int id)
        {

            TempData["DonationDetails"] = null;
            TempData["DonationDetails"] = id;

            return RedirectToAction("Home");
        }


    }
}
