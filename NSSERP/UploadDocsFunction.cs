using Microsoft.AspNetCore.Hosting;

namespace NSSERP
{
    public class UploadDocsFunction
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
       
        public UploadDocsFunction(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpClientFactory clientFactory, HttpClient httpClient)

        {
            _webHostEnvironment = webHostEnvironment;          
        }

        public async Task<List<string>> SaveFilesAsync(List<IFormFile> files, string FolderName)
        {
            var fileNames = new List<string>();

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, FolderName);
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

    }
}
