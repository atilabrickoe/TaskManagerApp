using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Services
{
    public class NavigationService : INavigationService
    {
        public Task PushAsync(Page page)
        {
            return Shell.Current.Navigation.PushAsync(page);
        }
    }
}
