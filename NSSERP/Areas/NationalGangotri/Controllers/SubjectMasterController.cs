using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.NationalGangotri.Models;
using System.Net;

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
            var response = await _apiClient.GetAsync($"api/DonationReceiveMaster/ViewDetails?id={id}");

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
    }
}
