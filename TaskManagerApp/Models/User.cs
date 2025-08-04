using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Models
{
    public partial class User : ObservableObject
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        [ObservableProperty]
        private bool notification;

        [ObservableProperty]
        private string menssageNotification = string.Empty;

        [ObservableProperty]
        private ObservableCollection<TaskItem> tasks = new();
    }
}
