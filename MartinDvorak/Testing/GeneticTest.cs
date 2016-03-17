using System;
using System.Collections;
using Cellular;
using Crypto;

namespace Testing
{
    static class GeneticTest
    {
        public static void RunTest()
        {
            KeyExtenderGenetic sga = new KeyExtenderGenetic();
            BitArray res = sga.ExtendKey(Utilities.RandomBitArr(20), 1000);
            Console.WriteLine(RandomnessTesting.RateSequence(res));
            Console.WriteLine("Genetic algorithm OK\n");
        }
    }
}
