using System.Collections;

namespace Crypto
{
    public interface IEncrypter
    {
        BitArray Encrypt(BitArray message, BitArray key);
    }
}
