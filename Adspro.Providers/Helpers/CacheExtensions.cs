using Microsoft.Extensions.Caching.Distributed;

namespace Adspro.Providers.Helpers
{
    internal static class CacheExtensions
    {
        public static T? GetObject<T>(this IDistributedCache cache, string key)
        {
            var result = cache.Get(key);
            if (result == null) return default;
            return (T?)SpanJson.JsonSerializer.NonGeneric.Utf8.Deserialize(result, typeof(T));
        }

        public static void SetObject<T>(this IDistributedCache cache, string key, T value)
        {
            var body = SpanJson.JsonSerializer.NonGeneric.Utf8.Serialize(value);
            cache.Set(key, body);
        }

        public static async Task<T> GetOrCreate<T>(this IDistributedCache cache, string key, Func<Task<T>> createFunction)
            where T : class
        {
            var value = GetObject<T?>(cache, key);
            if (value != null)
            {
                return value;
            }

            value = await createFunction();
            cache.SetObject(key, value);

            return value;
        }

        public static async Task<T> GetOrCreateStruct<T>(this IDistributedCache cache, string key, Func<Task<T>> createFunction)
            where T : struct
        {
            var value = await GetOrCreate(cache, key, async () => new StructValue<T>(await createFunction()));
            return value.value;
        }

    }
    internal record StructValue<T>(T value) where T : struct;
}
