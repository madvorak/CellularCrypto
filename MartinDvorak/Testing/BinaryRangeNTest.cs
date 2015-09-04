using System;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple demonstration that <c>BinaryRangeAutomaton</c> can be used in a place of <c>ElementaryAutomaton</c>.
    /// </summary>
    static class BinaryRangeNTest
    {
        public static void RunTest()
        {
            ElementaryAutomaton basic = new ElementaryAutomaton(90, 50);
            BinaryRangeAutomaton range = basic.ConvertToRangeN();
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine((range as IBinaryCA).StateAsString());
                range.Step();
            }

            for (byte i = 0; i < 255; i++)
            {
                basic = new ElementaryAutomaton(i, 200);
                range = basic.ConvertToRangeN();
                basic.Step(100);
                range.Step(100);
                if ((basic as IBinaryCA).GetValueAt(0) != (range as IBinaryCA).GetValueAt(0)
                    || (basic as IBinaryCA).GetValueAt(1) != (range as IBinaryCA).GetValueAt(1)
                    || (basic as IBinaryCA).GetValueAt(100) != (range as IBinaryCA).GetValueAt(100)
                    || (basic as IBinaryCA).GetValueAt(129) != (range as IBinaryCA).GetValueAt(129)
                    || (basic as IBinaryCA).GetValueAt(190) != (range as IBinaryCA).GetValueAt(190))
                {
                    Console.WriteLine("Error of N-ary automaton corresponding to the rule " + i);
                }
            }
            Console.WriteLine("BinaryRangeNTest completed.");
        }
    }
}
