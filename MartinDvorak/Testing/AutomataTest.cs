using System;
using Cellular;

namespace Testing
{
    static class AutomataTest
    {
        public static void RunTest()
        {
            BinaryCA demoCA;
            demoCA = new BasicAutomaton(90, Console.WindowWidth - 1);
            Console.WriteLine(demoCA.StateAsString());
            for (int i = 0; i < 30; i++)
            {
                demoCA.Step();
                Console.WriteLine(demoCA.StateAsString());
            }
            Console.ReadKey();
            Console.WriteLine("Hash " + demoCA.GetHashCode());

            Console.WriteLine();
            Console.WriteLine("Game of Life");
            BinaryCA conway;
            conway = new GameOfLife(20, 20, Program.rnd);
            Console.WriteLine(conway.StateAsString());
            Console.ReadKey();
            for (int i = 0; i < 10; i++)
            {
                conway.Step();
                Console.WriteLine(conway.StateAsString());
            }
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
