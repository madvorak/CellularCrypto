using System;
using System.Diagnostics;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple test of how different classes are (in)efficient for implementing elementary CA rules. 
    /// </summary>
    static class ElementaryTimeMeasure
    {
        private const int autSize = 1005;
        private const int stepsCount = 9000;
        private const byte elCode = 35;

        public static void RunTest()
        {
            Console.WriteLine();
            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();
            ElementaryAutomaton elAut = new ElementaryAutomaton(elCode, autSize);
            elAut.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)elAut).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryAutomaton: {0} ms", stopwatch.ElapsedMilliseconds); // 319 ms

            stopwatch.Reset();
            stopwatch.Start();
            ElementaryFastAutomaton elFast = new ElementaryFastAutomaton(elCode, autSize);
            elFast.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)elFast).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryFastAutomaton: {0} ms", stopwatch.ElapsedMilliseconds); // was 199 ms, now 152 ms

            stopwatch.Reset();
            stopwatch.Start();
            BinaryRangeAutomaton range = new ElementaryAutomaton(elCode, autSize).ConvertToRangeN(); // 350 ms
            range.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)range).StateAsString().Substring(500, 50));
            Console.WriteLine("BinaryRangeAutomaton: {0} ms\n", stopwatch.ElapsedMilliseconds);
        }
    }
}
