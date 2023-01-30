namespace Raiden.Environment
{
    public sealed partial class Configuration
    {
        public class Metadata
        {
            private string _version = "0.0.0";

            public uint Number
            {
                get;
                set;
            }

            public string Version
            {
                get => _version;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentNullException(nameof(Version));

                    _version = value;
                }
            }

            public void Update(uint num, string ver)
            {
                Number  = num;
                Version = ver;
            }
        }
    }
}