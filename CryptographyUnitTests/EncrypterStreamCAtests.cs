using Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographyUnitTests
{
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
    }
}
