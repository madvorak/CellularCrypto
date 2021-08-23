using System.Collections;

namespace Crypto
{
    /// <summary>
    /// Interface for all symmetric cryptography algorithms.
    /// </summary>
    public interface IEncrypter
    {
        /// <summary>
        /// Performs encryption / decryption.
        /// </summary>
        /// <param name="message">Plaintext / ciphertext.</param>
        /// <param name="key">Encryption / decryption key.</param>
        /// <returns>Ciphertext / plaintext.</returns>
        BitArray Encrypt(BitArray message, BitArray key);
    }
}
