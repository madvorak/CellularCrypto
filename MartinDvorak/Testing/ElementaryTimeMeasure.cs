using System;
using System.Collections;
using System.Diagnostics;
using Cellular;

namespace Testing
{
    /// <summary>
    /// Simple test of how different classes are (in)efficient for implementing elementary CA rules. 
    /// </summary>
    static class ElementaryTimeMeasure
    {
        private const int autSize = 1 << 24;
        private const int stepsCount = 10;
        private const byte elCode = 30;

        public static void RunTest()
        {
            Console.WriteLine();
            BitArray initial = Utilities.RandomBitArr(autSize);
            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();
            ElementaryAutomaton elAut = new ElementaryAutomaton(elCode, initial);
            elAut.Step(stepsCount);
            stopwatch.Stop();
            //Console.WriteLine(((IBinaryCA)elAut).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryAutomaton: {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            ElementaryAutomatonFast elFast = new ElementaryAutomatonFast(elCode, initial);
            elFast.Step(stepsCount);
            stopwatch.Stop();
            //Console.WriteLine(((IBinaryCA)elFast).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryAutomatonFast: {0} ms", stopwatch.ElapsedMilliseconds);
            
            stopwatch.Reset();
            stopwatch.Start();
            ElementaryAutomatonFaster elFaster = new ElementaryAutomatonFaster(elCode, initial);
            elFaster.Step(stepsCount);
            stopwatch.Stop();
            //Console.WriteLine(((IBinaryCA)elFaster).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryAutomatonFaster: {0} ms", stopwatch.ElapsedMilliseconds);
            
            stopwatch.Reset();
            stopwatch.Start();
            ElementaryAutomatonParallel elParallel = new ElementaryAutomatonParallel(elCode, initial);
            elParallel.Step(stepsCount);
            stopwatch.Stop();
            //Console.WriteLine(((IBinaryCA)elParallel).StateAsString().Substring(500, 50));
            Console.WriteLine("ElementaryAutomatonParallel: {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            BinaryRangeAutomaton range = new ElementaryAutomaton(elCode, initial).ConvertToRangeN();
            range.Step(stepsCount);
            stopwatch.Stop();
            //Console.WriteLine(((IBinaryCA)range).StateAsString().Substring(500, 50));
            Console.WriteLine("BinaryRangeAutomaton: {0} ms\n", stopwatch.ElapsedMilliseconds);

            Console.ReadKey();
        }
    }
}
