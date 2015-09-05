using System;
using System.Collections;
using System.Collections.Generic;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration of working with period lengths.
    /// </summary>
    static class SearchLongest
    {
        private const int SIZE = 25;
        private const int SIZEsqrt = 5;

        private class CAwithLength
        {
            public CellularAutomaton ca;
            public long periodLength;

            public CAwithLength(CellularAutomaton CA, long PeriodLength)
            {
                ca = CA;
                periodLength = PeriodLength;
            }
        }

        private class LengthCompare : IComparer<CAwithLength>
        {
            public int Compare(CAwithLength a, CAwithLength b)
            {
                return a.periodLength.CompareTo(b.periodLength);
            }
        }

        /// <summary>
        /// Searches for a CA with the longest period on some sample data.
        /// </summary>
        /// <returns>The longest period.</returns>
        public static long LongestPeriod()
        {
            BitArray input = Utilities.RandomBitArr(SIZE);
            var CAperiods = new SortedSet<CAwithLength>(new LengthCompare());
            CellularAutomaton ca;

            for (int i = 0; i < 256; i++)
            {
                ca = new ElementaryFastAutomaton((byte)i, input);
                var caL = new CAwithLength(ca, Utilities.PeriodLengthFast(ca));
                CAperiods.Add(caL);
            }

            CellularAutomaton conway = (CellularAutomaton)((IBinaryCA)(new GameOfLife(SIZEsqrt, SIZEsqrt))).CloneTemplate(input);
            var conwayL = new CAwithLength(conway, Utilities.PeriodLengthFast(conway));
            CAperiods.Add(conwayL);

            for (int i = 0; i < 1000; i++)
            { 
                var rule = Utilities.RandomBoolArr(32);
                if (i % 2 == 0)
                {
                    ca = new BinaryRangeAutomaton(2, rule, input);
                }
                else
                {
                    ca = new BinaryRangeCyclicAutomaton(2, rule, input);
                }
                var caL = new CAwithLength(ca, Utilities.PeriodLengthFast(ca));
                CAperiods.Add(caL);
            }

            CAwithLength bestCA = CAperiods.Max;
            Console.WriteLine("CA with the longest period ({0}) is:\n {1}", bestCA.periodLength, bestCA.ca.TellType());
            Console.ReadKey();
            Console.WriteLine();
            return bestCA.periodLength;
        }
    }
}
