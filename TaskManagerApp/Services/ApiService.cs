using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace TaskManagerApp.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        //public static readonly string BaseUrl = "https://localhost:7015/";
        public static readonly string BaseUrl = "https://6r2dt60c-7015.brs.devtunnels.ms/";

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> PostRequest(string uri, HttpContent content)
        {
            var urlAdress = BaseUrl + uri;
            try
            {
                return await _httpClient.PostAsync(urlAdress, content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending POST request to {uri}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public async Task<HttpResponseMessage> GetRequest(string uri, Dictionary<string, string>? queryParams = null)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl.TrimEnd('/'))
                      .Append('/')
                      .Append(uri.TrimStart('/'));

            if (queryParams != null && queryParams.Count > 0)
            {
                var query = string.Join("&", queryParams.Select(qp => $"{qp.Key}={Uri.EscapeDataString(qp.Value)}"));
                urlBuilder.Append('?').Append(query);
            }

            try
            {
                return await _httpClient.GetAsync(urlBuilder.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending GET request: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
