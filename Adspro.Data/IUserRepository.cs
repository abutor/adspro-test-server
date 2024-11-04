using Adspro.Data.Models;
using Dapper;
using System.Data;
using System.Linq;

namespace Adspro.Data
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetUserAsync(Guid id);
        Task<UserEntity?> GetUserByUsernameAsync(string username);
        Task<ICollection<UserEntity>> GetUsersAsync(int page, int limit);
        Task<UserEntity> CreateUserAsync(UserEntity user);
        Task UpdateActiveAsync(Guid id, bool active);
        Task BatchUpdateActiveAsync(Guid[] nextActiveUsers, Guid[] nextInactiveUsers);
        Task DeleteUserAsync(Guid id);
        Task<int> UsersCount();
    }

    public class UserRepository(IDbConnection connection) : IUserRepository
    {
        public async Task<UserEntity?> GetUserAsync(Guid id)
        {
            return await connection.QueryFirstOrDefaultAsync<UserEntity>("[dbo].[GetUser]", new { UserId = id });
        }

        public async Task<ICollection<UserEntity>> GetUsersAsync(int page, int limit)
        {
            return (await connection.QueryAsync<UserEntity>("[dbo].[GetUsers]", new { Page = page, Limit = limit })).ToArray();
        }

        public async Task<UserEntity> CreateUserAsync(UserEntity user)
        {
            user.Id = Guid.NewGuid();
            await connection.ExecuteAsync("[dbo].[CreateUser]", user);
            return user;
        }

        public async Task UpdateActiveAsync(Guid id, bool active)
        {
            await connection.ExecuteAsync("[dbo].[SetUserActivity]", new { UserId = id, Active = active });
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await connection.ExecuteAsync("[dbo].[DeleteUser]", new { UserId = id });
        }

        public async Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            return await connection.QueryFirstOrDefaultAsync<UserEntity>("[dbo].[GetUserByUsername]", new { Username = username });
        }

        public async Task<int> UsersCount()
        {
            return await connection.ExecuteScalarAsync<int>("[dbo].[GetUsersCount]");
        }

        public async Task BatchUpdateActiveAsync(Guid[] activeUsers, Guid[] inactiveUsers)
        {
            var p = new { @ActiveIds = string.Join(",", activeUsers), @InActiveIds = string.Join(",", inactiveUsers) };
            await connection.ExecuteAsync("[dbo].[SetBatchUserActivity]", new { @ActiveIds = string.Join(",", activeUsers), @InActiveIds = string.Join(",", inactiveUsers) });
        }
    }
}
