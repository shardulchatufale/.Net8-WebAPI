using DotNet8WebAPI.Model;

namespace DotNet8WebAPI.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?>Authenticate(AuthenticationRequest model);

        Task<IEnumerable<User>> GetALL();
        Task<User> GetByID(int id);

        Task<User?>AddAndUpdateUser(User userObj);
    }
}
