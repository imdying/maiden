namespace Raiden.Runtime.Invocation;

public sealed class Script
{
    public Script(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        FileName = path;
        OnInvocation += OnScriptInvocation;
    }

    public event EventHandler? AfterInvocation;
    public event EventHandler? BeforeInvocation;
    private event EventHandler OnInvocation;

    public string FileName
    {
        get;
        init;
    }

    private int ExitCode
    {
        get;
        set;
    }

    public int Invoke()
    {
        if (!File.Exists(FileName))
        {
            throw new FileNotFoundException(FileName);
        }

        BeforeInvocation?.Invoke(
            this, 
            default!
        );

        OnInvocation?.Invoke(
            this, 
            default!
        );

        AfterInvocation?.Invoke(
            this, 
            default!
        );

        return ExitCode;
    }

    private void OnScriptInvocation(object? sender, EventArgs e)
    {
        ExitCode = new Process("pwsh").CreateProcess(
            $"-ExecutionPolicy Bypass -File \"{FileName}\""
        );
    }
}