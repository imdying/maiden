namespace Raiden.Data;

public sealed class ConfigurationMetadata
{
    private string? _version;

    public uint Number
    {
        get;
        set;
    }

    public string Version
    {
        get => _version ??= "0.0.0";
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(Version));
            }

            _version = value;
        }
    }

    public void Update(string ver)
    {
        Version = ver;
        _ = Number++;
    }

    public override string ToString() => string.Format("{0} ({1})", Version, Number);
}