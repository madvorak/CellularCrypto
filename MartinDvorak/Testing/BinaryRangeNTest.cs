using System;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration that <c>BinaryAutomatonRangeN</c> can be used in a place of <c>BasicAutomaton</c>.
    /// </summary>
    static class BinaryRangeNTest
    {
        public static void RunTest()
        {
            BasicAutomaton basic = new BasicAutomaton(90, 50);
            BinaryAutomatonRangeN range = basic.ConvertToRangeN();
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine((range as BinaryCA).StateAsString());
                range.Step();
            }

            for (byte i = 0; i < 255; i++)
            {
                basic = new BasicAutomaton(i, 200);
                range = basic.ConvertToRangeN();
                basic.Step(100);
                range.Step(100);
                if ((basic as BinaryCA).GetValueAt(0) != (range as BinaryCA).GetValueAt(0)
                    || (basic as BinaryCA).GetValueAt(1) != (range as BinaryCA).GetValueAt(1)
                    || (basic as BinaryCA).GetValueAt(100) != (range as BinaryCA).GetValueAt(100)
                    || (basic as BinaryCA).GetValueAt(129) != (range as BinaryCA).GetValueAt(129)
                    || (basic as BinaryCA).GetValueAt(190) != (range as BinaryCA).GetValueAt(190))
                {
                    Console.WriteLine("Error of N-ary automaton corresponding to the rule " + i);
                }
            }
            Console.WriteLine("BinaryRangeNTest completed.");
        }
    }
}
