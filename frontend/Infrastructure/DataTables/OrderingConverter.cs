using Newtonsoft.Json;
using System;

namespace Venjix.Infrastructure.DataTables
{
    public class OrderingConverter : JsonConverter<DataTablesOrdering>
    {
        public override DataTablesOrdering ReadJson(JsonReader reader, Type objectType, DataTablesOrdering existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString() == "asc" ? DataTablesOrdering.Ascending : DataTablesOrdering.Descending;
        }

        public override void WriteJson(JsonWriter writer, DataTablesOrdering value, JsonSerializer serializer)
        {
            if (value == DataTablesOrdering.Ascending)
            {
                writer.WriteValue("asc");
            }
            else
            {
                writer.WriteValue("desc");
            }
        }
    }
}