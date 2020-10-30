using System.Text.Json.Serialization;

namespace Venjix.Infrastructure.DataTables
{
    public class DataTablesSearch
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("regex")]
        public bool Regex { get; set; }
    }
}