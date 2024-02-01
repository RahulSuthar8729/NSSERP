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

        public IActionResult Index(ImportExcel model)
        {
            if (TempData.ContainsKey("msg"))
            {
                string messageFromFirstController = TempData["msg"] as string;
                model.msg = messageFromFirstController;
                TempData.Remove("msg");
            }
            return View(model);
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
                            TRDATE = DateTime.TryParse(worksheet.Cells[row, 1].Value?.ToString(), out DateTime trDateValue) ? trDateValue : DateTime.MinValue,
                            Bank_Name = worksheet.Cells[row, 2].Value?.ToString(),
                            DESCRIPTION = worksheet.Cells[row, 3].Value?.ToString(),
                            CHEQUENO = worksheet.Cells[row, 4].Value?.ToString(),
                            DR = decimal.TryParse(worksheet.Cells[row, 5].Value?.ToString(), out decimal drValue) ? drValue : 0,
                            CR = decimal.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out decimal crValue) ? crValue : 0,
                            RATE = decimal.TryParse(worksheet.Cells[row, 7].Value?.ToString(), out decimal rateValue) ? rateValue : 0,
                            BAL = decimal.TryParse(worksheet.Cells[row, 8].Value?.ToString(), out decimal balValue) ? balValue : 0, 
                            BRANCH = worksheet.Cells[row, 9].Value?.ToString() ?? string.Empty, 
                        };

                        excelData.Add(data);
                    }


                }

                string ExcelData = System.Text.Json.JsonSerializer.Serialize(excelData);
                var requestBody = new 
                {
                    ExcelData = ExcelData
                };
                string request = System.Text.Json.JsonSerializer.Serialize(requestBody);
                string apiUrl = "api/ImportExcel/InsertExcelData";
                var requestContent = new StringContent(request, Encoding.UTF8, "application/json");
               
                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "ImportExcel");
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
