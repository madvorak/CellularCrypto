using System;
using Cellular;
using Crypto;

namespace Testing
{
    static class MainTests
    {
        public static void RunTest()
        {
            FunctionTestsForThesis evaluator = new FunctionTestsForThesis(new FunctionTesting());

            Console.WriteLine("KeyExtenderCopy:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderCopy()));

            Console.WriteLine("KeyExtenderCheating:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderCheating()));

            const int ruleNo = 94;
            IBinaryCA automaton = new ElementaryFastAutomaton(ruleNo, 1);

            Console.WriteLine($"KeyExtenderSimpleLinear using rule No. {ruleNo}:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderSimpleLinear(automaton)));

            const int rows = 10;
            const int skips = 0;
            Console.WriteLine($"KeyExtenderInterlaced({rows}, {skips}) using rule No. {ruleNo}:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderInterlaced(automaton, rows, skips)));

            Console.WriteLine($"KeyExtenderUncertain using rule No. {ruleNo}:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderUncertain(automaton)));

            Console.WriteLine($"KeyExtenderSimpleQuadratic using rule No. {ruleNo}:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderSimpleQuadratic(automaton)));

            /*automaton = new GameOfLife(1, 1);

            Console.WriteLine("KeyExtenderSimpleLinear using Game of Life:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderSimpleLinear(automaton)));

            Console.WriteLine($"KeyExtenderInterlaced({rows}, {skips}) using Game of Life:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderInterlaced(automaton, rows, skips)));

            Console.WriteLine("KeyExtenderUncertain using using Game of Life:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderUncertain(automaton)));

            Console.WriteLine("KeyExtenderSimpleQuadratic using rule Game of Life:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderSimpleQuadratic(automaton)));*/

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
