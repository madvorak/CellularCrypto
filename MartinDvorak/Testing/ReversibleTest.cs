using System;
using System.Collections;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration that <c>ReversibleAutomaton</c> is really reversible.
    /// </summary>
    static class ReversibleTest
    {
        public static void RunTest()
        {
            const int size = 40;
            const int ruleNo = 60;
            ReversibleAutomaton revers = (new ElementaryAutomaton(ruleNo, size)).ConvertToCyclicN()
                .ConvertToReversible(new BitArray(size, false));

            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine((revers as IBinaryCA).StateAsString());
                revers.Step();
            }

            Console.WriteLine("\n-------------------------------------------\nReverse:");

            BitArray stateA = new BitArray(size, false);
            // next to last state of the previous run
            stateA[20] = stateA[28] = stateA[38] = true;
            BitArray stateB = new BitArray(size, false);
            // the very last state of the previous run
            for (int i = 36; i < 40; i++) stateB[i] = true;
            revers = (new ElementaryAutomaton(ruleNo, stateB)).ConvertToCyclicN().ConvertToReversible(stateA);

            for (int i = 0; i < 80; i++)
            {
                Console.WriteLine((revers as IBinaryCA).StateAsString());
                revers.Step();
            }

            Console.WriteLine();
            Console.ReadKey();
        }
    }   
}

/*
                                    ████
                    █       █         █
                    */