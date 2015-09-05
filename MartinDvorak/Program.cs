using System;
using Testing;

class Program
{
    public static Random rnd;

    static void Main(string[] args)
    {
        rnd = new Random();
        SearchLongest.LongestPeriod();
        TotalisticTernaryTest.RunTest();
        AutomataTest.RunTest();
        RandomTestTest.RunTest();
        //CryptoTest.RunTest();
        BinaryRangeNTest.RunTest();
        UtilityTest.RunTest();
        Implementation2Test.RunTest();
        ElementaryTimeMeasure.RunTest();
        SolverTest.RunTest();

        Console.WriteLine("\n________ THE END _______");
        Console.ReadLine();
    }
}