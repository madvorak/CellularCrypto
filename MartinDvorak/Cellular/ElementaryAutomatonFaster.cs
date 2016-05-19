using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing 256 elementary CA with firmly set borders.
    /// This specific implementation reads every state only once, so it should be even faster.
    /// </summary>
    class ElementaryAutomatonFaster : ElementaryAutomaton
    {
        protected bool[] rule1D;

        /// <summary>
        /// Creates a new basic CA with given rule and initial state (even faster variant).
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="initialState">A <c>BitArray</c> describing the initial state of the CA.
        /// This also determines the size of the new CA.</param>
        public ElementaryAutomatonFaster(byte ruleNo, BitArray initialState) : base(ruleNo, initialState)
        {
            createRule1D();
        }

        /// <summary>
        /// Creates a new basic CA with given rule and 000...00100...000 as its initial state (even faster variant).
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="size">The size of the new CA.</param>
        public ElementaryAutomatonFaster(byte ruleNo, int size) : base(ruleNo, size)
        {
            createRule1D();
        }

        private void createRule1D()
        {
            rule1D = new bool[8];
            byte number = ruleNumber;
            for (int i = 0; i < 8; i++)
            {
                rule1D[i] = number % 2 == 1 ? true : false;
                number /= 2;
            }
        }

        public override void Step()
        {
            BitArray newState = new BitArray(size);
#if (perf)
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            int foo = state[0] ? 1 : 0;
            for (int i = 0; i < size - 1; i++)
            {
                foo = (foo << 1) & 7;
                if (state[i + 1]) foo++;
                newState[i] = rule1D[foo];
            }
            newState[size - 1] = rule1D[(foo << 1) & 7];
#if (perf)
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);
#endif

            state = newState;
            time++;
        }

        public override object Clone()
        {
            return new ElementaryAutomatonFaster(ruleNumber, state);
        }

        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            return new ElementaryAutomatonFaster(ruleNumber, newInstanceState);
        }
    }
}
