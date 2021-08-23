using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Crypto
{
    /// <summary>
    /// Public class that provides encryption and decryption of data streams in a convenient way.
    /// This class wraps any <c>IEncrypter</c> implementation.
    /// </summary>
    public class EncryptionProvider
    {
        private IEncrypter encrypter;
        private HashAlgorithm hashFunction;
        private RandomNumberGenerator secureRandom;
        private const int saltLength = 16;

        /// <summary>
        /// Creates a new <c>EncryptionProvider</c>.
        /// </summary>
        /// <param name="encryptionAlgorithm">Encryption algorithm to be used.</param>
        public EncryptionProvider(IEncrypter encryptionAlgorithm)
        {
            encrypter = encryptionAlgorithm;
            hashFunction = new MD5Cng();
            secureRandom = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Encrypts a stream using a given cryptography key.
        /// </summary>
        /// <param name="inputStream">Input stream (contains plaintext).</param>
        /// <param name="outputStream">Output strean (ciphertext will be written here).</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="salt">The salt is only written at the beginning of the output stream. 
        /// This method uses only the key for encrypting (salt should have been applied earlier).</param>
        public void Encrypt(Stream inputStream, Stream outputStream, BitArray key, byte[] salt)
        {
            if (salt.Length != saltLength)
            {
                throw new ArgumentException("The salt should have exactly" + saltLength + " bytes!");
            }

            byte[] buffer = new byte[inputStream.Length];
            inputStream.Read(buffer, 0, buffer.Length);
            BitArray plaintext = new BitArray(buffer);
            BitArray ciphertext = encrypter.Encrypt(plaintext, key);

            outputStream.Write(salt, 0, salt.Length);
            ciphertext.CopyTo(buffer, 0);
            outputStream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Encrypts a stream using a given cryptography key.
        /// </summary>
        /// <param name="inputStream">Input stream (contains plaintext).</param>
        /// <param name="outputStream">Output strean (ciphertext will be written here).</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="salt">The salt is only written at the beginning of the output stream. 
        /// This method uses only the key for encrypting (salt should have been applied earlier).</param>
        public void Encrypt(Stream inputStream, Stream outputStream, byte[] key, byte[] salt)
        {
            Encrypt(inputStream, outputStream, new BitArray(key), salt);
        }

        /// <summary>
        /// Encrypts a stream. This method uses password and salt to create the encryption key.
        /// </summary>
        /// <param name="inputStream">Input stream (contains plaintext).</param>
        /// <param name="outputStream">Output strean (ciphertext will be written here).</param>
        /// <param name="password">Password from user.</param>
        /// <param name="salt">Salt to create the key (will be written to the output stream, too).</param>
        public void Encrypt(Stream inputStream, Stream outputStream, string password, byte[] salt)
        {
            Encrypt(inputStream, outputStream, calculateHash(password, salt), salt);
        }

        /// <summary>
        /// Encrypts a stream using a given password.
        /// Salt is generated randomly and written at the beginning of the output stream.
        /// </summary>
        /// <param name="inputStream">Input stream (contains plaintext).</param>
        /// <param name="outputStream">Output strean (ciphertext will be written here).</param>
        /// <param name="password">Password from user.</param>
        public void Encrypt(Stream inputStream, Stream outputStream, string password)
        {
            byte[] salt = new byte[saltLength];
            secureRandom.GetBytes(salt);
            Encrypt(inputStream, outputStream, password, salt);
        }

        /// <summary>
        /// Decrypts a stream using a password. Salt is read from the beginning of the stream.
        /// </summary>
        /// <param name="inputStream">Input stream (contains ciphertext).</param>
        /// <param name="outputStream">Output strean (plaintext will be written here).</param>
        /// <param name="password">Password from user.</param>
        public void Decrypt(Stream inputStream, Stream outputStream, string password)
        {
            byte[] buffer = new byte[inputStream.Length - saltLength];
            byte[] salt = new byte[saltLength];
            inputStream.Read(salt, 0, saltLength);
            BitArray key = new BitArray(calculateHash(password, salt));

            inputStream.Read(buffer, 0, buffer.Length);
            BitArray ciphertext = new BitArray(buffer);
            BitArray plaintext = encrypter.Encrypt(ciphertext, key);

            plaintext.CopyTo(buffer, 0);
            outputStream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Calculates MD5 hash from a password and a salt to generate a (short) key.
        /// </summary>
        /// <param name="password">Password from user.</param>
        /// <param name="salt">Random salt.</param>
        /// <returns>Hash / 128-bit key.</returns>
        private byte[] calculateHash(string password, byte[] salt)
        {
            byte[] pswdBytes = Encoding.UTF8.GetBytes(password);
            byte[] pswdWithSalt = new byte[pswdBytes.Length + salt.Length];
            pswdBytes.CopyTo(pswdWithSalt, 0);
            salt.CopyTo(pswdWithSalt, pswdBytes.Length);
            return hashFunction.ComputeHash(pswdWithSalt);
        }
    }
}
