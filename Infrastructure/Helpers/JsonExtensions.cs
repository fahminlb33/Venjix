﻿using System;
using System.Buffers;
using System.Text.Json;

namespace Venjix.Infrastructure.Helpers
{
    public static class JsonExtensions
    {
        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                element.WriteTo(writer);
            }

            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }

        public static T ToObject<T>(this JsonDocument document, JsonSerializerOptions options = null)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            return document.RootElement.ToObject<T>(options);
        }

        public static T ToObject<T>(this JsonProperty property, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                writer.WriteStartObject();
                property.WriteTo(writer);
                writer.WriteEndObject();
            }

            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }
    }
}
