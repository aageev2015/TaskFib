using Microsoft.Extensions.Options;
using System.Numerics;
using TaskFib.Service;
using TaskFib.Service.Contract;

namespace TaskFib.WebApi.Utilities
{
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
            services.AddSingleton<ISubsequenceServiceAsync<BigInteger>, SubsequenceServiceAsync>();

            return services;
        }
    }
}
