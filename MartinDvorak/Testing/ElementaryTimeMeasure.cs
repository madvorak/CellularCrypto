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
        private const int autSize = 5000;
        private const int stepsCount = 10000;
        private const byte elCode = 110;

        public static void RunTest()
        {
            Console.WriteLine();
            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();
            ElementaryAutomaton elAut = new ElementaryAutomaton(elCode, autSize);
            elAut.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)elAut).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryAutomaton: {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            ElementaryAutomatonFast elFast = new ElementaryAutomatonFast(elCode, autSize);
            elFast.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)elFast).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryFastAutomaton: {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            ElementaryAutomatonFaster elFaster = new ElementaryAutomatonFaster(elCode, autSize);
            elFaster.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)elFaster).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryFasterAutomaton: {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            ElementaryAutomatonFasterParallel elParallel = new ElementaryAutomatonFasterParallel(elCode, autSize);
            elParallel.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)elParallel).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryAutomatonFastParallel: {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            BinaryRangeAutomaton range = new ElementaryAutomaton(elCode, autSize).ConvertToRangeN();
            range.Step(stepsCount);
            stopwatch.Stop();
            Console.WriteLine(((IBinaryCA)range).StateAsString().Substring(500, 50));
            Console.WriteLine("BinaryRangeAutomaton: {0} ms\n", stopwatch.ElapsedMilliseconds);

            RunTest(); // TODO delete recursion
        }
    }
}
