using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Venjix.Infrastructure
{
    public interface IWritableOptions<out T> : IOptions<T> where T : class, new()
    {
        Task Update(Action<T> applyChanges);
    }

    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IOptionsMonitor<T> _options;
        private readonly IConfigurationRoot _configuration;
        private readonly string _section;
        private readonly string _file;

        public WritableOptions(
            IWebHostEnvironment environment,
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

        public async Task Update(Action<T> applyChanges)
        {
            applyChanges(Value);
            ExpandoObject obj;

            if (File.Exists(_file))
            {
                using (var reader = File.OpenRead(_file))
                {
                    obj = await JsonSerializer.DeserializeAsync<ExpandoObject>(reader);
                    ((IDictionary<string, object>)obj)[_section] = Value;
                }
            }
            else
            {
                obj = new ExpandoObject();
                ((IDictionary<string, object>)obj).Add(_section, Value);
            }

            using (var writer = File.OpenWrite(_file))
            {
                writer.SetLength(0);
                await JsonSerializer.SerializeAsync(writer, obj, new JsonSerializerOptions { WriteIndented = true });
            }

            _configuration.Reload();
        }
    }
}
