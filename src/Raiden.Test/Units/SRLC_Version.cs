using Raiden.SRLC;
using Version = Raiden.SRLC.Version;

namespace Raiden.Test.Units;

[TestClass]
public class SRLC_Version
{
    private readonly Version _version = new ("1.1.1");

    [TestMethod]
    public void Increment_Build()
    {
        var version = new Version("2.0.0-beta.1");
        version.Update(VersionIdentifier.Build);
        Assert.AreEqual("2.0.0-beta.2", version.ToString());
    }

    [TestMethod]
    public void Increment_Major()
    {
        _version.Update(VersionIdentifier.Major);
        Assert.AreEqual("2.0.0", _version.ToString());
    }

    [TestMethod]
    public void Increment_Minor()
    {
        _version.Update(VersionIdentifier.Minor);
        Assert.AreEqual("1.2.0", _version.ToString());
    }

    [TestMethod]
    public void Increment_Patch()
    {
        _version.Update(VersionIdentifier.Patch);
        Assert.AreEqual("1.1.2", _version.ToString());
    }

    [TestMethod]
    public void Is_Version_A_Prerelease_Version()
    {
        var version = new Version("1.0.0-beta.1");
        Assert.IsTrue(version.IsPrerelease);
    }

    [TestMethod]
    public void Is_Version_A_Release_Version()
    {
        var version = new Version("0.0.0");
        Assert.IsTrue(version.IsRelease);
    }
    [TestMethod]
    public void Prerelease_To_String_Should_Match_Behind_Code()
    {
        var version = new Version("1.0.0-beta.1");
        Assert.IsTrue(version.Prerelease == "beta.1");
    }

    [TestMethod]
    public void Prerelease_Version_To_Release_Version()
    {
        var version = new Version("2.0.0-beta.1");
        version.UpdatePrerelease(new(ReleaseCycle.Gold));

        Assert.AreEqual("2.0.0", version.ToString());
    }

    [TestMethod]
    public void Prerelease_Version_To_String()
    {
        var version = new Version("1.0.0-beta.1");
        Assert.IsTrue(version.ToString() == "1.0.0-beta.1");
    }

    [TestMethod]
    public void Release_Version_To_Prerelease_Version()
    {
        _version.UpdatePrerelease(new(ReleaseCycle.Beta));
        Assert.AreEqual("2.0.0-beta.1", _version.ToString());
    }

    [TestMethod]
    public void Version_To_String()
    {
        var version = new Version("1.0.0");
        Assert.IsTrue(version.ToString() == "1.0.0");
    }
}