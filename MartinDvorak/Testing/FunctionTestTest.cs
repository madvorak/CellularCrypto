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
        public static void RunTest()
        {
            Console.WriteLine("KeyExtenderCopy:");
            runTestsOneAlgorithm(new KeyExtenderCopy());

            Console.WriteLine("KeyExtenderCheating:");
            runTestsOneAlgorithm(new KeyExtenderCheating());

            IBinaryCA automaton = new ElementaryFastAutomaton(30, 1);

            Console.WriteLine("KeyExtenderSimpleLinear:");
            runTestsOneAlgorithm(new KeyExtenderSimpleLinear(automaton));

            Console.WriteLine("KeyExtenderSimpleQuadratic:");
            runTestsOneAlgorithm(new KeyExtenderSimpleQuadratic(automaton));

            Console.WriteLine();
            Console.ReadKey();
        }

        private static void runTestsOneAlgorithm(IKeyExtender algorithm)
        {
            const int ratio = 7;
            Console.WriteLine(" TestBitChange: " + FunctionTesting.TestBitChange(algorithm, ratio));
            Console.WriteLine(" TestAverageDistance: " + FunctionTesting.TestAverageDistance(algorithm, ratio));
            Console.WriteLine(" TestLargestBall: " + FunctionTesting.TestLargestBall(algorithm));
            Console.WriteLine(" TestRandomSequences: " + FunctionTesting.TestRandomSequences(algorithm, ratio));
            Console.WriteLine(" TestSystematicSequences: " + FunctionTesting.TestSystematicSequences(algorithm, ratio));
            Console.WriteLine();
        }
    }
}
