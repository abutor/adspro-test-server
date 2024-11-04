using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Caching.Distributed;
using AutoMapper;
using Adspro.Contract.Models;
using Adspro.Data;
using Adspro.Providers.Helpers;
using Adspro.Providers;
using Adspro.Data.Models;
using Adspro.Providers.Test;

[TestFixture]
public class AuthProviderTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IDistributedCache> _cacheMock;
    private Mock<IMapper> _mapperMock;
    private AuthProvider _authProvider;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _mapperMock = new Mock<IMapper>();
        _authProvider = new AuthProvider(_userRepositoryMock.Object, _cacheMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task Login_ValidCredentials_ReturnsUserModel()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var passwordHash = HashHelper.ComputeHash(password);
        var userCacheKey = $"user_password_{username}";

        var user = new UserEntity { Username = username, Password = passwordHash, Active = true };
        var userModel = new UserModel { Username = username };

        _cacheMock.MockGet(userCacheKey, passwordHash);
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserModel>(user)).Returns(userModel);

        // Act
        var result = await _authProvider.Login(username, password);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo(username));
    }

    [Test]
    public async Task Login_IncorrectPassword_ReturnsNull()
    {
        // Arrange
        var username = "testuser";
        var password = "wrongpassword";
        var passwordHash = HashHelper.ComputeHash("password123"); // Correct hash
        var wrongPasswordHash = HashHelper.ComputeHash(password);
        var userCacheKey = $"user_password_{username}";

        var user = new UserEntity { Username = username, Password = passwordHash, Active = true };

        _cacheMock.MockGet(userCacheKey, passwordHash);
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(user);

        // Act
        var result = await _authProvider.Login(username, password);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Login_InactiveUser_ReturnsNull()
    {
        // Arrange
        var username = "inactiveuser";
        var password = "password123";
        var passwordHash = HashHelper.ComputeHash(password);
        var userCacheKey = $"user_password_{username}";

        var user = new UserEntity { Username = username, Password = passwordHash, Active = false };

        _cacheMock.MockGet(userCacheKey, passwordHash);
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(user);

        // Act
        var result = await _authProvider.Login(username, password);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Login_CorrectPassword_CacheMisses_ReturnsUserModel()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var passwordHash = HashHelper.ComputeHash(password);
        var userCacheKey = $"user_password_{username}";

        var user = new UserEntity { Username = username, Password = passwordHash, Active = true };
        var userModel = new UserModel { Username = username };

        _cacheMock.MockGet(userCacheKey, passwordHash);
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserModel>(user)).Returns(userModel);

        // Act
        var result = await _authProvider.Login(username, password);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo(username));
        _cacheMock.Verify(cache => cache.Remove(userCacheKey), Times.Once);
    }

    [Test]
    public async Task Login_CorrectPassword_CacheHit_RemovesCache()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var passwordHash = HashHelper.ComputeHash(password);
        var userCacheKey = $"user_password_{username}";

        var user = new UserEntity { Username = username, Password = passwordHash, Active = true };
        var userModel = new UserModel { Username = username };

        _cacheMock.MockGet(userCacheKey, passwordHash);
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserModel>(user)).Returns(userModel);

        // Act
        var result = await _authProvider.Login(username, password);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo(username));
        _cacheMock.Verify(cache => cache.Remove(userCacheKey), Times.Once);
    }
}
