using System;
using Testing;
using UserForms;

class Program
{
    public static Random rnd;

    /// <summary>
    /// Entry point of the experimental program. Feel free to change the order of actions, only initialize the RNG first of all.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        rnd = new Random();
        FunctionTestTest.RunTest();
        GeneticTest.RunTest();
        Adapter.DisplayForm();
        //Crypto.SearchSGA.SearchForGoodExtenders();
        AutomataTest.RunTest();
        TotalisticTernaryTest.RunTest();
        UtilityTest.RunTest();
        RandomTestTest.RunTest();
        BinaryRangeNTest.RunTest();
        Implementation2Test.RunTest();
        ElementaryTimeMeasure.RunTest();
        SearchLongest.LongestPeriod();

        Console.WriteLine("\n________ THE END _______");
        Console.ReadLine();
    }
}