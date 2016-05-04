using System.Collections;

namespace Crypto
{
    /// <summary>
    /// Class for symmetric cryptogryphy performed by extending key using CA.
    /// </summary>
    class EncrypterStreamCA : IEncrypter
    {
        private IKeyExtender xtender;

        /// <summary>
        /// Creates a new <c>EncrypterStreamCA</c>.
        /// </summary>
        /// <param name="keyExtender">Key extending algorithms to be used. (CA is inside)</param>
        public EncrypterStreamCA(IKeyExtender keyExtender)
        {
            xtender = keyExtender;
        }

        public BitArray Encrypt(BitArray message, BitArray key)
        {
            BitArray oneTimePad = xtender.ExtendKey(key, message.Length);
            return message.Xor(oneTimePad);
        }
    }
}
