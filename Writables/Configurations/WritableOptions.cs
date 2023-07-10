using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace Writables.Configurations
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostEnvironment _environment;
        private readonly IOptionsMonitor<T> _options;
        private readonly IConfigurationRoot _configuration;
        private readonly string _section;
        private readonly string _file;

        public WritableOptions(
            IHostEnvironment environment,
            IOptionsMonitor<T> options,
            IConfigurationRoot configuration,
            string section,
            string file)
        {
            _environment = environment;
            _options = options;
            _configuration = configuration;
            _section = section;
            _file = file;
        }

        public T Value => _options.CurrentValue;

        public T Get(string name) => _options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileProvider = _environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(_file);
            var physicalPath = fileInfo.PhysicalPath;

            var jObject = JsonSerializer.Deserialize<JsonObject>(File.ReadAllText(physicalPath));
            
            var sectionObject = JsonSerializer.Deserialize<T>(jObject![_section]!.ToString());

            applyChanges(sectionObject!);

            jObject[_section] = JsonSerializer.SerializeToNode<T>(sectionObject!);

            File.WriteAllText(physicalPath, JsonSerializer.Serialize<JsonObject>(jObject, new JsonSerializerOptions
            {
                WriteIndented = true,
            }));

            _configuration.Reload();
        }
    }
}
