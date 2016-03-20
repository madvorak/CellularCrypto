using System;
using System.Collections;
using Cellular;

namespace Crypto
{
    class KeyExtenderInterlaced : KeyExtenderAbstractD
    {
        private readonly IBinaryCA ca;
        private readonly int rows;
        private readonly int skips;

        /// <summary>
        /// Creates a new KeyExtenderInterlaced.
        /// </summary>
        /// <param name="binaryCA">What cellular automaton should be used to generate keys.</param>
        /// <param name="rowCount">How many different rows should be used to generate keys.</param>
        /// <param name="skipCount">How many extra steps of the underlying CA should be performed between every use.</param>
        public KeyExtenderInterlaced(IBinaryCA binaryCA, int rowCount, int skipCount)
        {
            if (rowCount < 2)
            {
                throw new ArgumentException("At least 2 rows must be used to generate the longer key!");
            }
            ca = binaryCA.CloneEverything();
            rows = rowCount;
            skips = skipCount;
        }

        public override BitArray DoubleKey(BitArray shortKey)
        {
            IBinaryCA generatorCA = ca.CloneTemplate(shortKey);
            int length = shortKey.Length;
            int fromEvery = (int) Math.Ceiling(2.0 * length / rows);
            int distance = length / fromEvery;
            BitArray longKey = new BitArray(2 * length);
            bool end = false;
            for (int i = 0; i < rows; i++)
            {
                generatorCA.Step();
                for (int j = 0; j < fromEvery / 2; j++)
                {
                    if (i * fromEvery + j >= 2 * length)
                    {
                        end = true;
                        break;
                    }
                    longKey[i * fromEvery + j] = generatorCA.GetValueAt(j * distance + distance - 1);
                }
                if (end)
                {
                    break;
                }
                for (int j = fromEvery / 2; j < fromEvery; j++)
                {
                    if (i * fromEvery + j >= 2 * length)
                    {
                        end = true;
                        break;
                    }
                    longKey[i * fromEvery + j] = generatorCA.GetValueAt(j * distance);
                }
                if (end)
                {
                    break;
                }
                for (int j = 0; j < skips; j++)
                {
                    generatorCA.Step();
                }
            }
            return longKey;
        }

        internal override string GetInfo()
        {
            return $"Interlaced({rows}, {skips}) using {((CellularAutomaton)ca).TellType()}";
        }
    }
}
