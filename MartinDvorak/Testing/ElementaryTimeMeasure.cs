using System;
using System.Diagnostics;
using Cellular;

namespace Testing
{
    static class ElementaryTimeMeasure
    {
        private const int autSize = 1005;
        private const int stepsCount = 9000;

        public static void RunTest()
        {
            Console.WriteLine();
            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();
            ElementaryAutomaton elAut = new ElementaryAutomaton(autSize);
            elAut.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)elAut).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryAutomaton: {0} ms", stopwatch.ElapsedMilliseconds); // was 318

            stopwatch.Reset();
            stopwatch.Start();
            ElementaryFastAutomaton elFast = new ElementaryFastAutomaton(autSize);
            elFast.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)elFast).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryFastAutomaton: {0} ms", stopwatch.ElapsedMilliseconds); // was 199

            stopwatch.Reset();
            stopwatch.Start();
            BinaryRangeAutomaton range = new ElementaryAutomaton(autSize).ConvertToRangeN(); // was 350
            range.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)range).StateAsString().Substring(500, 50));
            Console.WriteLine("BinaryRangeAutomaton: {0} ms\n", stopwatch.ElapsedMilliseconds);
        }
    }
}
