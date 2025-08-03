using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TaskManagerApp.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        public readonly string BaseUrl;

        public ApiService(IConfiguration config, HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            BaseUrl = config["ApiSettings:BaseUrl"] ?? "http://localhost:7015/";
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, HttpContent content)
        {
            var urlAdress = BaseUrl + uri;
            try
            {
                await SetTokenBearer();

                return await _httpClient.PostAsync(urlAdress, content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending POST request to {uri}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }


        public async Task<HttpResponseMessage> GetAsync(string uriWithParans)
        {
            try
            {
                await SetTokenBearer();

                var urlAdress = BaseUrl + uriWithParans;
                return await _httpClient.GetAsync(urlAdress);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending GET request: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        public async Task<HttpResponseMessage> DeleteAsync(string uriWithParans)
        {
            try
            {
                await SetTokenBearer();

                var urlAdress = BaseUrl + uriWithParans;
                return await _httpClient.DeleteAsync(urlAdress);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending Delete request: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        private async Task SetTokenBearer()
        {
            var token = await SecureStorage.GetAsync("access_token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
