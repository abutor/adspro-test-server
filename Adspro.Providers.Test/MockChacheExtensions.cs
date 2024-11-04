using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Adspro.Providers.Test
{
    internal static class MockChacheExtensions
    {
        public static void MockGet<T>(this Mock<IDistributedCache> cache, string key, T result)
        {
            var json = SpanJson.JsonSerializer.NonGeneric.Utf8.Serialize(result);
            cache.Setup(x => x.Get(key)).Returns(json);
        }

        public static void MockGetOrCreateStruct<T>(this Mock<IDistributedCache> cache, string key, T value)
        {
            var json = SpanJson.JsonSerializer.NonGeneric.Utf8.Serialize(new { value });
            cache.Setup(x => x.Get(key)).Returns(json);
        }

        public static void MockGetOrCreate<T>(this Mock<IDistributedCache> cache, string key, T value)
        {
            var json = SpanJson.JsonSerializer.NonGeneric.Utf8.Serialize(value);
            cache.Setup(x => x.Get(key)).Returns(json);
        }
    }
}
