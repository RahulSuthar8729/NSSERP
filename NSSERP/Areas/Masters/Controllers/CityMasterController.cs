﻿using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSSERP.Areas.Masters.Models;
using NSSERP.Models;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Security.Claims;
using NSSERP.DbFunctions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace NSSERP.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class CityMasterController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public CityMasterController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _apiClient.GetAsync($"api/CityMaster/Index?DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}");

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
            var detail = json != null ? JsonConvert.DeserializeObject<CityMaster>(json) : null;
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

        [HttpGet]
        public IActionResult Create()
        {
            CityMaster model = new CityMaster();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityMaster model)
        {
            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(model);
                string apiUrl = string.Empty;
                if (model.City_Code != null)
                    apiUrl = "api/CityMaster/UpdateData";
                else
                    apiUrl = "api/CityMaster/InsertData";


                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "CityMaster", new { model = modelget });
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


        public async Task<IActionResult> Edit(string id)
        {
            var response = await _apiClient.GetAsync($"api/CityMaster/GetCitybyID?id={id}&DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}");

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
            var detail = json != null ? JsonConvert.DeserializeObject<CityMaster>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CityMaster model)
        {
            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(model);
                string apiUrl = string.Empty;
                if (model.City_Code != null)
                    apiUrl = "api/CityMaster/UpdateData";
                else
                    apiUrl = "api/CityMaster/InsertData";


                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "CityMaster", new { model = modelget });
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
