using System.Collections;

namespace Crypto
{
    abstract class KeyExtenderAbstractN : IKeyExtender
    {
        public BitArray DoubleKey(BitArray shortKey)
        {
            return ExtendKey(shortKey, shortKey.Length * 2);
        }

        public abstract BitArray ExtendKey(BitArray shortKey, int targetLength);
    }
}
