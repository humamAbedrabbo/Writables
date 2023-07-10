using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Writables.Configurations;

namespace Writables.Localization;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly IDistributedCache _cache;
    private readonly IOptionsMonitor<ServerSettings> options;

    public JsonStringLocalizerFactory(IDistributedCache cache, IOptionsMonitor<ServerSettings> options)
    {
        _cache = cache;
        this.options = options;
    }

    public IStringLocalizer Create(Type resourceSource) =>
        new JsonStringLocalizer(_cache, options);

    public IStringLocalizer Create(string baseName, string location) =>
        new JsonStringLocalizer(_cache, options);
}

