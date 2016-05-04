using System.IO;
using Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographyUnitTests
{
    class CommonPart
    {
        private EncrypterWrapper cryptoProvider;

        public CommonPart(EncrypterWrapper encryptionAlgorithm)
        {
            cryptoProvider = encryptionAlgorithm;
        }

        public void test01()
        {
            const string password = "bflmpsvz";
            byte[] data = new byte[] { 56, 129, 13, 207, 0, 0, 15, 42 };
            MemoryStream input = new MemoryStream(data);
            MemoryStream encrypted = new MemoryStream();
            cryptoProvider.Encrypt(input, encrypted, password);
            encrypted.Position = 0;
            MemoryStream decrypted = new MemoryStream();
            cryptoProvider.Decrypt(encrypted, decrypted, password);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], (decrypted.GetBuffer())[i]);
            }
        }

        public void test02()
        {
            const string password = "Moje tajné heslo :)";
            byte[] data = new byte[] { 58, 129, 13, 207, 255, 255, 15, 10 };
            MemoryStream input = new MemoryStream(data);
            MemoryStream encrypted = new MemoryStream();
            cryptoProvider.Encrypt(input, encrypted, password);
            encrypted.Position = 0;
            MemoryStream decrypted = new MemoryStream();
            cryptoProvider.Decrypt(encrypted, decrypted, password);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], (decrypted.GetBuffer())[i]);
            }
        }
    }
}
