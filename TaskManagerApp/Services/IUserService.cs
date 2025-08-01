using TaskManagerApp.Models;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public interface IUserService
    {
        Task<Response<Token>?> LoginAsync(string username, string password);
        Task<Response<User>?> CreateUserAsync(string username, string password);
        Task<Response<List<User>>> GetAllUsers(bool withTask);
        Task<Response<List<User>>> CreateRandomAsync(int amount);
    }
}