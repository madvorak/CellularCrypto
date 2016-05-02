using System;
using System.Collections;
using System.Security.Cryptography;

namespace Crypto
{
    /// <summary>
    /// Wrapper around <c>System.Security.Cryptography.AesCryptoServiceProvider</c> for testing purposes.
    /// </summary>
    class KeyExtenderBlockAes : KeyExtenderAbstractN
    {
        public override BitArray ExtendKey(BitArray shortKey, int targetLength)
        {
            var aes = new AesCryptoServiceProvider();
            int arraySize = 16;//(int)Math.Ceiling(shortKey.Length / 8.0);
            var byteKey = new byte[arraySize];
            for (int i = 0; i < shortKey.Length && i / 8 < arraySize; i++)
            {
                if (shortKey[i])
                {
                    byteKey[i / 8] |= (byte)(1 << (i % 8));
                }
            }
            var iv = new byte[arraySize];
            iv[0] = 8;
            var encryptor = aes.CreateEncryptor(byteKey, iv);

            int targetSize = (int)Math.Ceiling(targetLength / 8.0);
            var emptyBuffer = new byte[targetSize];
            var outputBuffer = new byte[targetSize];
            encryptor.TransformBlock(emptyBuffer, 0, targetSize, outputBuffer, 0);
            aes.Dispose();
            BitArray output = new BitArray(targetLength);
            for (int i = 0; i < targetLength; i++)
            {
                output[i] = (outputBuffer[i / 8] & (1 << (i % 8))) > 0;
            }
            return output;
        }
    }
}
