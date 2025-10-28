using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;

namespace CourseMapping.Infrastructure.Extensions
{
    public class HybridCacheService : IHybridCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        public HybridCacheService(IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T? value))
                return value;
            
            var cached = await _distributedCache.GetStringAsync(key);
            if (cached == null) return default;
            
            var result = System.Text.Json.JsonSerializer.Deserialize<T>(cached);
            _memoryCache.Set(key, result);
            return result;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            _memoryCache.Set(key, value, absoluteExpiration ?? TimeSpan.FromMinutes(10));
            var serialized = System.Text.Json.JsonSerializer.Serialize(value);

            await _distributedCache.SetStringAsync(key, serialized);
        }

        public async Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            await _distributedCache.RemoveAsync(key);
        }
    }
}
