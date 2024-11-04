using Adspro.Providers.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using SpanJson;

namespace Adspro.Providers.Tests.Helpers
{
    [TestFixture]
    public class CacheExtensionsTests
    {
        private Mock<IDistributedCache> _cacheMock;

        [SetUp]
        public void SetUp()
        {
            _cacheMock = new Mock<IDistributedCache>();
        }

        [Test]
        public void GetObject_ShouldReturnDeserializedObject_WhenCacheHasData()
        {
            // Arrange
            var key = "test_key";
            var expected = "test_value";
            var serializedValue = JsonSerializer.NonGeneric.Utf8.Serialize(expected);
            _cacheMock.Setup(c => c.Get(key)).Returns(serializedValue);

            // Act
            var result = _cacheMock.Object.GetObject<string>(key);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetObject_ShouldReturnNull_WhenCacheIsEmpty()
        {
            // Arrange
            var key = "test_key";
            _cacheMock.Setup(c => c.Get(key)).Returns((byte[]?)null);

            // Act
            var result = _cacheMock.Object.GetObject<string>(key);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void SetObject_ShouldSerializeAndSetObjectInCache()
        {
            // Arrange
            var key = "test_key";
            var value = "test_value";
            var serializedValue = JsonSerializer.NonGeneric.Utf8.Serialize(value);

            // Act
            _cacheMock.Object.SetObject(key, value);

            // Assert
            _cacheMock.Verify(c => c.Set(key, serializedValue, It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }

        [Test]
        public async Task GetOrCreate_ShouldReturnCachedValue_WhenExists()
        {
            // Arrange
            var key = "test_key";
            var cachedValue = "cached_value";
            var serializedValue = JsonSerializer.NonGeneric.Utf8.Serialize(cachedValue);
            _cacheMock.Setup(c => c.Get(key)).Returns(serializedValue);

            // Act
            var result = await _cacheMock.Object.GetOrCreate(key, () => Task.FromResult("new_value"));

            // Assert
            Assert.That(result, Is.EqualTo(cachedValue));
            _cacheMock.Verify(c => c.Set(key, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()), Times.Never);
        }

        [Test]
        public async Task GetOrCreate_ShouldCallFactoryAndCacheResult_WhenCacheIsEmpty()
        {
            // Arrange
            var key = "test_key";
            var expectedValue = "new_value";
            _cacheMock.Setup(c => c.Get(key)).Returns((byte[]?)null);

            // Act
            var result = await _cacheMock.Object.GetOrCreate(key, () => Task.FromResult(expectedValue));

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
            var serializedValue = JsonSerializer.NonGeneric.Utf8.Serialize(expectedValue);
            _cacheMock.Verify(c => c.Set(key, serializedValue, It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }

        [Test]
        public async Task GetOrCreateStruct_ShouldReturnCachedValue_WhenExists()
        {
            // Arrange
            var key = "struct_key";
            var cachedValue = new StructValue<int>(5);
            var serializedValue = JsonSerializer.NonGeneric.Utf8.Serialize(cachedValue);
            _cacheMock.Setup(c => c.Get(key)).Returns(serializedValue);

            // Act
            var result = await _cacheMock.Object.GetOrCreateStruct(key, () => Task.FromResult(10));

            // Assert
            Assert.That(result, Is.EqualTo(5));
            _cacheMock.Verify(c => c.Set(key, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()), Times.Never);
        }

        [Test]
        public async Task GetOrCreateStruct_ShouldCallFactoryAndCacheResult_WhenCacheIsEmpty()
        {
            // Arrange
            var key = "struct_key";
            var expectedValue = 10;
            _cacheMock.Setup(c => c.Get(key)).Returns((byte[]?)null);

            // Act
            var result = await _cacheMock.Object.GetOrCreateStruct(key, () => Task.FromResult(expectedValue));

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
            var serializedValue = JsonSerializer.NonGeneric.Utf8.Serialize(new StructValue<int>(expectedValue));
            _cacheMock.Verify(c => c.Set(key, serializedValue, It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }
    }
}
