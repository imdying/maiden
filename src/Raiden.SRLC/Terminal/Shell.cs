namespace Raiden.SRLC.Terminal
{
    internal static class Shell
    {
        public static void WriteLine(string? value, ConsoleColor? color)
        {
            if (color != null)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}