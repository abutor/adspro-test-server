using Adspro.Contract.Models;

namespace Adspro.Contract.Providers
{
    public interface ITokenProvider
    {
        Guid? GetUserIdFromToken(string token);
        string GetUserIdToken(Guid userId);
    }
}
