using System;
using System.IO;
using Cellular;
using Crypto;

namespace Testing
{
    /// <summary>
    /// The most important tests (rating of key extenders) for my thesis.
    /// </summary>
    static class MainTests
    {
        /// <summary>
        /// Runs the most important tests for my thesis.
        /// </summary>
        public static void RunTest()
        {
            FunctionTestsForThesis evaluator = new FunctionTestsForThesis(new FunctionTesting(false));
            
            Console.WriteLine("KeyExtenderCopy:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderCopy()));

            Console.WriteLine("KeyExtenderCheating:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderCheating()));

            IBinaryCA automaton;
            try
            {
                automaton = Factory.CreateAutomaton(File.ReadAllText("ca.txt"));
            }
            catch
            {
                automaton = new ElementaryAutomaton();
            }

            Console.WriteLine($"KeyExtenderSimpleLinear using {automaton.TellType()}:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderSimpleLinear(automaton)));

            const int rows = 10;
            const int skips = 0;
            Console.WriteLine($"KeyExtenderInterlaced({rows}, {skips}) using {automaton.TellType()}:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderInterlaced(automaton, rows, skips)));

            Console.WriteLine($"KeyExtenderUncertain using {automaton.TellType()}:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderUncertain(automaton)));

            Console.WriteLine($"KeyExtenderSimpleQuadratic using {automaton.TellType()}:");
            Console.WriteLine(evaluator.CalculateAndPrintResults(new KeyExtenderSimpleQuadratic(automaton)));

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
