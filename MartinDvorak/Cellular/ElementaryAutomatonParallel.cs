using System;
using System.Collections;
using System.Threading;

namespace Cellular
{
    /// <summary>
    /// Class representing 256 elementary CA with firmly set borders.
    /// Parallel implementation. Unfortunately, it seems this is not the fastest implementation.
    /// For small sizes, the threading overhead is too big.
    /// For large sizes, there seems to be a performce problem with parallel memory access.
    /// </summary>
    class ElementaryAutomatonParallel : ElementaryAutomatonFaster
    {
        private int threadCount;

        /// <summary>
        /// Creates a new basic CA with given rule and initial state (parallel variant).
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="initialState">A <c>BitArray</c> describing the initial state of the CA.
        /// This also determines the size of the new CA.</param>
        public ElementaryAutomatonParallel(byte ruleNo, BitArray initialState) : base(ruleNo, initialState)
        {
            countProcessors();
        }

        /// <summary>
        /// Creates a new basic CA with given rule and 000...00100...000 as its initial state (parallel variant).
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="size">The size of the new CA.</param>
        public ElementaryAutomatonParallel(byte ruleNo, int size) : base(ruleNo, size)
        {
            countProcessors();
        }

        /// <summary>
        /// Decides how many threads should be used for <c>Step()</c> according to the number of processors.
        /// </summary>
        private void countProcessors()
        {
            threadCount = (Environment.ProcessorCount + 1) / 2;
        }

        /// <summary>
        /// A job for one thread.
        /// </summary>
        private class Job
        {
            private readonly int startIndex;
            private readonly int count;
            private readonly bool[] rule;
            private readonly BitArray sourceState;
            private BitArray targetState;

            public Job(int startAtIndex, int countToDo, bool[] flatRule, BitArray oldState, BitArray newState)
            {
                startIndex = startAtIndex;
                count = countToDo;
                rule = new bool[8];
                flatRule.CopyTo(rule, 0);
                sourceState = oldState;
                targetState = newState;
            }

            /// <summary>
            /// Performs Step() on a part of the automaton.
            /// </summary>
            public void Run()
            {
                int foo = 0;
                if (startIndex > 0 && sourceState[startIndex - 1])
                {
                    foo = 2;
                }
                if (sourceState[startIndex])
                {
                    foo++;
                }
                int endIndex = startIndex + count - (startIndex + count == targetState.Count ? 1 : 0);
                for (int i = startIndex; i < endIndex; i++)
                {
                    foo = (foo << 1) & 7;
                    if (sourceState[i + 1])
                    {
                        foo++;
                    }
                    targetState[i] = rule[foo];
                }
                if (startIndex + count == targetState.Count)
                {
                    targetState[startIndex + count - 1] = rule[(foo << 1) & 7];
                }
            }
        }

        public override void Step()
        {
            if (size < 300)
            {
                base.Step();
            }
            else
            {
                BitArray newState = new BitArray(size);
                // the sice of chunks must be a multiple of 32 -> then we use BitArray in a thread-safe way
                int chunk = ((size / threadCount) >> 5) << 5;
                int extra = size - (threadCount * chunk);
                Thread[] threads = new Thread[threadCount - 1];
                for (int i = 0; i < threadCount - 1; i++)
                {
                    Job job = new Job(chunk * i, chunk, rule1D, state, newState);
                    threads[i] = new Thread(job.Run);
                    threads[i].Start();
                }
                Job oneJob = new Job(chunk * (threadCount - 1), chunk + extra, rule1D, state, newState);
#if (perf)
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
#endif
                oneJob.Run();
#if (perf)
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
                sw.Restart();
#endif
                for (int i = 0; i < threadCount - 1; i++)
                {
                    threads[i].Join();
#if (perf)
                    sw.Stop();
                    Console.WriteLine(sw.ElapsedMilliseconds);
                    sw.Restart();
#endif
                }

                state = newState;
                time++;
            }
        }

        public override object Clone()
        {
            return new ElementaryAutomatonParallel(ruleNumber, state);
        }

        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            return new ElementaryAutomatonParallel(ruleNumber, newInstanceState);
        }
    }
}
