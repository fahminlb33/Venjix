using Newtonsoft.Json;
using System;
using Venjix.Infrastructure.DataTables;

namespace Venjix.Infrastructure.DTO
{
    public class VisualizeTableRequestDto : DataTablesRequestModel
    {
        [JsonProperty("sensorId")]
        public int SensorId { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
    }
}