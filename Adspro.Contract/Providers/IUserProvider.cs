using Adspro.Contract.Models;

namespace Adspro.Contract.Providers
{
    public interface IUserProvider
    {
        Task<UserModel?> GetUserById(Guid id);
        Task<PagingResult<UserModel>> GetUsersAsync(int page, int limit);
        Task<UserModel> CreateUser(UserCredentials user);
        Task DeleteUser(Guid id);

        Task UpdateActivityAsync(Guid id, bool active);
        Task UpdateBatchActivityAsync(UserActiveFlagUpdateModel[] updates);
    }
}
