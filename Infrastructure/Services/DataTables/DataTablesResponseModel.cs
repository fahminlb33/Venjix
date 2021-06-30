using Newtonsoft.Json;

namespace Venjix.Infrastructure.Services.DataTables
{
    public class DataTablesResponseModel
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("recordsTotal")]
        public long RecordsTotal { get; set; }

        [JsonProperty("recordsFiltered")]
        public long RecordsFiltered { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}