using Adspro.Contract.Models;

namespace Adspro.Contract.Providers
{
    public interface IAuthProvider
    {
        Task<UserModel?> Login(string username, string password);
    }
}
