using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Index(DonationReceiveMasterDetails model)
    {
        try
        {
            if (HttpContext.Request.Method == "POST")
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

                var responsepost = await _httpClientFactory.CreateClient("WebApi").PostAsJsonAsync("api/SearchDonationReceiveDataBYPara/SearchDonationReceiveDataBYPara", parameters);
                responsepost.EnsureSuccessStatusCode();

                var jsonpost = await responsepost.Content.ReadAsStringAsync();

                var modelWithSearchResults = new DonationReceiveMasterDetails()
                {
                    masterDetails = JsonConvert.DeserializeObject<List<DonationReceiveMasterDetails>>(jsonpost),
                    CityMasterList = _dbFunctions.GetActiveCities(),
                    paymentModes = _dbFunctions.GetPaymentModes(),
                    statelist = _dbFunctions.GetStates()
                };

                return View(modelWithSearchResults);
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

    [HttpPost]
    public async Task<IActionResult> SearchDataWithPara(DonationReceiveMasterDetails model)
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

            var parameterList = new List<object> { parameters };

            var response = await _apiClient.PostAsJsonAsync("api/DonationReceiveDetails/SearchDonationReceiveDataBYPara", parameterList);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var modelWithSearchResults = new DonationReceiveMasterDetails()
            {
                masterDetails = JsonConvert.DeserializeObject<List<DonationReceiveMasterDetails>>(jsonResponse),
                CityMasterList = _dbFunctions.GetActiveCities(),
                paymentModes = _dbFunctions.GetPaymentModes(),
                statelist = _dbFunctions.GetStates()
            };

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Log the exception
           
            return BadRequest($"An error occurred during the search operation: {ex.Message}");
        }
    }



}
