using System.Diagnostics;

namespace Raiden.Runtime.Invocation;

public sealed class Process
{
    private readonly System.Diagnostics.Process _process;

    public Process(string exec)
    {
        if (string.IsNullOrWhiteSpace(exec))
        {
            throw new ArgumentNullException(exec);
        }

        Executable = exec;
        _process = new();
    }

    public string Executable
    {
        get;
        private set;
    }

    public int CreateProcess(string args) => Invoke(args, false, false);

    public int Invoke(string args, bool useShellExecute, bool createNoWindow)
    {
        _process.StartInfo = new ProcessStartInfo()
        {
            Arguments = args,
            FileName = Executable,

            // https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.createnowindow?view=net-7.0#remarks
            CreateNoWindow = createNoWindow,
            
            // https://stackoverflow.com/a/5255335
            UseShellExecute = useShellExecute
        };

        try
        {
            _process.Start();
            _process.WaitForExit();

            return _process.ExitCode;
        }
        finally
        {
            _process.Dispose();
        }
    }

    public int ShellExecute(string args) => Invoke(args, true, false);
}