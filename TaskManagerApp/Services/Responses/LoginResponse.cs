using TaskManagerApp.Models;

namespace TaskManagerApp.Services.Responses
{
    public class LoginResponse
    {
        public Token Token { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string ErrorCode { get; set; }
    }
}
