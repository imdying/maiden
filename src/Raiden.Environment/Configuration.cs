using System.Xml.Serialization;

namespace Raiden.Environment
{
    [Serializable]
    [XmlRoot]
    public sealed partial class Configuration : ConfigurationSerializer<Configuration>
    {
        private static readonly Dictionary<string, Configuration> _store = new();
        private Metadata? _build;

        public Configuration() 
        {
        }

        private Configuration(string src, string script)
        {
            Source = src;
            Script = script;
        }

        public Metadata Build
        {
            get => _build ?? new();
            set => _build = value;
        }

        public bool Restore
        {
            get;
            set;
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

        [XmlIgnore]
        public string Version => Build.Version;

        [XmlIgnore]
        private string? Source
        {
            get;
            set;
        }

        public static Configuration Read(string path) => Read(path, true);

        public static Configuration Read(string path, bool tryCache)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            if (tryCache && _store.TryGetValue(path, out var result))
                return result;

            result = Parse(path);

            if (tryCache)
                _store.Add(path, result);

            return result;
        }

        public void Save() => Save(Source!, this);

        internal static void CreateXml(string path, string script_url) => new Configuration(path, script_url).Save();
        
        private static Configuration Parse(string path)
        {
            // ParseAndValidateAllProperties(path, out var cfg);
            var cfg = Deserialize(path);

            cfg.Source = path;

            return cfg;
        }
    }
}