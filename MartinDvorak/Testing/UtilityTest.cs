using System;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration of utilities for CA.
    /// </summary>
    static class UtilityTest
    {
        public static void RunTest()
        {
            uint[] u = new uint[2] { 123456, 999999999 };
            BinaryCA binCA = new BasicAutomaton(1, Utilities.UintArrToBitArr(u));
            uint[] p = binCA.GetPacked();
            Console.WriteLine(p[0] + " " + p[1]);
            if (System.Linq.Enumerable.SequenceEqual(u, p)) Console.WriteLine("OK");
            else Console.WriteLine("Error!!");
            Console.WriteLine();

            long time;
            CellularAutomaton basicCA = new BasicAutomaton(125, 10);
            Console.WriteLine("Period of length {0} at iteration {1} ", Utilities.PeriodLengthFast(basicCA, long.MaxValue, out time), time);
            CellularAutomaton conway = new GameOfLife(60, 50, 151);
            Console.WriteLine("Period of length {0} at iteration {1} ", Utilities.PeriodLengthFast(conway, long.MaxValue, out time), time);
            BinaryCA basicCA_ = new BasicAutomaton(125, 10);
            Console.WriteLine("Period of length {0} at iteration {1} ", Utilities.PeriodLengthSlow(basicCA_, long.MaxValue, out time), time);
            BinaryCA conway_ = new GameOfLife(60, 50, 151);
            Console.WriteLine("Period of length {0} at iteration {1} ", Utilities.PeriodLengthSlow(conway_, long.MaxValue, out time), time);
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
