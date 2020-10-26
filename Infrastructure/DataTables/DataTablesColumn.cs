using System.Text.Json.Serialization;

namespace Venjix.Infrastructure.DataTables
{
    public class DataTablesColumn
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("searchable")]
        public bool Searchable { get; set; }

        [JsonPropertyName("orderable")]
        public bool Orderable { get; set; }

        [JsonPropertyName("search")]
        public DataTablesSearch Search { get; set; }
    }
}
