using System;
using Cellular;

namespace Testing
{
    static class Implementation2Test
    {
        public static void RunTest()
        {
            BinaryCA one, two;
            for (byte i = 0; i < 255; i++)
            {
                one = new BasicAutomaton(i, 1000);
                two = new BasicAutomaton2(i, 1000);
                (one as CellularAutomaton).Step(300);
                (two as CellularAutomaton).Step(300);
                if (one.GetHashCode() != two.GetHashCode()) Console.WriteLine("Error of Basic Automaton No. " + i);
            }
            Console.WriteLine("Implementation 2 OK");
            Console.ReadKey();
        }
    }
}
