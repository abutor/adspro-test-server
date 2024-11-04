using Adspro.Contract.Models;
using Adspro.Contract.Providers;

namespace Adspro.Providers
{
    internal class CurrentUserProvider : ICurrentUserProvider
    {
        public bool IsAuthorized => CurrentUser != null;

        public bool IsActive => CurrentUser?.Active == true;

        public UserModel? CurrentUser { get; set; }
    }
}
