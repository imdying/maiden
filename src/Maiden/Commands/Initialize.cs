using Maid.Environment;

namespace Maid.Commands
{
    public sealed class Initialize : Command
    {
        [Option(
            "--config-only",
            "Create only the configuration file.",
            Alias = "-c"
        )]
        public bool CreateConfigOnly
        {
            get;
            set;
        }

        [Option(
            "--name",
            "The name for the output being created. " +
            "If no name is specified, the name of the output directory is used.",
            Alias = "-n")]
        public string? Name
        {
            get;
            set;
        }

        [Option(
            "--output",
            "Location to place the generated output.",
            Alias = "-o")]
        public string? Output
        {
            get;
            set;
        }

        public Solution? Solution
        {
            get;
            set;
        }

        protected override CommandProperty Properties => new()
        {
            Name = "init",
            Synopsis = "Create a solution.",
            Description =
            "Create a solution.\n\n" +
            "In the context of Maid, a solution is more than just a 'Visual Studio Solution'.\n" +
            "It essentially creates a customized sln, alongside Maid's prerequisites.\n\n" +
            "This behavior can be altered by appending a specific flag."
        };

        protected override void Invocation(object?[] args)
        {
            Output ??= Directory.GetCurrentDirectory();
            Name ??= Path.GetFileName(Output) ?? "Undefined";
            Solution = new Solution(Output);

            if (Solution.HasConfiguration)
            {
                Cli.WriteError("Directory already has a configuration.");
            }

            if (CreateConfigOnly)
                InitPrerequisites(Solution);
            else
                Init(Solution, Name, Output);

            Console.WriteLine("Maid was successfully initialized.");
        }

        private void Init(Solution sln, string name, string output)
        {

            if (File.Exists($"{Path.Combine(output, name)}.sln"))
            {
                Cli.WriteError("Directory already has a solution with the same name.");
            }

            // Create sln
            sln?.CreateSolution(name, output);

            // Create config and etc
            sln?.CreatePrerequisites();
        }

        private void InitPrerequisites(Solution sln)
        {
            sln.CreatePrerequisites();
        }
    }
}