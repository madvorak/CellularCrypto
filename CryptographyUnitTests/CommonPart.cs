using System;
using System.IO;
using Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographyUnitTests
{
    /// <summary>
    /// A class for use by unit tests to test encryption algorithms.
    /// Initialize this class in [TestInitialize] and then call its methods in [TestMethod]s.
    /// </summary>
    class CommonPart
    {
        private EncrypterWrapper cryptoProvider;

        public CommonPart(EncrypterWrapper encryptionAlgorithm)
        {
            cryptoProvider = encryptionAlgorithm;
        }

        private byte[] encryptDecryptReturn(byte[] data, string password)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream encrypted = new MemoryStream();
            cryptoProvider.Encrypt(input, encrypted, password);
            encrypted.Position = 0;
            MemoryStream decrypted = new MemoryStream();
            cryptoProvider.Decrypt(encrypted, decrypted, password);
            return decrypted.GetBuffer();
        }

        public void test01()
        {
            const string password = "bflmpsvz";
            byte[] data = new byte[] { 56, 129, 13, 207, 0, 0, 15, 42 };
            byte[] result = encryptDecryptReturn(data, password);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], result[i]);
            }
        }

        public void test02()
        {
            const string password = "Moje tajné heslo :)";
            byte[] data = new byte[] { 58, 129, 13, 207, 255, 255, 15, 10 };
            byte[] result = encryptDecryptReturn(data, password);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], result[i]);
            }
        }

        public void test03()
        {
            const string password = "";
            byte[] data = new byte[] { 1, 2, 3 };
            byte[] result = encryptDecryptReturn(data, password);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], result[i]);
            }
        }

        public void testBig()
        {
            const string password = "asdf#&!!<><>0XYZ";
            byte[] data = new byte[10000];
            Random r = new Random();
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)r.Next(256);
            }
            byte[] result = encryptDecryptReturn(data, password);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], result[i]);
            }
        }
    }
}
