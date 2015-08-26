using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cellular
{
    static class Utilities
    {
        public static bool[] UintArrToBoolArr(uint[] input)
        {
            if (input == null) return null;

            bool[] output = new bool[input.Length * 32];
            for (int i = 0; i < input.Length; i++)
            {
                uint x = input[i];
                for (int j = 31; j >= 0; j--)
                {
                    output[32 * i + j] = x % 2 == 1;
                    x /= 2;
                }
            }
            return output;
        }

        public static BitArray UintArrToBitArr(uint[] input)
        {
            if (input == null) return null;

            BitArray output = new BitArray(input.Length * 32);
            for (int i = 0; i < input.Length; i++)
            {
                uint x = input[i];
                for (int j = 31; j >= 0; j--)
                {
                    output[32 * i + j] = x % 2 == 1;
                    x /= 2;
                }
            }
            return output;
        }

        public static long PeriodLengthFast(CellularAutomaton CA, long limit, out long time)
        {
            CellularAutomaton ca = (CellularAutomaton)CA.Clone();
            var dict = new Dictionary<int, long>();

            for (long i = 0; i < limit; i++)
            {
                int hash = ca.GetHashCode();
                if (dict.ContainsKey(hash))
                {
                    time = ca.GetTime();
                    return i - dict[hash];      // UNRELIABLE !!! - gives only lower estimate (because of hashing)
                }
                else dict.Add(hash, i);
                ca.Step();
            }
            time = -1;
            return -1;
        }

        public static long PeriodLengthFast(CellularAutomaton CA)
        {
            long time;
            return PeriodLengthFast(CA, 1 << 30, out time);
        }

        private struct Element
        {
            public uint[] packedState;
            public long index;
        }

        public static long PeriodLengthSlow(BinaryCA CA, long limit, out long time)
        {
            BinaryCA ca = CA.Clone();
            var dict = new Dictionary<int, List<Element>>();

            for (long i = 0; i < limit; i++)
            {
                int hash = ca.GetHashCode();
                Element el;
                el.packedState = ca.GetPacked();
                el.index = i;
                if (dict.ContainsKey(hash))
                {
                    foreach (Element e in dict[hash])
                    {
                        if (Enumerable.SequenceEqual(e.packedState, el.packedState))
                        {
                            time = (ca as CellularAutomaton).GetTime();
                            return i - e.index;
                        }
                    }
                    dict[hash].Add(el);
                }
                else 
                {
                    var list = new List<Element>();
                    list.Add(el);
                    dict.Add(hash, list);
                }
                ca.Step();
            }
            time = -1;
            return -1;
        }

        public static bool[] RandomBoolArr(uint length, Random rnd)
        {
            bool[] array = new bool[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = rnd.Next(2) == 1;
            }
            return array;
        }

        public static bool[] RandomBoolArr(uint length)
        {
            return RandomBoolArr(length, new Random());
        }

        public static BitArray RandomBitArr(int length, Random rnd)
        {
            BitArray b = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                b[i] = rnd.Next(2) == 1;
            }
            return b;
        }

        public static BitArray RandomBitArr(int length)
        {
            return RandomBitArr(length, new Random());
        }
    }
}
