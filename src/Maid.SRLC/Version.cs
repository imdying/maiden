namespace Maid.SRLC
{
    public class Version : SemanticVersion
    {
        private Prerelease? _prerelease;

        public Version(string version) : base(version)
        {
            Prerelease = Prerelease.Parse(base._version);
        }

        public override int Major
        {
            get => base.Major;
            set
            {
                if (IsRelease)
                {
                    base.Major = value;
                    Minor = 0;
                }
            }
        }

        public override int Minor
        {
            get => base.Minor;
            set
            {
                base.Minor = value;
                Patch = 0;
            }
        }

        public override int Patch
        {
            get => base.Patch;
            set => base.Patch = value;
        }

        public new Prerelease? Prerelease
        {
            get
            {
                base.Prerelease = _prerelease?.ToString();
                return _prerelease;
            }
            private set
            {
                _prerelease = value;
                base.Prerelease = value?.ToString();
            }
        }

        public void UpdatePrerelease(Prerelease? newState)
        {
            _ = Major++;
            Prerelease = newState;
        }

        public override string ToString()
        {
            var version = $"{Major}.{Minor}.{Patch}";

            if (IsPrerelease)
            {
                version += $"-{Prerelease}";
            }

            //if (HasMetadata)
            //{
            //    version += $"+{Metadata}";
            //}

            return version;
        }
    }
}