using System.Text;

namespace Cellular
{
    /// <summary>
    /// Class representing any n-ary one-dimensional automaton with a totalistic rule - bordered variant. 
    /// The new state of each cell depends on the sum of its current state and the current state of adjecent cells.
    /// </summary>
    class NaryTotalisticAutomaton : Nary1DAutomaton
    {
        protected int[] rule;

        /// <summary>
        /// Creates a new totalistic N-ary automaton with given rule and initial state.
        /// </summary>
        /// <param name="numberOfStates">The N.</param>
        /// <param name="rule">Array representing the rule for creating a new state.
        /// Value in rule[0] determines what happens to a white (0/dead) cell with white immediate neighbours.
        /// Value in rule[3*(N-1)] determines what happens to a black (maxVal) cell with black immediate neighbours.</param>
        /// <param name="initialState">An integer array describing the initial state of the CA.
        /// This also determines the size of the new CA.</param>
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
            time++;
        }

        public override object Clone()
        {
            return new NaryTotalisticAutomaton(N, rule, state);
        }

        public override string TellType()
        {
            return "Totalistic " + N + "-ary automaton";
        }

        /// <summary>
        /// Converts the inner state into a well-printable string. This method works best for ternary (N=3) automata.
        /// </summary>
        /// <returns>String consisting of different shades of rectangles. All values 2+ are represented by a full rectangle.</returns>
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

        /// <summary>
        /// Converts the inner state into a well-printable string. This method works best for quinary (N=5) automata.
        /// </summary>
        /// <returns>String consisting of different shades of rectangles. All values 4+ are represented by a full rectangle.</returns>
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
