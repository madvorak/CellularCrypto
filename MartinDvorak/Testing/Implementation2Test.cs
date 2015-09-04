using System;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration that <c>ElementaryAutomaton</c> and <c>ElementaryFastAutomaton</c> do the same thing.
    /// </summary>
    static class Implementation2Test
    {
        public static void RunTest()
        {
            IBinaryCA one, two;
            for (byte i = 0; i < 255; i++)
            {
                one = new ElementaryAutomaton(i, 1000);
                two = new ElementaryFastAutomaton(i, 1000);
                (one as CellularAutomaton).Step(300);
                (two as CellularAutomaton).Step(300);
                if (one.GetHashCode() != two.GetHashCode()) Console.WriteLine("Error of Basic Automaton No. " + i);
            }
            Console.WriteLine("Implementation 2 OK");
            Console.ReadKey();
        }
    }
}
