﻿using System.Text.Json;
using System.Text.Json.Nodes;

namespace Lagrange.XocMat.Extensions;

public static class JsonExtension
{
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
    };

    public static T? ToObject<T>(this JsonNode node)
    { 
        return JsonSerializer.Deserialize<T>(node, JsonSerializerOptions);
    }

    public static T? ToObject<T>(this JsonObject obj)
    {
        return JsonSerializer.Deserialize<T>(obj, JsonSerializerOptions);
    }

    public static T? ToObject<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }

    public static JsonObject? ToJsonObject<T>(this T obj)
    { 
        return obj.ToJson().ToObject<JsonObject>();
    }

    public static string ToJson<T>(this T obj)
    { 
        return JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }
}
