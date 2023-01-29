using Semver;

namespace Maid.SRLC
{
    public sealed class Prerelease
    {
        public Prerelease(string id, int rev)
        {
            Identifier = id;
            Revision = (uint)rev;
        }

        public string Identifier
        {
            get;
            init;
        }

        public uint Revision
        {
            get;
            set;
        }

        public static Prerelease? Parse(SemVersion version)
        {
            if (version.IsRelease)
                return null;

            var ids = version.PrereleaseIdentifiers.ToArray();

            if (ids.Length < 1)
                throw new InvalidDataException(nameof(Identifier));

            var id = ids[0];
            var rv = ids.Length < 2 ? 1 : ids[1].NumericValue ?? 1;

            return new(id, rv);
        }

        public override string ToString() => $"{Identifier}.{Revision}";
    }
}