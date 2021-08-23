using System.Collections;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Fake key extender which only generates random long key independently of the short key.
    /// </summary>
    class KeyExtenderCheating : KeyExtenderAbstractN
    {
        public override BitArray ExtendKey(BitArray shortKey, int targetLength)
        {
            return Utilities.RandomBitArr(targetLength);
        }
    }
}
