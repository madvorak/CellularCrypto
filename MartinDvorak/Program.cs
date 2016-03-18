using System;
using Testing;
using UserForms;

class Program
{
    public static Random rnd;

    static void Main(string[] args)
    {
        rnd = new Random();
        GeneticTest.RunTest();
        FunctionTestTest.RunTest();
        //Crypto.SearchSGA.SearchForGoodExtenders();
        AutomataTest.RunTest();
        UtilityTest.RunTest();
        RandomTestTest.RunTest();
        Adapter.DisplayForm();
        TotalisticTernaryTest.RunTest();
        BinaryRangeNTest.RunTest();
        Implementation2Test.RunTest();
        ElementaryTimeMeasure.RunTest();
        SearchLongest.LongestPeriod();

        Console.WriteLine("\n________ THE END _______");
        Console.ReadLine();
    }
}