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
            BitArray input = Utilities.RandomBitArr(8);
            const int length = 500;
            IBinaryCA aut = new ElementaryFastAutomaton();
            double result;
            double bestRes;

            BitArray simpleRes = new KeyExtenderSimpleLinear(aut).ExtendKey(input, length);
            result = RandomnessTesting.RateSequence(simpleRes);
            Console.WriteLine("KeyExtenderSimpleLinear: {0}\n", result);
            bestRes = result;

            for (int i = 2; i < 15; i += 6)
            {
                for (int j = 0; j < 10; j += 4)
                {
                    BitArray interlRes = new KeyExtenderInterlaced(aut, i, j).ExtendKey(input, length);
                    result = RandomnessTesting.RateSequence(interlRes);
                    Console.WriteLine($"KeyExtenderInterlaced({i}, {j}): {result}");
                    if (result > bestRes)
                    {
                        bestRes = result;
                    }
                }
            }
            Console.WriteLine();

            KeyExtenderGenetic sga = new KeyExtenderGenetic(false);
            BitArray geneticRes = sga.ExtendKey(input, length);
            result = RandomnessTesting.RateSequence(geneticRes);
            Console.WriteLine("KeyExtenderGenetic with naive primitives: {0}", result);
            Console.WriteLine("***   change:    {0:0.0000}", result - bestRes);
            Console.WriteLine(sga.getInfoAboutWinner());
            Console.WriteLine();

            sga = new KeyExtenderGenetic(true);
            geneticRes = sga.ExtendKey(input, length);
            result = RandomnessTesting.RateSequence(geneticRes);
            Console.WriteLine("KeyExtenderGenetic with pre-generated primitives: {0}", result);
            Console.WriteLine("***   change:    {0:0.0000}", result - bestRes);
            Console.WriteLine(sga.getInfoAboutWinner());
            Console.WriteLine();

            BitArray quadratRes = new KeyExtenderSimpleQuadratic(aut).ExtendKey(input, length);
            Console.WriteLine("KeyExtenderSimpleQuadratic: {0}\n", RandomnessTesting.RateSequence(quadratRes));
            Console.ReadKey();
        }
    }
}
