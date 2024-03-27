using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSSERP.Areas.ReceiptBook.Models;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NSSERP.Areas.ReceiptBook.Controllers
{
    [Authorize]
    [Area("ReceiptBook")]
    public class ReceiptBookIssueController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _apiClient;
        public ReceiptBookIssueController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _webHostEnvironment = webHostEnvironment;
            _apiClient = clientFactory.CreateClient("WebApi");
            _httpClientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Home(int id)
        {
            var response = await _apiClient.GetAsync($"api/ReceiptBookIssue/Home?id={id}&DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}");

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
            var detail = json != null ? JsonConvert.DeserializeObject<ReceiptBookIssue>(json) : null;

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);

        }

        [HttpPost]
        public async Task<IActionResult> Home(ReceiptBookIssue model)
        {

            try
            {
                string requestBody = System.Text.Json.JsonSerializer.Serialize(model);
                string apiUrl = string.Empty;
                if (model.book_issue_no != null)
                    apiUrl = "api/ReceiptBookIssue/UpdateData";
                else
                    apiUrl = "api/ReceiptBookIssue/InsertData";


                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = await _apiClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "ReceiptBookIssue", new { model = modelget });
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
            var response = await _apiClient.GetAsync($"api/ReceiptBookIssue/Index?DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}");

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
            var detail = json != null ? JsonConvert.DeserializeObject<ReceiptBookIssue>(json) : null;
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

        [HttpPost]
        public async Task<IActionResult> Index(ReceiptBookIssue model, int id)
        {
            try
            {
                if (model.DocUpload != null && model.DocUpload.Any())
                {
                    var fileNames = await SaveFilesAsync(model.DocUpload);
                    model.Docs = string.Join(",", fileNames);
                }

                var requestData = new
                {
                    Id = id,
                    FilePath = "",
                    FileName = model.Docs,
                    DataFlag = User.FindFirst("DataFlag")?.Value,
                    FYID = 0
                };

                var response = await _apiClient.PostAsJsonAsync("api/ReceiptBookIssue/InsertFileInfo", requestData);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "ReceiptBookIssue", new { model = modelget });
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

        private async Task<List<string>> SaveFilesAsync(List<IFormFile> files)
        {
            var fileNames = new List<string>();

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "DocReceiptBook");
            Directory.CreateDirectory(folderPath);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    fileNames.Add(fileName);
                }
            }

            return fileNames;
        }

        [HttpGet]
        public async Task<IActionResult> DepartmentSubmit(string subByValue, string chkSubValue, int id)
        {
            try
            {
                var response = await _apiClient.GetAsync($"api/ReceiptBookIssue/DepartmentSubmit?id={id}&DataFlag={User.FindFirst("DataFlag")?.Value.ToString()}&subByValue={subByValue}&chkSubValue={chkSubValue}");

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    return Json(new { success = true, message = modelget });
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Json(new { success = false, message = "Resource not found" });
                }
                else
                {
                    return Json(new { success = false, message = $"Error: {response.ReasonPhrase}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TransferPerson(ReceiptBookIssue model, int id, int OldPId, int BookId, int PNoFrom, int PNoTo, string TP)
        {
            try
            {

                var requestData = new
                {
                    ISNO = id,
                    PID = model.RefPersonid,
                    OldPId = OldPId,
                    UserId = User.FindFirst("UserID")?.Value,
                    DataFlag = User.FindFirst("DataFlag")?.Value,
                    BookId = BookId,
                    PNoFrom = PNoFrom,
                    PNoTo = PNoTo,
                    TP = TP
                };

                var response = await _apiClient.PostAsJsonAsync("api/ReceiptBookIssue/TransferPerson", requestData);

                if (response.IsSuccessStatusCode)
                {
                    var modelget = await response.Content.ReadAsStringAsync();
                    TempData["msg"] = modelget;
                    return RedirectToAction("Index", "ReceiptBookIssue", new { model = modelget });
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
