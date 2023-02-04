namespace Raiden.Data;

internal static class Application
{
    public static ConsoleColor Theme 
        => ConsoleColor.Magenta;

    public static string BaseDirectory 
        => AppDomain.CurrentDomain.BaseDirectory;

    public static string TemplateDirectory 
        => Path.Combine(BaseDirectory, "templates");

    public static string GetPath(ApplicationDirectory directory) => directory switch
    {
        ApplicationDirectory.Scripts => Path.Combine(TemplateDirectory, "scripts"),
        ApplicationDirectory.Solutions => Path.Combine(TemplateDirectory, "sln"),
        _ => throw new NotSupportedException(),
    };
}