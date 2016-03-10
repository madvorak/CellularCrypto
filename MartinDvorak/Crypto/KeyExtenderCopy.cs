using System;
using System.Collections;

namespace Crypto
{
    class KeyExtenderCopy : KeyExtenderAbstract
    {
        public override BitArray DoubleKey(BitArray shortKey)
        {
            int length = shortKey.Length;
            BitArray longKey = new BitArray(2 * length);
            for (int i = 0; i < length; i++)
            {
                longKey[i] = shortKey[i];
                longKey[length + i] = shortKey[i];
            }
            return longKey;
        }
    }
}
