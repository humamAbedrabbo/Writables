using Microsoft.Extensions.Options;

namespace Writables.Configurations
{
    public static class DependencyInjection
    {
        public static void ConfigureWritable<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);

            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var configuration = (IConfigurationRoot)provider!.GetRequiredService<IConfiguration>();

                var environment = provider.GetService<IHostEnvironment>();
                
                var options = provider.GetService<IOptionsMonitor<T>>();
                
                return new WritableOptions<T>(environment!, options!, configuration, section.Key, file);
            });
        }
    }
}
