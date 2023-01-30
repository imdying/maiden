namespace Raiden.SRLC
{
    public class Controller
    {
        private UserPrompt _userPrompt = new();

        public Controller(string version)
        {
            Software = new(version);
            Preversion = version;
        }

        public Software Software
        {
            get;
            private set;
        }

        public Version Version => Software.Version;

        public string Preversion
        {
            get;
            init;
        }

        public void Invoke(ConsoleColor? theme)
        {
            if (theme != null)
            {
                _userPrompt.Theme = theme.Value;
            }

            Console.WriteLine($"Current version: {Version}");

            if (Version.IsRelease)
            {
                _ = _userPrompt.PromptSelector("Select a version to increment",
                                               new[] { "Major", "Minor", "Patch", "Pre-release" },
                                               out var what,
                                               3);


                switch (what)
                {
                    case 0:
                        Software.Update(VersionType.Major);
                        break;
                    case 1:
                        Software.Update(VersionType.Minor);
                        break;
                    case 2:
                        Software.Update(VersionType.Patch);
                        break;
                    case 3:
                        var result = _userPrompt.PromptSelector("Select the pre-release version to increment to",
                                                                Software.Stages);

                        Software.Update(VersionType.Prerelease, result);
                        break;
                }
            }
            else
            {
                Software = Software.From(Software);
                var result = _userPrompt.PromptSelector("Select the (pre-release) version to increment to",
                                                        Software.Stages,
                                                        defaultValue: Software.IndexOfCurrentLifeCycleFromKey);

                if (result.Level > Software.Stage.Level)
                {
                    Software.Update(VersionType.Prerelease, result);
                }    
                else
                {
                    Software.Update(VersionType.Revision);
                }
            }

            Console.WriteLine($"\nUpgrading from '{Preversion}' to '{Version}'");
        }
    }
}