using Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographyUnitTests
{
    /// <summary>
    /// This class contains unit tests of encryption using reversible CA.
    /// </summary>
    [TestClass]
    public class EncrypterReversibleCAtests
    {
        private CommonPart common;

        [TestInitialize]
        public void init()
        {
            common = new CommonPart(Export.GetEncrypterReversibleCA());
        }

        [TestMethod]
        public void test01R()
        {
            common.test01();
        }

        [TestMethod]
        public void test02R()
        {
            common.test02();
        }

        [TestMethod]
        public void test03R()
        {
            common.test03();
        }

        [TestMethod]
        public void testBigR()
        {
            common.testBig();
        }
    }
}
