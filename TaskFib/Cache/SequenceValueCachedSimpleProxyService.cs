using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TaskFib.Service.Contract;
using TaskFib.WebApi.Utilities;

namespace TaskFib.WebApi.Cache
{
    public class SequenceValueCachedSimpleProxyService<T>(
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
            var value = await _memoryCache.GetOrCreateAsync(index, async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(_expirationSeconds);
                return await _wrappedService.Get(index, ct);
            });

            return value;
        }
    }
}
