using System.Collections;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Simple algorithm, which uses only two steps of the underlying CA.
    /// </summary>
    class KeyExtenderSimpleLinear : KeyExtenderAbstractD
    {
        private readonly IBinaryCA ca;

        public KeyExtenderSimpleLinear(IBinaryCA binaryCA)
        {
            ca = binaryCA.CloneEverything();
        }

        public override BitArray DoubleKey(BitArray shortKey)
        {
            IBinaryCA generatorCA = ca.CloneTemplate(shortKey);
            int length = shortKey.Length;
            BitArray longKey = new BitArray(2 * length);
            generatorCA.Step();
            for (int i = 0; i < length; i++)
            {
                longKey[i] = generatorCA.GetValueAt(i);
            }
            generatorCA.Step();
            for (int i = 0; i < length; i++)
            {
                longKey[i + length] = generatorCA.GetValueAt(i);
            }
            return longKey;
        }

        internal override string GetInfo()
        {
            return $"SimpleLinear using {((CellularAutomaton)ca).TellType()}";
        }
    }
}
