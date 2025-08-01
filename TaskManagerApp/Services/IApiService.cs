using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Services
{
    public interface IApiService
    {
        Task<HttpResponseMessage> PostAsync(string uri, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string uriWithParans);
        Task<HttpResponseMessage> DeleteAsync(string endpoint);
    }
}
