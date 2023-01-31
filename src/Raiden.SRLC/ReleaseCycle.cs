using System.ComponentModel;

namespace Raiden.SRLC;

public enum ReleaseCycle
{
    Alpha,

    Beta,

    Preview,

    [Description("Release Candidate")]
    RC,

    [Description("Stable Release")]
    Gold = byte.MaxValue
}