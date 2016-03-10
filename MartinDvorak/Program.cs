using System;
using Testing;
using UserForms;

class Program
{
    public static Random rnd;

    static void Main(string[] args)
    {
        rnd = new Random();
        FunctionTestTest.RunTest();
        UtilityTest.RunTest();
        Adapter.DisplayForm();
        AutomataTest.RunTest();
        TotalisticTernaryTest.RunTest();
        RandomTestTest.RunTest();
        BinaryRangeNTest.RunTest();
        Implementation2Test.RunTest();
        ElementaryTimeMeasure.RunTest();
        SearchLongest.LongestPeriod();

        Console.WriteLine("\n________ THE END _______");
        Console.ReadLine();
    }
}