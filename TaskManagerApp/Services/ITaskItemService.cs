using TaskManagerApp.Models;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public interface ITaskItemService
    {
        Task <Response<TaskItem>> CreateTaskAsync(TaskItem taskItem);
        Task<Response<TaskItem>> UpdateTaskAsync(TaskItem taskItem);
        Task<Response<string>> DeleteTaskByIdAsync(Guid id);
        Task<Response<TaskItem>> GetTaskByIdAsync(Guid id, bool withUser = false);
    }
}
