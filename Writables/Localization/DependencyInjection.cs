using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Writables.Configurations;

namespace Writables.Localization;

public static class DependencyInjection
{
    public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
    {
        services.AddLocalization();
        services.AddDistributedMemoryCache();
        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        services.AddSingleton<IStringLocalizer>(sp =>
        {
            var language = sp.GetRequiredService<IOptionsMonitor<ServerSettings>>()
                .CurrentValue.Language;

            return sp.GetRequiredService<IStringLocalizerFactory>().Create("", "");
        });
        return services;
    }
}


