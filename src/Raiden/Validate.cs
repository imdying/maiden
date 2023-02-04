namespace Raiden;

internal static class Validate
{
    public static void ShouldNotBeNullOrEmpty(string? value, string propName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Cli.WriteError($"'{propName}' cannot be null or empty.");
        }
    }

    public static void PathShouldExist(string? path)
    {
        if (!Path.Exists(path))
        {
            Cli.WriteError($"Cannot find '{path}'. Either the path is invalid or doesn't exist.");
        }
    }
}