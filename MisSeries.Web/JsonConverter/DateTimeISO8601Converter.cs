﻿using System.Text.Json.Serialization;
using System.Text.Json;

namespace MisSeries.Web.JsonConverter;

public class DateTimeISO8601Converter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        return string.IsNullOrEmpty(str) ? default : DateTime.Parse(str);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUniversalTime());
    }
}
