using System;
using Cellular;

namespace Testing
{
    static class AutomataTest
    {
        public static void RunTest()
        {
            IBinaryCA demoCA;
            demoCA = new ElementaryAutomaton(90, Console.WindowWidth - 1);
            Console.WriteLine(demoCA.StateAsString());
            for (int i = 0; i < 30; i++)
            {
                demoCA.Step();
                Console.WriteLine(demoCA.StateAsString());
            }
            Console.ReadKey();
            Console.WriteLine("Hash " + demoCA.GetHashCode());

            Console.WriteLine();
            /*Console.WriteLine("Game of Life");
            IBinaryCA conway;
            conway = new GameOfLife(20, 20);
            Console.WriteLine(conway.StateAsString());
            Console.ReadKey();
            for (int i = 0; i < 10; i++)
            {
                conway.Step();
                Console.WriteLine(conway.StateAsString());
            }*/
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
