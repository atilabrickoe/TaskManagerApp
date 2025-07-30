using System.Text;
using System.Text.Json;
using TaskManagerApp.Models;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public class UserService : IUserService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _serializerOptions;

        public UserService(IApiService apiService)
        {
            _apiService = apiService;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<User?> CreateUserAsync(string userName, string password)
        {
            var user = new User { UserName = userName, Password = password };
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _apiService.PostRequest("User/CreateUser", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseBody, _serializerOptions);
        }

        public async Task<Token?> LoginAsync(string userName, string password)
        {
            var user = new User { UserName = userName, Password = password };
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _apiService.PostRequest("User/Login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseToken = JsonSerializer.Deserialize<LoginResponse>(responseBody, _serializerOptions);
            return responseToken?.Token;
        }
    }
}
