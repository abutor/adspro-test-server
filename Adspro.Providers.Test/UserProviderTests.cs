using Adspro.Contract.Models;
using Adspro.Data;
using Adspro.Data.Models;
using Adspro.Providers.Helpers;
using Adspro.Providers.Test;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Adspro.Providers.Tests
{
    [TestFixture]
    public class UserProviderTests
    {
        private const string _countCacheKey = "users_count";
        private string GetUserCacheKey(Guid userId) => $"user_{userId}";

        private Mock<IUserRepository> _repositoryMock;
        private Mock<IDistributedCache> _cacheMock;
        private Mock<IMapper> _mapperMock;
        private UserProvider _userProvider;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _cacheMock = new Mock<IDistributedCache>();
            _mapperMock = new Mock<IMapper>();
            _userProvider = new UserProvider(_repositoryMock.Object, _cacheMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetUsersAsync_ShouldReturnPagedUsers_WhenCalled()
        {
            // Arrange
            var page = 1;
            var limit = 10;
            var users = new[] { new UserEntity(), new UserEntity() };
            var mappedUsers = new[] { new UserModel(), new UserModel() };
            var totalCount = 50;

            _repositoryMock.Setup(r => r.GetUsersAsync(page, limit)).ReturnsAsync(users);
            _repositoryMock.Setup(r => r.UsersCount()).ReturnsAsync(totalCount);
            _mapperMock.Setup(m => m.Map<UserModel[]>(users)).Returns(mappedUsers);
            _cacheMock.MockGetOrCreateStruct(_countCacheKey, totalCount);

            // Act
            var result = await _userProvider.GetUsersAsync(page, limit);

            // Assert
            Assert.That(result.Values, Is.EqualTo(mappedUsers));
            Assert.That(result.Total, Is.EqualTo(totalCount));
            Assert.That(result.Page, Is.EqualTo(page));
            Assert.That(result.Limit, Is.EqualTo(limit));
        }

        [Test]
        public async Task UpdateActivityAsync_ShouldUpdateActivityAndRemoveCache_WhenCalled()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var active = true;

            // Act
            await _userProvider.UpdateActivityAsync(userId, active);

            // Assert
            _repositoryMock.Verify(r => r.UpdateActiveAsync(userId, active), Times.Once);
            _cacheMock.Verify(c => c.Remove(GetUserCacheKey(userId)), Times.Once);
        }

        [Test]
        public async Task UpdateBatchActivityAsync_ShouldUpdateActivityAndRemoveCacheForEachUser_WhenCalled()
        {
            // Arrange
            var updates = new[]
            {
                new UserActiveFlagUpdateModel { Id = Guid.NewGuid(), Active = true },
                new UserActiveFlagUpdateModel { Id = Guid.NewGuid(), Active = false }
            };
            var activeIds = updates.Where(u => u.Active).Select(u => u.Id).ToArray();
            var inactiveIds = updates.Where(u => !u.Active).Select(u => u.Id).ToArray();

            // Act
            await _userProvider.UpdateBatchActivityAsync(updates);

            // Assert
            _repositoryMock.Verify(r => r.BatchUpdateActiveAsync(activeIds, inactiveIds), Times.Once);
            foreach (var update in updates)
            {
                _cacheMock.Verify(c => c.Remove(GetUserCacheKey(update.Id)), Times.Once);
            }
        }

        [Test]
        public async Task GetUserById_ShouldReturnUserFromCache_WhenCached()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cachedUser = new UserModel();

            _cacheMock.MockGetOrCreateStruct(GetUserCacheKey(userId), cachedUser);

            // Act
            var result = await _userProvider.GetUserById(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(cachedUser.Id));
            Assert.That(result.Username, Is.EqualTo(cachedUser.Username));
            Assert.That(result.Active, Is.EqualTo(cachedUser.Active));
        }

        [Test]
        public async Task CreateUser_ShouldSaveUserInCacheAndClearCountCache_WhenCalled()
        {
            // Arrange
            var credentials = new UserCredentials { Username = "testuser", Password = "password" };
            var hashedPassword = HashHelper.ComputeHash(credentials.Password);
            var newUser = new UserEntity { Id = Guid.NewGuid(), Username = credentials.Username, Password = hashedPassword };
            var mappedUser = new UserModel { Id = newUser.Id, Username = newUser.Username };

            _repositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<UserEntity>())).ReturnsAsync(newUser);
            _mapperMock.Setup(m => m.Map<UserModel>(newUser)).Returns(mappedUser);

            // Act
            var result = await _userProvider.CreateUser(credentials);

            // Assert
            Assert.That(result, Is.EqualTo(mappedUser));
            _cacheMock.Verify(c => c.Remove(_countCacheKey), Times.Once);
        }

        [Test]
        public async Task DeleteUser_ShouldRemoveUserAndCountCache_WhenCalled()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            await _userProvider.DeleteUser(userId);

            // Assert
            _repositoryMock.Verify(r => r.DeleteUserAsync(userId), Times.Once);
            _cacheMock.Verify(c => c.Remove(GetUserCacheKey(userId)), Times.Once);
            _cacheMock.Verify(c => c.Remove(_countCacheKey), Times.Once);
        }
    }
}
