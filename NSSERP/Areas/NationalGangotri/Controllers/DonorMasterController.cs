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
using System.Security.Cryptography.Xml;

namespace NSSERP.Areas.NationalGangotri.Controllers
{
    [Authorize]
    [Area("NationalGangotri")]
    public class DonorMasterController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public DonorMasterController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Home(int id)
        {
            var response = await _apiClient.GetAsync($"api/DonorMaster/Home");

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
            var detail = json != null ? JsonConvert.DeserializeObject<DonorMaster>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);

        }


        [HttpPost]
        public async Task<IActionResult> Home(DonorMaster model)
        {
      
            string FinYear = User.FindFirst("FinYear")?.Value ?? string.Empty;
            string UserID = User.FindFirst("UserID")?.Value ?? string.Empty;
            string DataFlag = User.FindFirst("DataFlag")?.Value ?? string.Empty;


            string Doc1 = string.Empty;
            string Doc2 = string.Empty;
            string Doc3 = string.Empty;          
        
            var requestData = new
            {
               

                FinYear = FinYear,
                UserID = UserID,               
                UserName = User.FindFirst(ClaimTypes.Name)?.Value,
                DataFlag = DataFlag,
                IsAnonymous = model.IsAnonymous,
                GroupNGCode = model.GroupNGCode,
                onlineCustId = model.onlineCustId,
                upiId = model.upiId,
                mailingNo=model.mailingNo,
                referenceNo=model.referenceNo,
                dateOfEntry=model.dateOfEntry,
                DonorCat=model.DonorCat,
                IsAppShreekaPurnVivranReceived=model.IsAppShreekaPurnVivranReceived,
                NamePrefix=model.NamePrefix,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                RelationToFullName = model.PrefixToRelation +" " + model.RelationToFullName,
                DateOfAnniversary=model.DateOfAnniversary,
                Company = model.Company,
                YourCompany=model.YourCompany,
                BussinessOrJobType=model.BussinessOrJobType,
                Profession=model.Profession,
                WorkingIn=model.WorkingIn,
                Designation=model.Designation,
                CareOf=model.CareOf,
                ToatlDonation=model.ToatlDonation,

                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
                PinCode = model.PinCode,
                CountryID = model.CountryId,
                CountryName = model.CountryName,
                StateID = model.StateID,
                StateName = model.StateName,
                DistrictID = model.DistrictID,
                DistrictName = model.DistrictName,
                CityID = model.CityID,
                CityName = model.CityName,              
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

                IsCallActive=model.IsCallActive,
                IsMsgActive=model.IsMsgActive,
                IsWhatsAppActive=model.IsWhatsAppActive,
                IsEmailActive=model.IsEmailActive,
                IsLetterCommunicationActive=model.IsLetterCommunicationActive,
                IsSendoperationPhotoActive=model.IsSendoperationPhotoActive,
                Website=model.Website,

                MobileList =model.MobileList,
                IdentityList=model.IdentityList,
                
                Sandipan =model.Sandipan,
                language = model.language,
                SandipanRemarksReason=model.SandipanRemarksReason,
                SandipanRemarks=model.SandipanRemarks,
                ReceiptCopyRequireOptions=model.ReceiptCopyRequireOptions,
                IsVisit =model.IsVisit,
                VisitYear=model.VisitYear,
                ForginNgCodeRefrence=model.ForginNgCodeRefrence,
                ChangesRemarks=model.ChangesRemarks,
                Remarks=model.Remarks,


                Doc1 = Doc1,
                Doc2 = Doc2,
                Doc3 = Doc3
               
            };


            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(requestData);
                string apiUrl = string.Empty;
                if (model.DonorID != null)
                    apiUrl = "api/DonorMaster/InsertData";
                else
                    apiUrl = "api/DonorMaster/InsertData";


                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

                requestContent.Headers.Add("MobileList", model.MobileList);
                requestContent.Headers.Add("IdentityList", model.IdentityList);              
                requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    var modeldata = modelget != null ? JsonConvert.DeserializeObject<DonorMaster>(modelget) : null;
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

    }
}
