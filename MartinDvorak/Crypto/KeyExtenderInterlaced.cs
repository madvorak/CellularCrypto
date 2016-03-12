using System;
using System.Collections;
using Cellular;

namespace Crypto
{
    class KeyExtenderInterlaced : KeyExtenderAbstract
    {
        private IBinaryCA ca;
        private int rows;
        private int skips;

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
    }
}
