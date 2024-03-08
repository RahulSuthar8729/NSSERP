using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.Masters.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NSSERP.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class StateMasterController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public StateMasterController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiClient.GetAsync($"api/StateMaster/Index?DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    Response.WriteAsJsonAsync(response);

                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var detail = json != null ? JsonConvert.DeserializeObject<StateMaster>(json) : null;
            if (TempData.ContainsKey("msg"))
            {
                string messageFromFirstController = TempData["msg"] as string;
                detail.msg = messageFromFirstController;
                TempData.Remove("msg");
            }
            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);
        }

        public async Task<IActionResult> Create()
        {
            var response = await _apiClient.GetAsync($"api/CountryMaster/Index");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    Response.WriteAsJsonAsync(response);

                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var detail = json != null ? JsonConvert.DeserializeObject<StateMaster>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);


        }



        [HttpPost]
        public async Task<IActionResult> Create(StateMaster model)
        {
            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(model);
                string apiUrl = string.Empty;
                if (model.State_Code != null)
                    apiUrl = "api/StateMaster/UpdateData";
                else
                    apiUrl = "api/StateMaster/InsertData";


                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "StateMaster", new { model = modelget });
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
                ViewBag.emsg = $"An error occurred: {ex.Message}";
            }

            return View();
        }


        public async Task<IActionResult> Edit(string CountryId)
        {
            var response = await _apiClient.GetAsync($"api/StateMaster/GetStateById?id={CountryId}&DataFlag={User.FindFirst("DataFlag")?.Value}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    Response.WriteAsJsonAsync(response);

                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var detail = json != null ? JsonConvert.DeserializeObject<StateMaster>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(StateMaster model)
        {
            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(model);
                string apiUrl = string.Empty;
                if (model.Country_Code != null)
                    apiUrl = "api/StateMaster/UpdateData";
                else
                    apiUrl = "api/StateMaster/InsertData";


                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "StateMaster", new { model = modelget });
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
                ViewBag.emsg = $"An error occurred: {ex.Message}";
            }


            return View(model);
        }

    }
}
