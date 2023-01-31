using Raiden.SRLC.Terminal;

namespace Raiden.SRLC;

public sealed class Release
{
    public Release(string version)
    {
        ArgumentException.ThrowIfNullOrEmpty(version);

        Version = new(version);
        Preversion = version;
    }

    public string Preversion
    {
        get;
        init;
    }

    public Version Version
    {
        get;
        private set;
    }

    private Stage[] Stages => Version.Stage.Cycle.GetCycles();

    public void Invoke(ConsoleColor? theme)
    {
        Shell.WriteLine($"Current version: {Version}", theme);

        if (Version.IsRelease)
        {
            _ = UserPrompt.PromptSelector(
                "Select a version to increment",
                new[] { "Major", "Minor", "Patch", "Pre-release" },
                out var what_to_do,
                theme,
                3
            );

            switch (what_to_do)
            {
                case 0:
                    Version.Update(VersionIdentifier.Major);
                    break;
                case 1:
                    Version.Update(VersionIdentifier.Minor);
                    break;
                case 2:
                    Version.Update(VersionIdentifier.Patch);
                    break;
                case 3:
                    var result = UserPrompt.PromptSelector(
                        "Select the pre-release version to increment to",
                        Stages,
                        theme
                    );

                    Version.UpdatePrerelease(result);
                    break;
            }
        }
        else
        {
            var result = UserPrompt.PromptSelector(
                "Select the (pre-release) version to increment to",
                Stages,
                theme
            );

            if (result.Cycle > Version.Stage.Cycle)
            {
                Version.UpdatePrerelease(result);
            }
            else
            {
                Version.Update(VersionIdentifier.Build);
            }
        }

        Shell.WriteLine($"\nUpgrading from '{Preversion}' to '{Version}'", theme);
    }

    public override string ToString() => Version.ToString();
}