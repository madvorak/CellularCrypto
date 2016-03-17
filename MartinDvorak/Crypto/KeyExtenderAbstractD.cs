using System.Collections;

namespace Crypto
{
    abstract class KeyExtenderAbstractD : IKeyExtender
    {
        public abstract BitArray DoubleKey(BitArray shortKey);

        public BitArray ExtendKey(BitArray shortKey, int targetLength)
        {
            BitArray key = new BitArray(shortKey);
            while (key.Length < targetLength)
            {
                key = DoubleKey(key);
            }
            BitArray longKey = new BitArray(targetLength);
            for (int i = 0; i < targetLength; i++)
            {
                longKey[i] = key[i];
            }
            return longKey;
        }
    }
}
