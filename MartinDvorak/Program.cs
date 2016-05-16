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
        //Crypto.Export.GetEncrypterStreamCA().Encrypt(new System.IO.FileStream(@"c:\Martin\stuff\John_Beak_Sigh.png", System.IO.FileMode.Open),
        //    new System.IO.FileStream(@"c:\Martin\stuff\John_Beak.cry", System.IO.FileMode.CreateNew), "heslo");
        rnd = new Random();
        Implementation2Test.RunTest();
        ElementaryTimeMeasure.RunTest();
        MainTests.RunTest();
        GeneticTest.RunTest();
        Adapter.DisplayForm();
        FactoryTest.RunTest();
        AutomataTest.RunTest();
        FunctionTestTest.RunTest();
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