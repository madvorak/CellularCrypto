using System.Collections;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Simple algorithm, which calls one step of the underlying CA for every bit to be generated.
    /// Generating the long key may take a very long time.
    /// </summary>
    class KeyExtenderSimpleQuadratic : KeyExtenderAbstractD
    {
        private readonly IBinaryCA ca;

        public KeyExtenderSimpleQuadratic(IBinaryCA binaryCA)
        {
            ca = binaryCA.CloneEverything();
        }

        public override BitArray DoubleKey(BitArray shortKey)
        {
            int length = shortKey.Length;
            int padding = 3 * length / 4;
            BitArray initial = new BitArray(padding + length + padding, false);
            for (int i = 0; i < length; i++)
            {
                initial[padding + i] = shortKey[i];
            }
            IBinaryCA generatorCA = ca.CloneTemplate(initial);
            int index = initial.Length / 2;

            BitArray longKey = new BitArray(2 * length);
            for (int i = 0; i < 2 * length; i++)
            {
                generatorCA.Step();
                longKey[i] = generatorCA.GetValueAt(index);
            }
            return longKey;
        }

        internal override string GetInfo()
        {
            return $"SimpleQuadratic using {((CellularAutomaton)ca).TellType()}";
        }
    }
}
