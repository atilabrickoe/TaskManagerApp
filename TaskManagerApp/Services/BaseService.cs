using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public abstract class BaseService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        protected BaseService(IApiService apiService)
        {
            _apiService = apiService;
        }

        protected async Task<Response<T>> PostAndHandleAsync<T>(string endpoint, object data, IHandleApiResponseService<T> handler)
        {
            var json = JsonSerializer.Serialize(data, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _apiService.PostAsync(endpoint, content);

            return await handler.HandleAsync(httpResponse);
        }

        protected async Task<Response<T>> GetAndHandleAsync<T>(string endpoint,
                                                                IHandleApiResponseService<T> handler,
                                                                List<string>? queryParams = null)
        {
            if (queryParams is not null && queryParams.Count > 0)
            {
                foreach (string param in queryParams)
                {
                    endpoint += $"/{param}";
                }

            }

            var httpResponse = await _apiService.GetAsync(endpoint);

            var result = await handler.HandleAsync(httpResponse);

            return result;
        }
        protected async Task<Response<T>> DeleteAndHandleAsync<T>(string endpoint,
                                                                  IHandleApiResponseService<T> handler,
                                                                  Guid id)
        {
            endpoint += $"/{id}";

            var httpResponse = await _apiService.DeleteAsync(endpoint);
            //in the case of delete, it does not call the IHandleApiResponseService as it only returns the status code with a success or error message.
            if (httpResponse.IsSuccessStatusCode)
            {
                return new Response<T>
                {
                    Success = true,
                    Data = default
                };
            }
            else
            {
                return new Response<T>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = "Erro ao deletar o item."
                };
            }
        }

    }
}
