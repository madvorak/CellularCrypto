using System;
using System.Text;

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

        public override void Step()
        {
            int[] newState = new int[size];
            for (int i = 0; i < size; i++)
            {
                newState[i] = rule[getValueAt(i - 1) + getValueAt(i) + getValueAt(i + 1)];
            }
            state = newState;
        }

        public override object Clone()
        {
            return new NaryTotalisticAutomaton(N, rule, state);
        }

        public override string TellType()
        {
            return "Totalistic " + N + "-ary automaton";
        }

        public string PrintTernary()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                switch (state[i])
                {
                    case 0: sb.Append(' '); break;
                    case 1: sb.Append('▒'); break;
                    default: sb.Append('█'); break;
                }
            }
            return sb.ToString();
        }

        public string PrintQuinary()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                switch (state[i])
                {
                    case 0: sb.Append(' '); break;
                    case 1: sb.Append('░'); break;
                    case 2: sb.Append('▒'); break;
                    case 3: sb.Append('▒'); break;
                    default: sb.Append('█'); break;
                }
            }
            return sb.ToString();
        }
    }
}
