using Adspro.Contract.Models;
using Adspro.Contract.Providers;
using Adspro.Data;
using Adspro.Providers.Helpers;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;

namespace Adspro.Providers
{
    internal class AuthProvider(IUserRepository _userRepository, IDistributedCache _cache, IMapper _mapper) : IAuthProvider
    {
        public async Task<UserModel?> Login(string username, string password)
        {
            var userCacheKey = $"user_password_{username}";
            var passwordHash = HashHelper.ComputeHash(password);
            var cachedPassword = _cache.GetObject<string>(userCacheKey);

            if (!string.IsNullOrEmpty(cachedPassword) && cachedPassword != passwordHash)
            {
                return null;
            }

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !user.Active)
            {
                return null;
            }

            if (user.Password != passwordHash)
            {
                _cache.SetObject(userCacheKey, user.Password);
            }
            else if (!string.IsNullOrEmpty(cachedPassword))
            {
                _cache.Remove(userCacheKey);
            }

            return _mapper.Map<UserModel>(user);
        }
    }
}
