using System.Text.Json;

namespace WebApi.Common.Utils;

/// <summary>
/// Json serializer utilities to serialize/deserialize web content.
/// </summary>
public static class JsonSerializerUtils
{
    private static readonly JsonSerializerOptions _webOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Serializes the content for web.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>The serialized content.</returns>
    public static string SerializeForWeb(object content)
    {
        return JsonSerializer.Serialize(content, _webOptions);
    }

    /// <summary>
    /// Deserializes a web json to a specific object.
    /// </summary>
    /// <typeparam name="T">The target object type.</typeparam>
    /// <param name="utf8json">The json text representation.</param>
    /// <returns>A <typeparamref name="T"/> representation of the json value.</returns>
    public static async ValueTask<T?> DeserializeFromWebAsync<T>(Stream utf8json)
    {
        return await JsonSerializer.DeserializeAsync<T>(utf8json, _webOptions);
    }
}
