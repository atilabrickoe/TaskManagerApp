using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enum = TaskManagerApp.Models.Enums;

namespace TaskManagerApp.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Enum.TaskStatus Status { get; set; }
        public Guid IdUser { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }
}
