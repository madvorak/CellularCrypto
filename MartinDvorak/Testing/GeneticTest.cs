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
            BitArray input = Utilities.RandomBitArr(16);
            const int length = 1000;
            IBinaryCA aut = new ElementaryFastAutomaton();

            BitArray simpleRes = new KeyExtenderSimpleLinear(aut).ExtendKey(input, length);
            Console.WriteLine("KeyExtenderSimpleLinear: {0}\n", RandomnessTesting.RateSequence(simpleRes));

            for (int i = 2; i < 15; i += 6)
            {
                for (int j = 0; j < 10; j += 4)
                {
                    BitArray interlRes = new KeyExtenderInterlaced(aut, i, j).ExtendKey(input, length);
                    Console.WriteLine($"KeyExtenderInterlaced({i}, {j}): {RandomnessTesting.RateSequence(interlRes)}");
                }
            }
            Console.WriteLine();

            KeyExtenderGenetic sga = new KeyExtenderGenetic();
            BitArray geneticRes = sga.ExtendKey(input, length);
            Console.WriteLine("KeyExtenderGenetic: {0}", RandomnessTesting.RateSequence(geneticRes));
            Console.WriteLine(sga.getInfoAboutWinner());
            Console.WriteLine();

            BitArray quadratRes = new KeyExtenderSimpleQuadratic(aut).ExtendKey(input, length);
            Console.WriteLine("KeyExtenderSimpleQuadratic: {0}\n", RandomnessTesting.RateSequence(quadratRes));
            Console.ReadKey();
        }
    }
}
