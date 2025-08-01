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
        private readonly IHandleApiResponseService<string> _TaskItemHandler;
        public TaskItemService(IApiService apiService, IHandleApiResponseService<string> taskItemHandler) : base(apiService)
        {
            _TaskItemHandler = taskItemHandler;
        }

        public async Task<Response<string>> DeleteTaskAsync(Guid idTask)
        {
            return await DeleteAndHandleAsync("Task/DeleteTask", _TaskItemHandler, idTask);
        }
    }
}
