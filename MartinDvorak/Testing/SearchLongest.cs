using System;
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
            var CAperiods = new SortedSet<CAwithLength>(new LengthCompare());
            CellularAutomaton ca;

            for (int i = 0; i < 256; i++)
            {
                ca = new ElementaryFastAutomaton((byte)i, SIZE, Program.rnd);
                var caL = new CAwithLength(ca, Utilities.PeriodLengthFast(ca));
                CAperiods.Add(caL);
            }

            var conway = new GameOfLife(SIZEsqrt, SIZEsqrt);
            var conwayL = new CAwithLength(conway, Utilities.PeriodLengthFast(conway));
            CAperiods.Add(conwayL);

            for (int i = 0; i < 1000; i++)
            { 
                var rule = Utilities.RandomBoolArr(32);
                var initial = Utilities.RandomBitArr(SIZE);
                if (i % 2 == 0)
                {
                    ca = new BinaryRangeAutomaton(2, rule, initial);
                }
                else
                {
                    ca = new BinaryRangeCyclicAutomaton(2, rule, initial);
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
