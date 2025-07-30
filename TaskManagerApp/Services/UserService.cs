using TaskManagerApp.Models;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHandleApiResponseService<Token> _loginHandler;
        private readonly IHandleApiResponseService<User> _createUserHandler;
        private readonly IHandleApiResponseService<List<User>> _createUserListHandler;

        public UserService(
            IApiService apiService,
            IHandleApiResponseService<Token> loginHandler,
            IHandleApiResponseService<User> createUserHandler,
            IHandleApiResponseService<List<User>> createUserListHandler)
            : base(apiService)
        {
            _loginHandler = loginHandler;
            _createUserHandler = createUserHandler;
            _createUserListHandler = createUserListHandler;
        }

        public async Task<Response<Token>?> LoginAsync(string username, string password)
        {
            return await PostAndHandleAsync("User/Login", new { username, password }, _loginHandler);
        }

        public async Task<Response<User>?> CreateUserAsync(string username, string password)
        {
            return await PostAndHandleAsync("User/CreateUser", new { username, password }, _createUserHandler);
        }
        public async Task<Response<List<User>>> GetAllUsers(bool withTask)
        {
            return await GetAndHandleAsync("User/GetAllUsers", _createUserListHandler, [withTask.ToString()]);
        }
    }
}
