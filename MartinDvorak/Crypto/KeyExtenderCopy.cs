using System.Collections;

namespace Crypto
{
    /// <summary>
    /// Stupid key extender which only copies the input (repeatedly).
    /// </summary>
    class KeyExtenderCopy : KeyExtenderAbstractD
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

        internal override string GetInfo()
        {
            return "Copy";
        }
    }
}
