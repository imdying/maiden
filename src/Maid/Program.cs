using System.Text;

namespace Maid
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            return App.Run(new(
                args, 
                "Maid, Meido or \"Maid-Chan\" is a build tool that assists you in versioning your releases."
            ));
        }
    }
}