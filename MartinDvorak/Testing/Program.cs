using System;
using Cellular;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            SolverTest.RunTest();
            AutomataTest.RunTest();
            RandomTestTest.RunTest();
            //CryptoTest.RunTest();
            BinaryRangeNTest.RunTest();
            SearchLongest.LongestPeriod();
            UtilityTest.RunTest();
            Implementation2Test.RunTest();

            Console.ReadLine();
        }
    }
}
