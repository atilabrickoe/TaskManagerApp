using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public interface IHandleApiResponseService<T>
    {
        Task<Response<T>> HandleAsync(HttpResponseMessage response);
    }
}
