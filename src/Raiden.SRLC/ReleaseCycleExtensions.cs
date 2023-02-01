namespace Raiden.SRLC;

internal static class ReleaseCycleExtensions
{
    public static ReleaseCycle GetCycle(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException("Identifier cannot be null or empty.", nameof(identifier));
        }

        if (!Enum.TryParse(identifier, true, out ReleaseCycle cycle))
        {
            throw new KeyNotFoundException($"Release cycle '{identifier}' is not supported.");
        }

        return cycle;
    }

    /// <summary>
    /// Returns an array of LifeCycle of which is a continuation of the current cycle given.
    /// </summary>
    /// <remarks>
    /// If the given cycle is Gold, an array will be returned without the Gold value.
    /// </remarks>
    public static Stage[] GetCycles(this ReleaseCycle stage)
    {
        var list = new List<Stage>();
        var official = stage == ReleaseCycle.Gold;

        foreach (var cycle in Enum.GetValues(typeof(ReleaseCycle)).Cast<ReleaseCycle>())
        {
            if (official)
            {
                // If the version is a not a prerelease,
                // do not add this one.
                if (cycle == ReleaseCycle.Gold)
                    continue;

                list.Add(new(cycle));
            }
            else
            {
                if (cycle >= stage)
                {
                    list.Add(new(cycle));
                }
            }
        }

        return list.ToArray();
    }

    public static string GetName(this ReleaseCycle cycle)
    {
        var value = Enum.GetName(cycle);

        ArgumentException.ThrowIfNullOrEmpty(value);

        return value;
    }
}