using System;
using Testing;

class Program
{
    public static Random rnd;

    static void Main(string[] args)
    {
        rnd = new Random();
        SolverTest.RunTest();
        AutomataTest.RunTest();
        RandomTestTest.RunTest();
        //CryptoTest.RunTest();
        BinaryRangeNTest.RunTest();
        SearchLongest.LongestPeriod();
        UtilityTest.RunTest();
        Implementation2Test.RunTest();

        Console.WriteLine("\n________ THE END _______");
        Console.ReadLine();
    }
}