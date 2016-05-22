﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Intelligent key extender that equalizes frequencies of 0 and 1, which may be different in the underlying CA.
    /// However it is not guaranteed that the long key will be generated.
    /// </summary>
    class KeyExtenderUncertain : KeyExtenderAbstractN
    {
        private const int padding = 10;

        private readonly IBinaryCA ca;

        public KeyExtenderUncertain(IBinaryCA binaryCA)
        {
            ca = binaryCA.CloneEverything();
        }

        public override BitArray ExtendKey(BitArray shortKey, int targetLength)
        {
            BitArray initial = new BitArray(padding + shortKey.Length + padding);
            for (int i = 0; i < shortKey.Length; i++)
            {
                initial[padding + i] = shortKey[i];
            }
            InnerGenerator generator = new InnerGenerator(ca.CloneTemplate(initial), shortKey.Length);
            BitArray longKey = new BitArray(targetLength);
            int index = 0;
            while (index < targetLength)
            {
                bool bitL = generator.GetNext();
                bool bitR = generator.GetNext();
                if (bitL && !bitR)
                {
                    // 10 -> generate 1
                    longKey[index++] = true;
                }
                if (!bitL && bitR)
                {
                    // 01 -> generate 0
                    longKey[index++] = false;
                }
                // 00 and 11 are discarded
            }
            return longKey;
        }

        /// <summary>
        /// This class is instantiated once for every <c>KeyExtenderUncertain.ExtendKey</c> invocation.
        /// It provides bits generated by the CA. It keeps the index where the read should continue 
        /// (when being aware of applied padding) and automatically calls next step of the CA when needed.
        /// It also checks whether the CA always changes its state and doesn't start oscillating.
        /// </summary>
        private class InnerGenerator
        {
            private IBinaryCA innerCA;
            private int position;
            private int size;
            private Dictionary<int, List<uint[]>> visitedStates;

            public InnerGenerator(IBinaryCA generatorCA, int originalSize)
            {
                innerCA = generatorCA;
                innerCA.Step();
                position = 0;
                size = originalSize;
                visitedStates = new Dictionary<int, List<uint[]>>();    // Lookup<int, uint[]> could be used instead
            }

            public bool GetNext()
            {
                if (position < size)
                {
                    return innerCA.GetValueAt((position++) + padding);
                }
                else
                {
                    int hash = innerCA.GetHashCode();
                    if (visitedStates.ContainsKey(hash))
                    {
                        uint[] pack = innerCA.GetPacked();
                        foreach (uint[] past in visitedStates[hash])
                        {
                            if (past.SequenceEqual(pack))
                            {
                                throw new CannotGenerateException("The state of the CA no longer changes.");
                            }
                        }
                        visitedStates[hash].Add(pack);
                    }
                    else
                    {
                        var list = new List<uint[]>();
                        list.Add(innerCA.GetPacked());
                        visitedStates.Add(hash, list);
                    }

                    innerCA.Step();
                    position = 1;
                    return innerCA.GetValueAt(padding);
                }
            }
        }
    }
}
