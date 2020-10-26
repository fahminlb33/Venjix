using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Venjix.Infrastructure.DataTables
{
    public class DataTablesRequestModel
    {
        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("search")]
        public DataTablesSearch Search { get; set; }

        [JsonPropertyName("order")]
        public List<DataTablesOrder> Ordering { get; set; }

        [JsonPropertyName("columns")]
        public List<DataTablesColumn> Columns { get; set; }
    }
}
