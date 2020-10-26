using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Venjix.Infrastructure.DataTables
{
    public class OrderingConverter : JsonConverter<DataTablesOrdering>
    {
        public override DataTablesOrdering Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Print(reader.GetString());
            return reader.GetString() == "asc" ? DataTablesOrdering.Ascending : DataTablesOrdering.Descending;
        }

        public override void Write(Utf8JsonWriter writer, DataTablesOrdering value, JsonSerializerOptions options)
        {
            if ((DataTablesOrdering)value == DataTablesOrdering.Ascending)
            {
                writer.WriteStringValue("asc");
            }
            else
            {
                writer.WriteStringValue("desc");
            }
        }
    }
}
