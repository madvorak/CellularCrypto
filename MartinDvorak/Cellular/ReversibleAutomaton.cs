using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing any binary 1D automaton with any symmetric rule which is made reversible.
    /// This is very inefficient (possibly throwaway) implementation.
    /// </summary>
    class ReversibleAutomaton : Binary1DAutomaton
    {
        private byte range;
        private bool[] rule;
        private BitArray prevState;

        public ReversibleAutomaton(byte range, bool[] rule, BitArray previousState, BitArray currentState) 
            : base(currentState)
        {
            this.range = range;
            this.rule = rule;
            prevState = previousState;
            state = currentState;
        }

        public override void Step()
        {
            IBinaryCA helpCA = new BinaryRangeCyclicAutomaton(range, rule, state);
            helpCA.Step();
            BitArray newState = new BitArray(state.Length);
            for (int i = 0; i < state.Length; i++)
            {
                newState[i] = helpCA.GetValueAt(i) ^ prevState[i];
            }
            prevState = state;
            state = newState;
        }

        public override string TellType()
        {
            return "Binary 1D reversible automaton with scope " + range;
        }

        public override object Clone()
        {
            return new ReversibleAutomaton(range, rule, prevState, state);
        }

        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            return new ReversibleAutomaton(range, rule, state, newInstanceState);
        }
    }
}
