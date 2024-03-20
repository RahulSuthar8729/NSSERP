using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.ReceiptBook.Models;
using System.Net;
using System.Text;

namespace NSSERP.Areas.ReceiptBook.Controllers
{
    [Authorize]
    [Area("ReceiptBook")]
    public class ReceiptBookCancellationController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public ReceiptBookCancellationController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }

        public async Task<IActionResult> Home(int id, string tp)
        {
            var dataFlag = User.FindFirst("DataFlag")?.Value.ToString();
            var response = await _apiClient.GetAsync($"api/ReceiptBookCancellation/Home?id={id}&DataFlag={dataFlag}&TP={tp}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Error: {errorContent}");
                }
            }

            var json = await response.Content.ReadAsStringAsync();
            var detail = JsonConvert.DeserializeObject<ReceiptBookCancellation>(json);

           if (detail == null)
            {
                return NotFound();
            }

            return View(detail);
        }


        [HttpPost]
        public async Task<IActionResult> Home(ReceiptBookCancellation model)
        {

            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(model);
                string apiUrl = string.Empty;            
                apiUrl = "api/ReceiptBookCancellation/UpdateData";               

                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "ReceiptBookCancellation", new { model = modelget });
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _apiClient.GetAsync($"api/ReceiptBookCancellation/Index?DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}");

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
            var detail = json != null ? JsonConvert.DeserializeObject<ReceiptbookCancellationDetails>(json) : null;
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
    }
}
