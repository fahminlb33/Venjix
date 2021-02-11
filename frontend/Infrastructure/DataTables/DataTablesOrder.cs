using Newtonsoft.Json;

namespace Venjix.Infrastructure.DataTables
{
    public class DataTablesOrder
    {
        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("dir")]
        [JsonConverter(typeof(OrderingConverter))]
        public DataTablesOrdering Direction { get; set; }
    }
}