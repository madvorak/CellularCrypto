using Crypto;
using System;
using Cellular;

namespace Testing
{
    static class SolverTest
    {
        public static void RunTest()
        {
            Solver solver = new Solver(9);
            solver.WriteWinningAutomata();
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
