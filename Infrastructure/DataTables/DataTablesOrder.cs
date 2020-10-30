using System.Text.Json.Serialization;

namespace Venjix.Infrastructure.DataTables
{
    public class DataTablesOrder
    {
        [JsonPropertyName("column")]
        public int Column { get; set; }

        [JsonPropertyName("dir")]
        [JsonConverter(typeof(OrderingConverter))]
        public DataTablesOrdering Direction { get; set; }
    }
}