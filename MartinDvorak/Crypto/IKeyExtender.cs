using System.Collections;

namespace Crypto
{
    interface IKeyExtender
    {
        BitArray DoubleKey(BitArray shortKey);

        BitArray ExtendKey(BitArray shortKey, int targetLength);
    }
}
