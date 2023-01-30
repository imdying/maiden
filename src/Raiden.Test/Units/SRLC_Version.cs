using Raiden.SRLC;

namespace Raiden.Test.Units
{
    [TestClass]
    public class SRLC_Version
    {
        Software _version = new("1.1.1");

        [TestMethod]
        public void Increment_Major()
        {
            _version.Update(VersionType.Major);
            Assert.AreEqual("2.0.0", _version.ToString());
        }

        [TestMethod]
        public void Increment_Minor()
        {
            _version.Update(VersionType.Minor);
            Assert.AreEqual("1.2.0", _version.ToString());
        }

        [TestMethod]
        public void Increment_Patch()
        {
            _version.Update(VersionType.Patch);
            Assert.AreEqual("1.1.2", _version.ToString());
        }

        [TestMethod]
        public void Increment_Revision()
        {
            var software = new Software("2.0.0-beta.1");
            software.Update(VersionType.Revision);
            Assert.AreEqual("2.0.0-beta.2", software.ToString());
        }

        [TestMethod]
        public void Prerelease_Version_To_Release_Version()
        {
            var soft = new Software("2.0.0-beta.1");
            soft.Update(VersionType.Prerelease, new(0, "gold", null));

            Assert.AreEqual("2.0.0", soft.ToString());
        }

        [TestMethod]
        public void Release_Version_To_Prerelease_Version()
        {
            _version.Update(VersionType.Prerelease, new(0, "beta", null));
            Assert.AreEqual("2.0.0-beta.1", _version.ToString());
        }

        [TestMethod]
        public void Version_To_String()
        {
            Assert.AreEqual("0.0.0", new Software("0.0.0").ToString());
        }
    }
}