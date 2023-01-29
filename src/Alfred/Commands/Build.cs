using Maid.Environment;
using Maid.SRLC;

namespace Maid.Commands
{
    public sealed class Build : Command
    {
        public string? Source
        {
            get;
            set;
        }

        private Solution? Solution
        {
            get;
            set;
        }

        private Configuration? Configuration
        {
            get;
            set;
        }

        public Controller? VersionControl
        {
            get;
            set;
        }

        protected override CommandProperty Properties => new()
        {
            Description = "Build a solution."
        };

        protected override void Invocation(object?[] args)
        {
            Source ??= Directory.GetCurrentDirectory();
            Solution = new Solution(Source);
            Configuration = Solution.Configuration;
            VersionControl = new Controller(Configuration.Version);
            var scriptLoader = new ScriptLoader(Configuration.Script);

            // Whether to save before or after successfully running the build-script.
            if (Configuration.UpdateBeforeScript)
                scriptLoader.BeforeShellInvocation += OnUpdating;
            else
                scriptLoader.AfterShellInvocation += OnUpdating;

            // restoring
            if (Configuration.Restore)
            {
                Solution.Restore();
            }

            // Versioning
            VersionControl.Invoke(Resources.Theme);
            Configuration.Build.Update(Configuration.Build.Number+ 1, 
                                       VersionControl.Version.ToString());

            // Script invocation
            scriptLoader.Symbols = new()
            {
                { "{{bNum}}", Configuration.Build.Number.ToString() },
                { "{{bVer}}", Configuration.Build.Version },
                { "{{bScript}}", Path.GetFullPath(Configuration.Script ?? string.Empty) },
                { "{{bVerId}}", VersionControl.Software.Stage.Abbreviation }
            };
            var result = scriptLoader.Execute();

            if (result != 0)
            {
                App.WriteError("Script has exited with a non-zero exit-code.");
            }
        }

        private void OnUpdating(object? sender, EventArgs e)
        {
            Configuration?.Save();
        }
    }
}