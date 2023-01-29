using Maid.Environment;

namespace Maid.Commands
{
    public sealed class Restore : Command
    {
        [Argument(
            1,
            "Path",
            Arity = ArgumentArity.ZeroOrOne)]
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

        protected override CommandProperty Properties => new()
        {
            Synopsis = "Restore a solution.",
            Description =
            "Restore a solution.\n\n" +
            "Essentially, this command \"restores\" the prerequisites and miscellaneous files of a solution that was initialized by Maid."
        };

        protected override void Invocation(object?[] args)
        {
            Source ??= Directory.GetCurrentDirectory();
            Solution = new(Source);

            Solution.Restore();

            Console.WriteLine(
                "Solution restored. " +
                "If you had vs opened, restart and delete the generated `bin` and `obj` from your projects."
            );
        }
    }
}