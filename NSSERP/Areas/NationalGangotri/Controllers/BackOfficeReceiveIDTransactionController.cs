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
using static NSSERP.Areas.NationalGangotri.Models.BackOfficeModel;
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

namespace NSSERP.Areas.NationalGangotri.Controllers
{
    [Authorize]
    [Area("NationalGangotri")]
    public class BackOfficeReceiveIDTransactionController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public BackOfficeReceiveIDTransactionController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {

            var response = await _apiClient.GetAsync($"api/BackOfficeWork/ViewDetails?id={id}");
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
            var detail = json != null ? JsonConvert.DeserializeObject<BackOfficeModel>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BackOfficeModel model)
        {

            string Doc3 = string.Empty;

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
            string UserID = User.FindFirst("UserID")?.Value ?? string.Empty;

            var requestData = new
            {
                ReceiveID = model.ReceiveID,
                UserID = UserID,
                UserName = User.FindFirst(ClaimTypes.Name)?.Value,
                Doc3 = Doc3

            };
            string requestBody = System.Text.Json.JsonSerializer.Serialize(requestData);
            string apiUrl = "api/BackOfficeWork/InsertData";
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            requestContent.Headers.Add("DepositeList", model.DonationDetails);
            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _apiClient.PostAsync(apiUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var modelget = await response.Content.ReadAsStringAsync();
                var modeldata = modelget != null ? JsonConvert.DeserializeObject<BackOfficeModel>(modelget) : null;
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
    }
}
