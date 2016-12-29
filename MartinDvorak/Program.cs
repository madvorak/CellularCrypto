using System;
using Testing;
using UserForms;

class Program
{
    public static Random rnd;

    /// <summary>
    /// Entry point of the experimental program. Feel free to change the order of actions, only initialize the RNG first of all.
    /// </summary>
    /// <param name="args">No arguments are used.</param>
    static void Main(string[] args)
    {
        rnd = new Random();
        MainTests.RunTest();
        GeneticTest.RunTest();
        ElementaryImplementationsTest.RunTest();
        ElementaryTimeMeasure.RunTest();
        Adapter.DisplayForm();
        FactoryTest.RunTest();
        FunctionTestTest.RunTest();
        AutomataTest.RunTest();
        ReversibleTest.RunTest();
        TotalisticTernaryTest.RunTest();
        UtilityTest.RunTest();
        RandomTestTest.RunTest();
        BinaryRangeNTest.RunTest();
        SearchLongest.LongestPeriod();

        Console.WriteLine("\n________ THE END _______");
        Console.ReadLine();
    }
}