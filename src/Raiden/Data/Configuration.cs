using System.Xml;
using System.Xml.Serialization;

namespace Raiden.Data;

[Serializable]
[XmlRoot]
public sealed class Configuration
{
    private ConfigurationMetadata? _build;
    private string? _source;

    public ConfigurationMetadata Build
    {
        get => _build ??= new();
        set => _build = value;
    }

    public string? Script
    {
        get;
        set;
    }

    public bool UpdateBeforeScript
    {
        get;
        set;
    }

    public string Version => Build.Version;

    [XmlIgnore]
    private string Source
    {
        get
        {
            ArgumentException.ThrowIfNullOrEmpty(_source);
            return _source;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(Source));
            }

            _source = value;
        }
    }

    public static void Create(string path, string script)
    {
        var cfg = new Configuration
        {
            Source = path,
            Script = script
        };

        cfg.Save();
    }

    public static Configuration Read(string path)
    {
        Configuration cfg;

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }

        cfg = Deserialize(path);
        cfg.Source = Path.GetFullPath(path);

        return cfg;
    }

    public void Save()
    {
        var serializer = new XmlSerializer(typeof(Configuration));
        using var strWriter = new StreamWriter(Source, false);
        using var xmlWriter = XmlWriter.Create(strWriter, new XmlWriterSettings
        {
            Indent = true
        });

        serializer.Serialize(xmlWriter, this);
    }

    private static Configuration Deserialize(string path)
    {
        var serializer = new XmlSerializer(typeof(Configuration));
        using var reader = XmlReader.Create(path);
        var value = serializer.Deserialize(reader);

        ArgumentNullException.ThrowIfNull(value);

        return (Configuration)value;
    }
}