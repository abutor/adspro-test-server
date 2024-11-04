using Adspro.Contract.Models;

namespace Adspro.Contract.Providers
{
    public interface ICurrentUserProvider
    {
        bool IsAuthorized { get; }
        bool IsActive { get; }
        UserModel? CurrentUser { get; set; }
    }
}
