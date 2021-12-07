using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SouthernCross.Core.Services.HelperServices
{
    public class SimpleMemoryCache : ISimpleMemoryCache
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

        public async Task<T> GetOrCreate<T>(object key, Func<Task<T>> item, int slidingExpiryInMin = 60, int absoluteExpiryInHours = 24)
        {
            T cacheEntry;

            if (!_cache.TryGetValue(key, out cacheEntry))
            {
                SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

                await mylock.WaitAsync();
                try
                {
                    if (!_cache.TryGetValue(key, out cacheEntry))
                    {
                        cacheEntry = await item();

                        if (cacheEntry != null)
                        {
                            var cacheEntryOptions = new MemoryCacheEntryOptions();
                            cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiryInMin));
                            cacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromHours(absoluteExpiryInHours));

                            _cache.Set(key, cacheEntry, cacheEntryOptions);
                        }
                    }
                }
                finally
                {
                    mylock.Release();
                }
            }
            return cacheEntry;
        }
    }
}
