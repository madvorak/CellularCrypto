using System;
using System.Collections;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration that <c>ElementaryAutomaton</c> and <c>ElementaryAutomatonFast</c> and
    /// <c>ElementaryAutomatonFaster</c> and <c>ElementaryAutomatonFastest</c> do the same thing.
    /// </summary>
    static class ElementaryImplementationsTest
    {
        public static void RunTest()
        {
            IBinaryCA one, two, three, four;
            BitArray initial = Utilities.RandomBitArr(1000);
            bool problem = false;

            for (byte i = 0; i < 255; i++)
            {
                one = new ElementaryAutomaton(i, initial);
                two = new ElementaryAutomatonFast(i, initial);
                three = new ElementaryAutomatonFaster(i, initial);
                four = new ElementaryAutomatonParallel(i, initial);
                (one as CellularAutomaton).Step(10);
                (two as CellularAutomaton).Step(10);
                (three as CellularAutomaton).Step(10);
                (four as CellularAutomaton).Step(10);
                if (one.GetHashCode() != two.GetHashCode())
                {
                    Console.WriteLine("Error of ElementaryAutomatonFast No. " + i);
                    problem = true;
                }
                if (one.GetHashCode() != three.GetHashCode())
                {
                    Console.WriteLine("Error of ElementaryAutomatonFaster No. " + i);
                    problem = true;
                }
                if (one.GetHashCode() != four.GetHashCode())
                {
                    Console.WriteLine("Error of ElementaryAutomatonFastest No. " + i);
                    problem = true;
                }
                else
                {
                    Console.Write('.');
                }
            }
            if (problem)
            {
                Console.WriteLine("Problem occured.");
            }
            else
            {
                Console.WriteLine("All 4 implementations OK.");
            }
            Console.ReadKey();
        }
    }
}
