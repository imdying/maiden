namespace Maid.SRLC
{
    public sealed class Software
    {
        public Software(string version)
        {
            Version = new Version(version);

            if (Version.IsRelease)
                Stage = Resources.Stages[^1];
            else
                Stage = GetKey(Version.Prerelease!.Identifier, true);
        }

        public int IndexOfCurrentLifeCycleFromKey
        {
            get
            {
                for (int i = 0; i < Stages.Length; i++)
                {
                    if (Stage.Level == Stages[i].Level)
                        return i;
                }

                return -1;
            }
        }

        public LifeCycle Stage
        {
            get;
            private set;
        }

        public LifeCycle[] Stages { get; private set; } = Resources.Stages;

        public Version Version
        {
            get; 
            private set;
        }
        public static Software From(Software software)
        {
            var newLifeCycles = new List<LifeCycle>();
            var stages = Resources.Stages;

            for (int i = 0; i < stages.Length; i++)
            {
                var lc = stages[i];

                if (lc.Level >= software.Stage.Level)
                    newLifeCycles.Add(lc);
            }

            return new(software.Version.ToString())
            {
                Stages = newLifeCycles.ToArray()
            };
        }

        public override string ToString() => Version.ToString();

        public void Update(VersionType version, LifeCycle? newState = null)
        {
            switch (version)
            {
                case VersionType.Major:
                    _ = Version.Major++;
                    return;

                case VersionType.Minor:
                    _ = Version.Minor++;
                    return;

                case VersionType.Patch:
                    _ = Version.Patch++;
                    return;

                case VersionType.Prerelease:
                    if (newState is null)
                    {
                        throw new ArgumentNullException(nameof(newState));
                    }
                    Stage = newState;
                    Version.UpdatePrerelease(newState.ToPrerelease());
                    return;

                case VersionType.Revision:
                    if (Version.Prerelease != null)
                    {
                        _ = Version.Prerelease.Revision++;
                    }
                    return;
            }
        }

        private LifeCycle GetKey(string name, bool searchId = false)
        {
            for (int i = 0; i < Stages.Length; i++)
            {
                var lifeCycle = Stages[i];

                if (searchId)
                {
                    if (lifeCycle.Abbreviation.Equals(name))
                        return lifeCycle;
                }
                else
                {
                    if (lifeCycle.Name.Equals(name))
                        return lifeCycle;
                }
            }

            throw new KeyNotFoundException(name);
        }

        /// <returns><see langword="true"/> if succeed.</returns>
        private bool TryGetKey(string name, out LifeCycle? key)
        {
            try
            {
                key = GetKey(name);
            }
            catch (KeyNotFoundException)
            {
                key = null;
            }

            return key is not null;
        }
    }
}