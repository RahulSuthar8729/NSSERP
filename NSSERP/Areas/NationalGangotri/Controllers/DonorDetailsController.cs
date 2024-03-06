using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.NationalGangotri.Models;
using NSSERP.DbFunctions;
using System.Net;
using System.Text;

namespace NSSERP.Areas.NationalGangotri.Controllers
{
    [Authorize]
    [Area("NationalGangotri")]
    public class DonorDetailsController : Controller
    {
        private readonly string _connectionString;
        private readonly DbClass _dbFunctions;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _apiClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DonationReceiveMaterDetailsController> _logger;

        public DonorDetailsController(DbClass dbFunctions, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory, ILogger<DonationReceiveMaterDetailsController> logger)
        {
            _dbFunctions = dbFunctions;
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
            _apiClient = httpClientFactory.CreateClient("WebApi");
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]    
        public async Task<IActionResult> Index(DonorMaster model)
        {
            try
            {
                var response = await _apiClient.GetAsync($"api/DonorDetails/GetDonorDetailsList");

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
                var detail = json != null ? JsonConvert.DeserializeObject<DonorMaster>(json) : null;
                if (TempData.ContainsKey("msg"))
                {
                    string messageFromFirstController = TempData["msg"] as string;
                    detail.msg = messageFromFirstController;
                    TempData.Remove("msg");
                }
                return View(detail);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
             
            }

            return View(new DonorMaster());
        }


    }
}
