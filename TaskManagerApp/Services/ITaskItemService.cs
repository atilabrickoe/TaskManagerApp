using TaskManagerApp.Models;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public interface ITaskItemService
    {
        Task<Response<string>> DeleteTaskAsync(Guid idTask);
    }
}
