using System.Text;

using Semver;

namespace Raiden.SRLC;

/// <summary>
/// A specific versioning format used by Raiden.
/// </summary>
public sealed class Version
{
    private int _major;
    private int _minor;
    private int _patch;

    public Version(string version)
    {
        SemVer = SemVersion.Parse(version, 0, 2048);

        _major = SemVer.Major;
        _minor = SemVer.Minor;
        _patch = SemVer.Patch;

        if (SemVer.IsPrerelease)
        {
            var ids = SemVer.PrereleaseIdentifiers.ToArray();

            if (ids.Length < 2)
                throw new InvalidDataException(nameof(ids));

            Stage = new(ReleaseCycleExtensions.GetCycle(ids[0]));
            Build = ids.Length < 2 ? 1 : ids[1].NumericValue ?? 1;
        }
        else
        {
            Stage = new(ReleaseCycle.Gold);
        }
    }

    public int Build
    {
        get;
        private set;
    }

    public bool IsPrerelease => IsRelease == false;

    public bool IsRelease => Identifier == ReleaseCycle.Gold;

    public int Major
    {
        get => _major;
        private set
        {
            if (IsRelease)
            {
                _major = value;
                Minor = 0;
            }
        }
    }

    public int Minor
    {
        get => _minor;
        private set
        {
            _minor = value;
            Patch = 0;
        }
    }

    public int Patch
    {
        get => _patch;
        private set => _patch = value;
    }

    public string? Prerelease
    {
        get
        {
            if (IsPrerelease)
            {
                var prerelease = new StringBuilder();

                prerelease.Append(Stage.Identifier);
                prerelease.Append('.');
                prerelease.Append(Build);

                return prerelease.ToString();
            }
            else
            {
                return null;
            }
        }
    }

    public Stage Stage
    {
        get; 
        private set;
    }

    private ReleaseCycle Identifier => Stage.Cycle;

    private SemVersion SemVer
    {
        get;
        set;
    }

    public override string ToString()
    {
        var version = new StringBuilder();

        version.Append(Major);
        version.Append('.');
        version.Append(Minor);
        version.Append('.');
        version.Append(Patch);

        if (IsPrerelease)
        {
            version.Append('-');
            version.Append(Prerelease);
        }

        return version.ToString();
    }

    internal void Update(VersionIdentifier identifier)
    {
        switch (identifier)
        {
            case VersionIdentifier.Major:
                _ = Major++;
                return;

            case VersionIdentifier.Minor:
                _ = Minor++;
                return;

            case VersionIdentifier.Patch:
                _ = Patch++;
                return;

            case VersionIdentifier.Build:
                _ = Build++;
                return;

            case VersionIdentifier.Revision:
                return;
        }
    }

    internal void UpdatePrerelease(Stage newStage)
    {
        ArgumentNullException.ThrowIfNull(newStage);

        if (newStage.Cycle == Identifier)
            throw new Exception("Wtf are you doing?");

        _ = Major++;
        Stage = newStage;
        Build = 1;
    }
}