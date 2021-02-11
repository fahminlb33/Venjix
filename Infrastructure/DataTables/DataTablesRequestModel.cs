using Newtonsoft.Json;
using System.Collections.Generic;

namespace Venjix.Infrastructure.DataTables
{
    public class DataTablesRequestModel
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("search")]
        public DataTablesSearch Search { get; set; }

        [JsonProperty("order")]
        public List<DataTablesOrder> Ordering { get; set; }

        [JsonProperty("columns")]
        public List<DataTablesColumn> Columns { get; set; }
    }
}