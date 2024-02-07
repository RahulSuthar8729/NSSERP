using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.NationalGangotri.Models;
using NSSERP.DbFunctions;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NSSERP.Areas.NationalGangotri.Controllers
{
    [Authorize]
    [Area("NationalGangotri")]
    public class SankalpSiddhiController : Controller
    {
        private readonly string _connectionString;
        private readonly DbClass _dbFunctions;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _apiClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DonationReceiveMaterDetailsController> _logger;

        public SankalpSiddhiController(DbClass dbFunctions, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory, ILogger<DonationReceiveMaterDetailsController> logger)
        {
            _dbFunctions = dbFunctions;
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
            _apiClient = httpClientFactory.CreateClient("WebApi");
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index(SankalpSiddhiDetails model)
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
                            model.IfDetailsNotComplete,
                        };

                        //var parameterList = new List<object> { parameters };

                        string requestBody = System.Text.Json.JsonSerializer.Serialize(parameters);
                        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                        var response1 = await _apiClient.PostAsync("api/DonationReceiveDetails/SearchDonationReceiveDataBYPara", content);
                        response1.EnsureSuccessStatusCode();

                        var jsonResponse = await response1.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<SankalpSiddhiDetails>(jsonResponse);
                        if (TempData.ContainsKey("msg"))
                        {
                            string messageFromFirstController = TempData["msg"] as string;
                            result.msg = messageFromFirstController;
                            TempData.Remove("msg");
                        }
                        return View(result);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred in the Index action.");

                        return BadRequest($"An error occurred during the search operation: {ex.Message}");
                    }
                }

                var baseAddress = _apiClient.BaseAddress;

                // Make a GET request to the GetDonationReceiveDetails endpoint
                var response = await _apiClient.GetAsync($"api/SankalpSiddhi/GetDataOnPageLoad?PageNumber={model.PageNumber}&PageSize={model.PageSize}");
                response.EnsureSuccessStatusCode();

                var jsonres= await response.Content.ReadAsStringAsync();
                var resultmodel = JsonConvert.DeserializeObject<SankalpSiddhiDetails>(jsonres);

                if (TempData.ContainsKey("msg"))
                {
                    string messageFromFirstController = TempData["msg"] as string;
                    resultmodel.msg = messageFromFirstController;
                    TempData.Remove("msg");
                }
                return View(resultmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                // Log or handle the exception
                ModelState.AddModelError("", $"An error occurred during the request: {ex.Message}");
            }

            return View(new SankalpSiddhiDetails());
        }
    

        [HttpGet]
        public async Task<IActionResult> GetDepositDetails(int receiveId)
        {
            try
            {
                string apiUrl = $"api/SankalpSiddhi/GetDepositDetails?refno={receiveId}";

                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jsonString = json.ToString();                 
                    return Content($"{{\"data\": {jsonString}}}", "application/json");
                
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
        public async Task<IActionResult> GetBankStatement()
        {
            try
            {
                string apiUrl = $"api/SankalpSiddhi/GetBankStatement";

                var response = await _apiClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jsonString = json.ToString();
                    return Content($"{{\"data\": {jsonString}}}", "application/json");

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
        public async Task<IActionResult> UpdateBankIDandTrasactionID([FromBody] SankalpSiddhiDetails model)
        {
            try
            {
                var requestData = new
                {                    
                    UserID = User.FindFirst("UserID")?.Value,
                    UserName = User.FindFirst(ClaimTypes.Name)?.Value,
                };
                string requestBody = System.Text.Json.JsonSerializer.Serialize(requestData);
                string apiUrl = "api/SankalpSiddhi/InsertData";
                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                requestContent.Headers.Add("DepositeDetailsListJson", model.MapWithBankIDList);
                requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jsonString = json.ToString();
                    return Json(new { success = true, message = jsonString, data = jsonString });

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



    }
}
