namespace Maid.SRLC
{
    /// <summary>
    /// Describes a stage or life-cycle in a software release life cycle.
    /// </summary>
    public sealed class LifeCycle
    {
        public LifeCycle(int level, string abbr, string name)
        {
            Level = level;
            Abbreviation = abbr;
            Name = name;
        }

        /// <summary>
        /// What is represented in a version number. E.g. X.X.X-beta
        /// </summary>
        public string Abbreviation
        {
            get;
            init;
        }

        public bool IsRelease => Abbreviation == "gold";

        public int Level
        {
            get;
            init;
        }

        /// <summary>
        /// E.g. Beta
        /// </summary>
        public string Name
        {
            get;
            init;
        }

        public Prerelease? ToPrerelease()
        {
            if (IsRelease)
            {
                return null;
            }

            return new Prerelease(Abbreviation, 1);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
