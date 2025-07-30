using TaskManagerApp.Models;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHandleApiResponseService<Token> _loginHandler;
        private readonly IHandleApiResponseService<User> _createUserHandler;

        public UserService(
            IApiService apiService,
            IHandleApiResponseService<Token> loginHandler,
            IHandleApiResponseService<User> createUserHandler)
            : base(apiService)
        {
            _loginHandler = loginHandler;
            _createUserHandler = createUserHandler;
        }

        public async Task<Response<Token>?> LoginAsync(string username, string password)
        {
            return await PostAndHandleAsync("User/Login", new { username, password }, _loginHandler);
        }

        public async Task<Response<User>?> CreateUserAsync(string username, string password)
        {
            return await PostAndHandleAsync("User/CreateUser", new { username, password }, _createUserHandler);
        }
    }
}
