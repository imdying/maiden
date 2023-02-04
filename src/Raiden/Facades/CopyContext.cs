namespace Raiden.Facades;

public sealed class CopyContext : EventArgs
{
    public CopyContext(string? source,
                       string? target,
                       bool overwrite)
    {
        ArgumentException.ThrowIfNullOrEmpty(source, nameof(source));
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));

        Source = source;
        Target = target;
        Overwrite = overwrite;
    }

    public bool Handled
    {
        get;
        set;
    }

    public string Source
    {
        get;
        init;
    }

    public string Target
    {
        get;
        set;
    }

    public bool Overwrite
    {
        get;
        init;
    }
}