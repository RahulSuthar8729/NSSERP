﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.NationalGangotri.Models;
using NSSERP.DbFunctions;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading;
using System.Text.Json;
using System.Text;



[Authorize]
[Area("NationalGangotri")]
public class DonationReceiveMaterDetailsController : Controller
{
    private readonly string _connectionString;
    private readonly DbClass _dbFunctions;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly HttpClient _apiClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public DonationReceiveMaterDetailsController(DbClass dbFunctions, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
    {
        _dbFunctions = dbFunctions;
        _connectionString = configuration.GetConnectionString("ConStr");
        _webHostEnvironment = webHostEnvironment;
        _apiClient = httpClientFactory.CreateClient("WebApi");
        _httpClientFactory = httpClientFactory;
    }
    
    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Index(DonationReceiveMasterDetails model)
    {
        try
        {
            if (HttpContext.Request.Method == "POST")
            {
                try
                {
                    var parameters = new
                    {
                        model.ReceiveID,
                        model.PaymentModeName,
                        model.TotalAmount,
                        model.CityName,
                        model.CityID,
                        model.StateName,
                        model.StateID,
                        model.MaterialID,
                        model.ProvNo,
                        model.ReceiveDate,
                        model.FullName,
                        model.PaymentModeID,
                        model.IfDetailsNotComplete
                    };

                    //var parameterList = new List<object> { parameters };

                    string requestBody = System.Text.Json.JsonSerializer.Serialize(parameters);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var response1 = await _apiClient.PostAsync("api/DonationReceiveDetails/SearchDonationReceiveDataBYPara", content);
                    response1.EnsureSuccessStatusCode();

                    var jsonResponse = await response1.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<DonationReceiveMasterDetails>(jsonResponse);

                    return View(result);
                }
                catch (Exception ex)
                {
                    // Log the exception

                    return BadRequest($"An error occurred during the search operation: {ex.Message}");
                }
            }
            var baseAddress = _apiClient.BaseAddress;

            // Make a GET request to the GetDonationReceiveDetails endpoint
            var response = await _apiClient.GetAsync("api/DonationReceiveDetails/GetDonationReceiveDetails");
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON to a list of DonationReceiveMasterDetails
            var modelitems = new DonationReceiveMasterDetails()
            {
                masterDetails = JsonConvert.DeserializeObject<List<DonationReceiveMasterDetails>>(json),
                CityMasterList = _dbFunctions.GetActiveCities(),
                paymentModes = _dbFunctions.GetPaymentModes(),
                statelist = _dbFunctions.GetStates()
            };

            return View(modelitems);
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            ModelState.AddModelError("", $"An error occurred during the request: {ex.Message}");
        }

        return View(new DonationReceiveMasterDetails());
    }   

}
