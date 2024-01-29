using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TaskFib.Service.Contract;
using TaskFib.WebApi.Utilities;

namespace TaskFib.WebApi.Cache
{
    public class SequenceValueCachedProxyService_NotWorking<T>(
            ISequenceValueServiceAsync<T> sourceService,
            [FromKeyedServices(ServiceKeys.ValuesCache)] IMemoryCache memoryCache,
            IOptions<TaskFibSettings> settings
        ) : ISequenceValueServiceAsync<T> where T : struct
    {
        private readonly ISequenceValueServiceAsync<T> _wrappedService = sourceService;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly int _expirationSeconds = settings.Value.SequenceValueCacheExpirationSeconds;

        public async Task<T> Get(int index, CancellationToken ct = default)
        {
            while (true)
            {
                SequenceCacheItem<T>? cacheItem;
                lock (_memoryCache)
                {
                    cacheItem = _memoryCache.GetOrCreate(index, cacheEntry =>
                    {
                        cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(_expirationSeconds);
                        return new SequenceCacheItem<T>();
                    });
                }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (cacheItem.IsNewOneTimeRead)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                {
                    var value = await _wrappedService.Get(index, ct);
                    if (ct.IsCancellationRequested)
                    {
                        // TODO: test cancellation with cache
                        _memoryCache.Remove(index);
                        return default;
                    }
                    cacheItem.Value = value;
                    return value;
                }
                else
                {
                    while (true)
                    {
                        await Task.Delay(20).ConfigureAwait(false);
                        if (ct.IsCancellationRequested)
                        {
                            return default;
                        }
                        if (!_memoryCache.TryGetValue(index, out SequenceCacheItem<T>? waitingCacheItem))
                        {
                            break;
                        }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        if (waitingCacheItem.Value != null)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        {
                            return waitingCacheItem.Value.Value;
                        }
                    }
                }
            }
        }

        public Task<T> Get1(int index, CancellationToken ct = default)
        {
            var task = _memoryCache.Get<Task<T>>(index);
            if (task != null)
            {
                return task;
            }

#pragma warning disable CS8603 // Possible null reference return.
            return _memoryCache.GetOrCreate(index, cacheEntry =>
                    {
                        cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(_expirationSeconds);
                        return _wrappedService.Get(index, ct);
                    });
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
