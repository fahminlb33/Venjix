using Newtonsoft.Json;

namespace Venjix.Infrastructure.Services.DataTables
{
    public class DataTablesSearch
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("regex")]
        public bool Regex { get; set; }
    }
}