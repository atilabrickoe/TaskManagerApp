using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Helpers
{
    public static class NotificationHelper
    {
        public static void ShowNotification(string title, string message)
        {
            var request = new NotificationRequest
            {
                NotificationId = new Random().Next(0, int.MaxValue),
                Title = title,
                Description = message
            };

            LocalNotificationCenter.Current.Show(request);
        }
    }

}
