using Raiden.Environment;
using Raiden.SRLC;

namespace Raiden.Commands
{
    public sealed class Build : Command
    {
        // [Argument(1, "Source", "The source directory.")]
        public string? SourceDir
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

        protected override CommandProperty Properties => new()
        {
            Description = "Build a solution."
        };

        protected override void Invocation(object?[] args)
        {
            Release release;
            SourceDir ??= Directory.GetCurrentDirectory();

            try
            {
                Solution = new Solution(SourceDir);
                Configuration = Solution.Configuration;
            }
            catch (FileNotFoundException)
            {
                Cli.WriteError("Configuration file doesn't exist.");
            }

            var scriptLoader = new ScriptLoader(Configuration!.Script);

            // Whether to save before or after successfully running the build-script.
            if (Configuration.UpdateBeforeScript)
                scriptLoader.BeforeShellInvocation += OnUpdating;
            else
                scriptLoader.AfterShellInvocation += OnUpdating;

            // restoring
            if (Configuration.Restore)
            {
                Solution!.Restore();
            }

            // Versioning
            release = new Release(Configuration.Version);
            release.Invoke(Resources.Theme);

            // Updating version in cfg
            Configuration.Build.Update(Configuration.Build.Number + 1,
                                       release.ToString());

            // Script invocation
            scriptLoader.Symbols = new()
            {
                { "{{bNum}}", Configuration.Build.Number.ToString() },
                { "{{bVer}}", Configuration.Build.Version },
                { "{{bScript}}", Path.GetFullPath(Configuration.Script ?? string.Empty) },
                { "{{bVerId}}", release.Version.Stage.Name }
            };

            if (scriptLoader.Execute() != 0)
            {
                Cli.WriteError("Script has exited with a non-zero exit-code.");
            }
        }

        private void OnUpdating(object? sender, EventArgs e)
        {
            Configuration?.Save();
        }
    }
}