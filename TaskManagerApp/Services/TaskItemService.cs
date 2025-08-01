using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Models;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public class TaskItemService : BaseService, ITaskItemService
    {
        private readonly IHandleApiResponseService<string> _taskItemDeleteHandler;
        private readonly IHandleApiResponseService<TaskItem> _taskItemHandler;
        public TaskItemService(IApiService apiService,
                               IHandleApiResponseService<string> taskItemDeleteHandler,
                               IHandleApiResponseService<TaskItem> taskItemHandler) : base(apiService)
        {
            _taskItemDeleteHandler = taskItemDeleteHandler;
            _taskItemHandler = taskItemHandler;
        }

        public async Task<Response<TaskItem>> CreateTaskAsync(TaskItem taskItem)
        {
            var request = new { Data = taskItem };
            return await PostAndHandleAsync("Task/CreateTask", request, _taskItemHandler);
        }
        public async Task<Response<TaskItem>> UpdateTaskAsync(TaskItem taskItem)
        {
            var request = new { Data = taskItem };
            return await PostAndHandleAsync("Task/UpdateTask", request, _taskItemHandler);
        }
        public async Task<Response<TaskItem>> GetTaskByIdAsync(Guid id, bool withUser = false)
        {
            return await GetAndHandleAsync("Task/GetTaskById", _taskItemHandler, [id.ToString(), withUser.ToString()]);
        }

        public async Task<Response<string>> DeleteTaskByIdAsync(Guid idTask)
        {
            return await DeleteAndHandleAsync("Task/DeleteTask", _taskItemDeleteHandler, idTask);
        }

    }
}
