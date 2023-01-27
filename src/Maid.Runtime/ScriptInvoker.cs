using CliWrap;

namespace Maid.Runtime
{
    public sealed class ScriptInvoker
    {
        public event EventHandler? AfterShellInvocation;
        public event EventHandler? BeforeShellInvocation;

        public int Invoke(string content)
        {
            BeforeShellInvocation?.Invoke(null, new());

            var shell = Cli.Wrap("pwsh")
                           .WithArguments(new[] { "-ExecutionPolicy", "Bypass", "-Command", $"&{{{content}}}" })
                           .WithValidation(CommandResultValidation.None);
            var reslt = shell.ExecuteAsync().GetAwaiter().GetResult();

            AfterShellInvocation?.Invoke(null, new());

            return reslt.ExitCode;
        }
    }
}