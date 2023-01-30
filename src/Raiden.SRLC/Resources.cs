
namespace Raiden.SRLC
{
    internal static class Resources
    {
        public static readonly LifeCycle[] Stages = new LifeCycle[]
        {
            new(1, "alpha", "Alpha"),
            new(2, "beta", "Beta"),
            new(3, "preview", "Preview"),
            new(4, "rc", "Release Candidate"),
            new(int.MaxValue, "gold", "Stable Release")
        };
    }
}
