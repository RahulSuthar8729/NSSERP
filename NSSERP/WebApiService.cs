using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using NSSERP.Areas.NationalGangotri.Models;
using Azure;
using OpenQA.Selenium.Remote;

namespace NSSERP
{
    // WebApiService.cs

    public interface IWebApiService
    {
        Task<IEnumerable<DonationReceiveMasterDetails>> GetDonationReciveDetailsAsync(DonationReceiveMasterDetails model, CancellationToken cancellationToken = default);
        // Add other methods as needed
    }

    public class WebApiService : IWebApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WebApiService> _logger;

        public WebApiService(IHttpClientFactory httpClientFactory, ILogger<WebApiService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
            _logger = logger;
        }

        public async Task<IEnumerable<DonationReceiveMasterDetails>> GetDonationReciveDetailsAsync(DonationReceiveMasterDetails model, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/DonationReceiveDetails", model, cancellationToken);

                response.EnsureSuccessStatusCode(); // Ensure the API call was successful

                return await response.Content.ReadFromJsonAsync<IEnumerable<DonationReceiveMasterDetails>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"API call failed: {ex.Message}");               
                throw;
            }
        }
        // Implement other methods
    }
}
