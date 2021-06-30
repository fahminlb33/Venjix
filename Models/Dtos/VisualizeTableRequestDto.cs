using System;
using Newtonsoft.Json;
using Venjix.Infrastructure.Services.DataTables;

namespace Venjix.Models.Dtos
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