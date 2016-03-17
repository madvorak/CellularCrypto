using System;
using System.Collections;
using Cellular;

namespace Crypto
{
    class KeyExtenderCheating : KeyExtenderAbstractN
    {
        public override BitArray ExtendKey(BitArray shortKey, int targetLength)
        {
            return Utilities.RandomBitArr(targetLength);
        }
    }
}
