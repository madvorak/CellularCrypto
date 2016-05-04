using Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographyUnitTests
{
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
    }
}
