using System;
using Cellular;
using Crypto;

namespace Testing
{
    /// <summary>
    /// Simple demonstration of utilities for CA.
    /// </summary>
    static class FunctionTestTest
    {
        private static FunctionTesting test = new FunctionTesting(false);

        public static void RunTest()
        {
            Console.WriteLine("KeyExtenderCopy:");
            runTestsOneAlgorithm(new KeyExtenderCopy());

            Console.WriteLine("KeyExtenderCheating:");
            runTestsOneAlgorithm(new KeyExtenderCheating());

            IBinaryCA automaton = new ElementaryFastAutomaton(30, 1);

            Console.WriteLine("KeyExtenderSimpleLinear:");
            runTestsOneAlgorithm(new KeyExtenderSimpleLinear(automaton));

            const int rows = 10;
            const int skips = 15;
            Console.WriteLine($"KeyExtenderInterlaced({rows}, {skips}):");
            runTestsOneAlgorithm(new KeyExtenderInterlaced(automaton, rows, skips));

            Console.WriteLine("KeyExtenderSimpleQuadratic:");
            runTestsOneAlgorithm(new KeyExtenderSimpleQuadratic(automaton));

            Console.WriteLine();
            Console.ReadKey();
        }

        private static void runTestsOneAlgorithm(IKeyExtender algorithm)
        {
            const int ratio = 7;
            Console.WriteLine(" TestBitChange: " + test.TestBitChange(algorithm, ratio));
            Console.WriteLine(" TestAverageDistance: " + test.TestAverageDistance(algorithm, ratio));
            Console.WriteLine(" TestLargestBallExactly: " + test.TestLargestBallExactly(algorithm));
            Console.WriteLine(" TestLargestBallApprox: " + test.TestLargestBallApprox(algorithm));
            Console.WriteLine(" TestRandomSequences: " + test.TestRandomSequences(algorithm, 10*ratio));
            Console.WriteLine(" TestSystematicSequences: " + test.TestSystematicSequences(algorithm, 1000));
            Console.WriteLine();
        }
    }
}
