using System.Collections;

namespace Crypto
{
    class EncrypterStreamCA : IEncrypter
    {
        private IKeyExtender xtender;

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
