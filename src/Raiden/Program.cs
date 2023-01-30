using System.Text;

namespace Raiden
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            return Cli.Run(new(
                args,
                "Raiden is a build tool designed to streamline the process of versioning software releases."
            ));
        }
    }
}