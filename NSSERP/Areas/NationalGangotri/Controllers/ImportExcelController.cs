using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.NationalGangotri.Models;
using NSSERP.DbFunctions;
using OfficeOpenXml;
using System.Net;
using System.Net.Http;
using System.Text;

namespace NSSERP.Areas.NationalGangotri.Controllers
{
    [Authorize]
    [Area("NationalGangotri")]
    public class ImportExcelController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public ImportExcelController( IHttpClientFactory clientFactory, HttpClient httpClient)

        {            
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ConvertExcel(ImportExcel model)
        {
            try
            {
                List<ImportExcel> excelData = new List<ImportExcel>();

                using (var stream = model.excelFile.OpenReadStream())
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    int startRow = 2; // Skip header row
                    int endRow = worksheet.Dimension.End.Row;

                    for (int row = startRow; row <= endRow; row++)
                    {
                        ImportExcel data = new ImportExcel
                        {
                            Column1 = int.Parse(worksheet.Cells[row, 1].Value?.ToString() ?? "0"),
                            Column2 = worksheet.Cells[row, 2].Value?.ToString(),
                            // ... (other columns)
                        };

                        excelData.Add(data);
                    }
                }

                string requestBody = System.Text.Json.JsonSerializer.Serialize(excelData);
                string apiUrl = "api/ImportExcel/InsertExcelData";
                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
               
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    var modeldata = modelget != null ? JsonConvert.DeserializeObject<ImportExcel>(modelget) : null;                    
                    return RedirectToAction("Index", "DonationReceiveMaterDetails", new { model = modeldata });
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
                // Handle exceptions, log them, or return an error response
                return BadRequest("Error converting Excel to JSON: " + ex.Message);
            }
        }

    }
}
