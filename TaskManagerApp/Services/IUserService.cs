using TaskManagerApp.Models;

namespace TaskManagerApp.Services
{
    public interface IUserService
    {
        Task<User?> CreateUserAsync(string userName, string password);
        Task<Token?> LoginAsync(string userName, string password);
    }
}