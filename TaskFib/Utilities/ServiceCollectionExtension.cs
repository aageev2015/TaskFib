using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Numerics;
using TaskFib.Service;
using TaskFib.Service.Contract;
using TaskFib.WebApi.Cache;

namespace TaskFib.WebApi.Utilities
{
    public static class ServiceKeys
    {
        public const string ValuesCache = "cached";
    }

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection TaskFibConfigureSettings(
            this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.Configure<TaskFibSettings>(
                configurationManager.GetSection(TaskFibSettings.AppSettingGroupName));

            return services;
        }

        public static IServiceCollection TaskFibAddIterationsWorkload(this IServiceCollection services)
        {
            services.AddSingleton<IIterationsWorkloadAsync>((IServiceProvider sp) =>
            {
                var settings = sp.GetRequiredService<IOptions<TaskFibSettings>>();
                return new IterationsWorkloadSleepAsync(settings.Value.SleepWorkloadDelayMS);
            });

            return services;
        }

        public static IServiceCollection TaskFibAddFibonacciServices(this IServiceCollection services)
        {
            services.AddSingleton<ISequenceValueServiceAsync<BigInteger>, FibonacciServiceAsync>();
            services.AddSingleton<ISubsequenceServiceAsync<BigInteger>, SubsequenceServiceAsync<BigInteger>>();

            services.AddKeyedSingleton<IMemoryCache, MemoryCache>(ServiceKeys.ValuesCache);
            services.AddKeyedSingleton<ISequenceValueServiceAsync<BigInteger>, SequenceValueCachedProxyService<BigInteger>>(ServiceKeys.ValuesCache);
            services.AddKeyedSingleton<ISubsequenceServiceAsync<BigInteger>, SubsequenceCachedService<BigInteger>>(ServiceKeys.ValuesCache);

            return services;
        }
    }
}
