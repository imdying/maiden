using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using Raiden.Runtime;

namespace Raiden.Environment;

public abstract class ConfigurationSerializer<T>
{
    public static T Deserialize(string path)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = XmlReader.Create(path);

        return (T)serializer.Deserialize(reader).ShouldNotBeNull();
    }

    /// <summary>
    /// Parse and validate the properties.
    /// </summary>
    /// <remarks>
    /// Validation is done by calling <see cref="PropertyInfo.GetValue(object?)"/> on all 
    /// properties except those marked with <see cref="XmlIgnoreAttribute"/>.
    /// <br/>
    /// This ensures that if a property were to throw an exception on read, would throw before being called.
    /// </remarks>
    public static void ParseAndValidateAllProperties(string path, out T result)
    {
        result = Deserialize(path);

        var @class = result!.GetType();
        var props = @class.GetProperties();

        foreach (var prop in props)
        {
            if (prop.GetCustomAttribute(typeof(XmlIgnoreAttribute)) is not null)
                continue;

            _ = prop.GetValue(result);
        }
    }

    public void Save(string path, object? data)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var strWriter = new StreamWriter(path, false);
        using var xmlWriter = XmlWriter.Create(strWriter, new XmlWriterSettings
        {
            Indent = true
        });

        serializer.Serialize(xmlWriter, data);
    }
}