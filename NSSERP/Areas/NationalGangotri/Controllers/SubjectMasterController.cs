using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.NationalGangotri.Models;
using System.Net;
using System.Text;

namespace NSSERP.Areas.NationalGangotri.Controllers
{
    [Authorize]
    [Area("NationalGangotri")]
    public class SubjectMasterController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public SubjectMasterController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Home(int id)
        {
            var response = await _apiClient.GetAsync($"api/SubjectMaster/Home?id={id}&DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}");

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
            var detail = json != null ? JsonConvert.DeserializeObject<SubjectMaster>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);

        }

        [HttpPost]
        public async Task<IActionResult> Home(SubjectMaster model)
        {

            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(model);
                string apiUrl = string.Empty;
                if (model.subject_id != null)
                    apiUrl = "api/SubjectMaster/UpdateData";
                else
                    apiUrl = "api/SubjectMaster/InsertData";


                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "SubjectMaster", new { model = modelget });
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
            var response = await _apiClient.GetAsync($"api/SubjectMaster/Index?DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}");

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
            var detail = json != null ? JsonConvert.DeserializeObject<SubjectMaster>(json) : null;
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
