using System;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration of 1D ternary totalistic automata.
    /// </summary>
    static class TotalisticTernaryTest
    {
        public static void RunTest()
        {
            int[] singleGrey = new int[71];
            singleGrey[36] = 1;
            NaryTotalisticAutomaton rule600 = new NaryTotalisticAutomaton(3, new int[] { 0, 2, 0, 1, 1, 2, 0 }, singleGrey);
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine(rule600.PrintTernary());
                rule600.Step();
            }
            Console.WriteLine();

            NaryTotalisticAutomaton rule777 = new NaryTotalisticAutomaton(3, new int[] { 0, 1, 2, 1, 0, 0, 1 }, singleGrey);
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine(rule777.PrintTernary());
                rule777.Step();
            }
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
