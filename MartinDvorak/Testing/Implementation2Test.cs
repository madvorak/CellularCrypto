using System;
using System.Collections;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration that <c>ElementaryAutomaton</c> and <c>ElementaryAutomatonFast</c> and
    /// <c>ElementaryAutomatonFaster</c> and <c>ElementaryAutomatonFastest</c> do the same thing.
    /// </summary>
    static class Implementation2Test
    {
        public static void RunTest()
        {
            IBinaryCA one, two, three, four;
            BitArray initial = Utilities.RandomBitArr(5000);
            for (byte i = 0; i < 255; i++)
            {
                one = new ElementaryAutomaton(i, initial);
                two = new ElementaryAutomatonFast(i, initial);
                three = new ElementaryAutomatonFaster(i, initial);
                four = new ElementaryAutomatonFastest(i, initial);
                (one as CellularAutomaton).Step(10);
                (two as CellularAutomaton).Step(10);
                (three as CellularAutomaton).Step(10);
                (four as CellularAutomaton).Step(10);
                if (one.GetHashCode() != two.GetHashCode())
                {
                    Console.WriteLine("Error of ElementaryAutomatonFast No. " + i);
                }
                if (one.GetHashCode() != three.GetHashCode())
                {
                    Console.WriteLine("Error of ElementaryAutomatonFaster No. " + i);
                }
                if (one.GetHashCode() != four.GetHashCode())
                {
                    Console.WriteLine("Error of ElementaryAutomatonFastest No. " + i);
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine("Implementation 2 OK");
            Console.ReadKey();
        }
    }
}
