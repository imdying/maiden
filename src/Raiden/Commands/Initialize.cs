using Raiden.Data;

namespace Raiden.Commands
{
    public sealed class Initialize : Command
    {
        private const string SCRIPT_NAME = "build.ps1";

        [Option(
            "--output",
            "Location to place the generated output.",
            Alias = "-o"
        )]
        public string Output { get; set; } = Directory.GetCurrentDirectory();

        protected override CommandProperty Properties => new()
        {
            Name = "init",
            Description = "Generate the necessary configuration files for your project."
        };

        protected override void Invocation(object?[] args)
        {
            if (string.IsNullOrWhiteSpace(Output))
            {
                Cli.WriteError("Output cannot be null or empty.");
            }

            var sln = new Solution(Output);
            var script = Path.Combine(sln.Source.FullName, SCRIPT_NAME);
            var scriptTemplate = Path.Combine(Application.GetPath(ApplicationDirectory.Scripts), SCRIPT_NAME);

            #region Creating configuration
            if (sln.HasConfiguration)
            {
                Cli.WriteError("Directory already has a configuration.");
            }
            else
            {
                Configuration.Create(
                    sln.Config.FullName,
                    Path.GetRelativePath(sln.Source.FullName, script)
                );
            }
            #endregion

            #region Creating build script
            if (File.Exists(script))
            {
                Cli.WriteWarning("Directory already has a build script, skipping generation.");
            }
            else
            {
                Solution.CopyContent(
                    scriptTemplate,
                    script,
                    true
                );
            } 
            #endregion

            Console.WriteLine(
                "Project successfully initialized."
            );
        }
    }
}