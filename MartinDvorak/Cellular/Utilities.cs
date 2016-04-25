using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cellular
{
    /// <summary>
    /// Static class packing a group of utilities for working with CA.
    /// </summary>
    static class Utilities
    {
        /// <summary>
        /// Converts an array of <c>System.UInt32</c> into an array of <c>System.Boolean</c>.
        /// The input is treated as MSB-first.
        /// </summary>
        /// <param name="input">Input array.</param>
        /// <returns>Output array.</returns>
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

        /// <summary>
        /// Converts an array of <c>System.UInt32</c> into an array of <c>System.Collections.BitArray</c>.
        /// The input is treated as MSB-first.
        /// </summary>
        /// <param name="input">Input array.</param>
        /// <returns>Output array.</returns>
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

        /// <summary>
        /// Calculates a period (number of steps between repetitions) of a CA.
        /// This method is fast, but unreliable! It gives only the lower estimate (because of hashing).
        /// </summary>
        /// <param name="CA">The examined cellular automaton. Don't worry, this method won't modify it.</param>
        /// <param name="limit">Number of steps that will be performed in no repetition is found earlier.</param>
        /// <param name="time">Output parameter telling the time of a first repetition (second occurence of the same state).
        /// It is the number of steps done since calling this method (not a real time, not a period length).</param>
        /// <returns>If a period is found within the limit, it returns the length of the period.
        /// Otherwise, it returns -1.</returns>
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
                    return i - dict[hash];
                }
                else dict.Add(hash, i);
                ca.Step();
            }
            time = -1;
            return -1;
        }

        /// <summary>
        /// Calculates a period (number of steps between repetitions) of a CA.
        /// This method is fast, but unreliable! It gives only the lower estimate (because of hashing).
        /// </summary>
        /// <param name="CA">The examined cellular automaton. Don't worry, this method won't modify it.</param>
        /// <returns>If a period is found within 2^30 steps, it returns the length of the period.
        /// Otherwise, it returns -1.</returns>
        public static long PeriodLengthFast(CellularAutomaton CA)
        {
            long time;
            return PeriodLengthFast(CA, 1 << 30, out time);
        }

        /// <summary>
        /// Inner structure for saving past states of a binary CA.
        /// </summary>
        private struct Element
        {
            public uint[] packedState;
            public long index;
        }

        /// <summary>
        /// Calculates a period (number of steps between repetitions) of a binary CA.
        /// This method is slower, but reliable.
        /// </summary>
        /// <param name="CA">The examined cellular automaton. Don't worry, this method won't modify it.</param>
        /// <param name="limit">Number of steps that will be performed in no repetition is found earlier.</param>
        /// <param name="time">Output parameter telling the time of a first repetition (second occurence of the same state).
        /// It is the number of steps done since calling this method (not a real time, not a period length).</param>
        /// <returns>If a period is found within the limit, it returns the length of the period.
        /// Otherwise, it returns -1.</returns>
        public static long PeriodLengthSlow(IBinaryCA CA, long limit, out long time)
        {
            IBinaryCA ca = CA.CloneEverything();
            var dict = new Dictionary<int, List<Element>>();  // Lookup<int, Element> could be used instead

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
                    dict[hash].Add(el);   // the same state was not found
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

        /// <summary>
        /// Generates a random array of <c>System.Boolean</c> of given length.
        /// </summary>
        /// <param name="length">The size of the array.</param>
        /// <param name="rnd">Pseudo-random number generator instance.</param>
        /// <returns>Random array.</returns>
        public static bool[] RandomBoolArr(int length, Random rnd)
        {
            bool[] array = new bool[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = rnd.Next(2) == 1;
            }
            return array;
        }

        /// <summary>
        /// Generates a random array of <c>System.Boolean</c> of given length.
        /// The instance of Pseudo-random number generator is created locally, so this method is unsuitable for repeated use.
        /// </summary>
        /// <param name="length">The size of the array.</param>
        /// <returns>Random array.</returns>
        public static bool[] RandomBoolArr(int length)
        {
            return RandomBoolArr(length, Program.rnd);
        }

        /// <summary>
        /// Generates a random array of <c>System.Collections.BitArray</c> of given length.
        /// </summary>
        /// <param name="length">The size of the array.</param>
        /// <param name="rnd">Pseudo-random number generator instance.</param>
        /// <returns>Random array.</returns>
        public static BitArray RandomBitArr(int length, Random rnd)
        {
            BitArray b = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                b[i] = rnd.Next(2) == 1;
            }
            return b;
        }

        /// <summary>
        /// Generates a random array of <c>System.Collections.BitArray</c> of given length.
        /// The instance of Pseudo-random number generator is created locally, so this method is unsuitable for repeated use.
        /// </summary>
        /// <param name="length">The size of the array.</param>
        /// <returns>Random array.</returns>
        public static BitArray RandomBitArr(int length)
        {
            return RandomBitArr(length, Program.rnd);
        }

        /// <summary>
        /// Class for enumerating all binary sequences of a given length. Starting from 00...00, ending at 11...11.
        /// </summary>
        public class AllBinarySequences : IEnumerable<BitArray>
        {
            private int length;

            /// <summary>
            /// Constructor. The amount of generated sequences will be 2 ^ <c>sequenceLength</c>.
            /// </summary>
            /// <param name="sequenceLength"></param>
            public AllBinarySequences(int sequenceLength)
            {
                length = sequenceLength;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return generateAll(length);
            }

            IEnumerator<BitArray> IEnumerable<BitArray>.GetEnumerator()
            {
                return generateAll(length);
            }

            private static IEnumerator<BitArray> generateAll(int length)
            {
                BitArray array = new BitArray(length, false);
                return changeFrom(array, 0);
            }

            private static IEnumerator<BitArray> changeFrom(BitArray array, int index)
            {
                if (index == array.Length)
                {
                    yield return new BitArray(array);
                }
                else
                {
                    IEnumerator<BitArray> start0 = changeFrom(array, index + 1);
                    while (start0.MoveNext())
                    {
                        yield return start0.Current;
                    }
                    array[index] = true;
                    IEnumerator<BitArray> start1 = changeFrom(array, index + 1);
                    while (start1.MoveNext())
                    {
                        yield return start1.Current;
                    }
                    array[index] = false;
                }
            }
        }
    }
}
