using System.Collections;

namespace Crypto
{
    /// <summary>
    /// Abstract class for all key extenders that implement ExtendKey(). Method DoubleKey() simply calls ExtendKey().
    /// </summary>
    abstract class KeyExtenderAbstractN : IKeyExtender
    {
        public BitArray DoubleKey(BitArray shortKey)
        {
            return ExtendKey(shortKey, shortKey.Length * 2);
        }

        public abstract BitArray ExtendKey(BitArray shortKey, int targetLength);
    }
}
