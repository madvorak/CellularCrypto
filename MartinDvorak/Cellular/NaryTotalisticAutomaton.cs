using System;

namespace Cellular
{
    /// <summary>
    /// Class representing any n-ary one-dimensional automaton with a totalistic rule.
    /// </summary>
    class NaryTotalisticAutomaton : Nary1DAutomaton
    {
        protected int[] rule; //values (0 to N-1) for indeces from 0 to 3*(N-1) => size is 3*N - 2

        public NaryTotalisticAutomaton(int numberOfStates, int[] rule, int[] initialState) : base(numberOfStates, initialState)
        {
            this.rule = rule;
        }

        public override object Clone()
        {
            return new NaryTotalisticAutomaton(N, rule, state);
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
