using System;
using Cellular;

namespace Testing
{
    /// <summary>
    /// This test lets the user choose an elementary CA and prints its evolution from a single live cell.
    /// </summary>
    static class AutomataTest
    {
        public static void RunTest()
        {
            IBinaryCA demoCA;
            Console.Write("Elementary CA No.: ");
            byte number = byte.Parse(Console.ReadLine());
            demoCA = new ElementaryAutomaton(number, Console.WindowWidth - 1);
            Console.WriteLine(demoCA.StateAsString());
            for (int i = 0; i < 35; i++)
            {
                demoCA.Step();
                Console.WriteLine(demoCA.StateAsString());
            }

            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
