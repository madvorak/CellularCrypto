using Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographyUnitTests
{
    /// <summary>
    /// This class contains unit tests of encryption using CA-based key extenders.
    /// </summary>
    [TestClass]
    public class EncrypterStreamCAtests
    {
        private CommonPart common;

        [TestInitialize]
        public void init()
        {
            common = new CommonPart(Export.GetEncrypterStreamCA());
        }

        [TestMethod]
        public void test01S()
        {
            common.test01();
        }

        [TestMethod]
        public void test02S()
        {
            common.test02();
        }

        [TestMethod]
        public void test03S()
        {
            common.test03();
        }

        [TestMethod]
        public void test04pozS()
        {
            common.test04poz();
        }

        [TestMethod]
        public void test04negS()
        {
            common.test04neg();
        }

        [TestMethod]
        public void testBigS()
        {
            common.testBig();
        }
    }
}
