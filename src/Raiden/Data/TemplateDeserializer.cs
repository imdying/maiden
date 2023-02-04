using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Raiden.Data;

internal sealed class TemplateDeserializer : JsonConverter
{
    private readonly FileInfo _source;

    public TemplateDeserializer(string fileName) => _source = new(fileName);

    public TemplateDeserializer(FileInfo file) => _source = file;

    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType) => (objectType == typeof(Template));

    public override object? ReadJson(JsonReader reader,
                                     Type objectType,
                                     object? existingValue,
                                     JsonSerializer serializer)
    {
        // Load the JSON for the Result into a JObject
        var obj = JObject.Load(reader);

        // Read the properties which will be used as constructor parameters
        var ignore = GetValue<List<string>>(obj, nameof(Template.Ignore));

        // Construct the Result object using the non-default constructor
        return new Template(_source, ignore);
    }

    public override void WriteJson(JsonWriter writer,
                                   object? value,
                                   JsonSerializer serializer) => throw new NotImplementedException();

    private static T? GetValue<T>(JObject? obj, string propName)
    {
        if (obj is null)
        {
            return default;
        }

        var propValue = obj.GetValue(
            propName,
            StringComparison.InvariantCultureIgnoreCase
        );

        if (propValue is null)
        {
            return default;
        }

        return propValue.ToObject<T>();
    }
}