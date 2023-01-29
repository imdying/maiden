using CliWrap;
using CliWrap.Buffered;

namespace Maid.Environment
{
    public sealed class ScriptLoader
    {
        public ScriptLoader(string? script)
        {
            Script = script ?? string.Empty;
        }

        public event EventHandler? AfterShellInvocation;
        public event EventHandler? BeforeShellInvocation;

        public string Script
        {
            get;
            set;
        }

        public Dictionary<string, string> Symbols { get; set; } = new();

        public int Execute()
        {
            if (!File.Exists(Script))
            {
                // display error
                throw new InvalidOperationException();
            }

            var content = GetScriptContent();
            var modifiedContent = Replace(content);

            BeforeShellInvocation?.Invoke(null, new());

            var shell = Cli.Wrap("pwsh")
                           .WithArguments(new[] { "-ExecutionPolicy", "Bypass", "-Command", $"&{{{modifiedContent}}}" })
                           .WithValidation(CommandResultValidation.None);

            var reslt = shell.ExecuteBufferedAsync().GetAwaiter().GetResult();

            AfterShellInvocation?.Invoke(null, new());

            if (!string.IsNullOrWhiteSpace(reslt.StandardOutput))
            {
                Console.WriteLine(reslt.StandardOutput);
            }

            if (!string.IsNullOrWhiteSpace(reslt.StandardError))
            {
                Console.WriteLine(reslt.StandardError);
            }

            return reslt.ExitCode;
        }

        private static string GetScriptContent()
        {
            return Solution.ReadResource("buildInvoker.ps1");
        }

        private string Replace(string content)
        {
            if (content.Length <= 0)
                throw new InvalidOperationException();

            foreach (var (Key, Value) in Symbols)
            {
                content = content.Replace(Key, Value);
            }

            return content;
        }
    }
}