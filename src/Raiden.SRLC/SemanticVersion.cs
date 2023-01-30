using Semver;

namespace Raiden.SRLC
{
    public abstract class SemanticVersion
    {
        internal readonly SemVersion _version;

        public SemanticVersion(string version)
        {
            _version   = SemVersion.Parse(version, 0, 2048);
            Major      = _version.Major;
            Minor      = _version.Minor;
            Patch      = _version.Patch;
            Metadata   = _version.Metadata;
            Prerelease = _version.Prerelease;
        }

        public bool HasMetadata => Metadata?.Length != 0;

        public bool IsPrerelease => !IsRelease;

        public bool IsRelease => string.IsNullOrWhiteSpace(Prerelease);

        public virtual int Major
        {
            get;
            set;
        }

        public string? Metadata
        {
            get;
            private set;
        }

        public virtual int Minor
        {
            get;
            set;
        }

        public virtual int Patch
        {
            get;
            set;
        }


        public string? Prerelease
        {
            get;
            set;
        }

        public override string ToString() => _version.ToString();
    }
}