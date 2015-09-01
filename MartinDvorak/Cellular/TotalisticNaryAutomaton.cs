using System;

namespace Cellular
{
    /// <summary>
    /// Stub. Uses only (self, ) left neighbour and right neighbour.
    /// </summary>
    class TotalisticNaryAutomaton : Nary1DAutomaton
    {
        protected int[] rule; //values (0 to N-1) for indeces from 0 to 3*(N-1) => size is 3*N - 2

        public TotalisticNaryAutomaton(int numberOfStates, int[] rule, int[] initialState) : base(numberOfStates, initialState)
        {
            this.rule = rule;
        }

        public override object Clone()
        {
            return new TotalisticNaryAutomaton(N, rule, state);
        }

        public override string TellType()
        {
            return "Totalistic " + N + "-ary automaton";
        }

        public override void Step()
        {
            int[] newState = new int[size];
            for (int i = 0; i < size; i++)
            {
                newState[i] = rule[getValueAt(i - 1) + getValueAt(i) + getValueAt(i + 1)];
            }
            state = newState;
        }
    }
}
