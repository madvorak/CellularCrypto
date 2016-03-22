using System;
using Cellular;

namespace Testing
{
    static class AutomataTest
    {
        public static void RunTest()
        {
            IBinaryCA demoCA;
            Console.Write("Elementary CA No.: ");
            byte number = byte.Parse(Console.ReadLine());
            demoCA = new ElementaryAutomaton(number, Console.WindowWidth - 1);
            Console.WriteLine(demoCA.StateAsString());
            for (int i = 0; i < 30; i++)
            {
                demoCA.Step();
                Console.WriteLine(demoCA.StateAsString());
            }
            Console.ReadKey();
            Console.WriteLine("Hash " + demoCA.GetHashCode());

            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
