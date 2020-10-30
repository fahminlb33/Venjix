using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace Venjix.Infrastructure
{
    public interface IWritableOptions<out T> : IOptions<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
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

        public void Update(Action<T> applyChanges)
        {
            applyChanges(Value);
            ExpandoObject obj;

            if (File.Exists(_file))
            {
                var converter = new ExpandoObjectConverter();
                obj = JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(_file), converter);
                ((IDictionary<string, object>)obj)[_section] = Value;
            }
            else
            {
                obj = new ExpandoObject();
                ((IDictionary<string, object>)obj).Add(_section, Value);
            }

            File.WriteAllText(_file, JsonConvert.SerializeObject(obj));
            _configuration.Reload();
        }
    }
}