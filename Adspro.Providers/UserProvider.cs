using Adspro.Contract.Models;
using Adspro.Contract.Providers;
using Adspro.Data;
using Adspro.Data.Models;
using Adspro.Providers.Helpers;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;

namespace Adspro.Providers
{
    internal class UserProvider(IUserRepository _repository, IDistributedCache _cache, IMapper _mapper) : IUserProvider
    {
        private const string _countCacheKey = "users_count";

        public async Task<PagingResult<UserModel>> GetUsersAsync(int page, int limit)
        {
            var users = await _repository.GetUsersAsync(page, limit);
            var count = await _cache.GetOrCreateStruct(_countCacheKey, _repository.UsersCount);

            return new PagingResult<UserModel>
            {
                Values = _mapper.Map<UserModel[]>(users),
                Total = count,
                Limit = limit,
                Page = page
            };
        }

        public async Task UpdateActivityAsync(Guid id, bool active)
        {
            await _repository.UpdateActiveAsync(id, active);
            _cache.Remove(GetUserCacheKey(id));
        }

        public async Task UpdateBatchActivityAsync(UserActiveFlagUpdateModel[] updates)
        {
            var active = updates.Where(x => x.Active).Select(x => x.Id).ToArray();
            var inActive = updates.Where(x => !x.Active).Select(x => x.Id).ToArray();

            await _repository.BatchUpdateActiveAsync(active, inActive);

            foreach (var update in updates)
            {
                _cache.Remove(GetUserCacheKey(update.Id));
            }
        }

        public async Task<UserModel?> GetUserById(Guid id)
        {
            return await _cache.GetOrCreate(GetUserCacheKey(id), async () =>
            {
                var user = await _repository.GetUserAsync(id);
                return _mapper.Map<UserModel>(user);
            });
        }

        public async Task<UserModel> CreateUser(UserCredentials credentials)
        {
            var entity = await _repository.CreateUserAsync(new UserEntity
            {
                Username = credentials.Username,
                Password = HashHelper.ComputeHash(credentials.Password),
            });

            var model = _mapper.Map<UserModel>(entity);

            _cache.SetObject(GetUserCacheKey(model.Id), model);
            _cache.Remove(_countCacheKey);

            return model;
        }

        public async Task DeleteUser(Guid id)
        {
            await _repository.DeleteUserAsync(id);

            _cache.Remove(GetUserCacheKey(id));
            _cache.Remove(_countCacheKey);
        }

        private string GetUserCacheKey(Guid userId) => $"user_{userId}";
    }
}
