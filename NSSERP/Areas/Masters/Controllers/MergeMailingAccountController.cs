﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSSERP.Areas.Masters.Models;
using System.Net;
using System.Text;

namespace NSSERP.Areas.Masters.Controllers
{
    [Area("Masters")]
    [Authorize]
    public class MergeMailingAccountController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public MergeMailingAccountController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)
        {
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Home(int id)
        {
            MergeMailingAccount model = new MergeMailingAccount();
            if (TempData.ContainsKey("msg"))
            {
                string messageFromFirstController = TempData["msg"] as string;
                model.msg = messageFromFirstController;
                TempData.Remove("msg");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Home(MergeMailingAccount model)
        {

            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(model);
                string apiUrl = string.Empty;
                if (model.OrgNgcode != null)
                    apiUrl = "api/MergeMailingAccount/InsertData";
                else
                    apiUrl = "api/MergeMailingAccount/InsertData";


                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Home", "MergeMailingAccount", new { model = modelget });
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
    }
}
