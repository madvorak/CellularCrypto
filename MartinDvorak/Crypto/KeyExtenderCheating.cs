using System;
using System.Collections;
using Cellular;

namespace Crypto
{
    class KeyExtenderCheating : KeyExtenderAbstract
    {
        public override BitArray DoubleKey(BitArray shortKey)
        {
            return Utilities.RandomBitArr(shortKey.Length * 2);
        }
    }
}
