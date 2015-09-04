namespace Cellular
{
    /// <summary>
    /// Class representing any N-ary one-dimensional automaton with a totalistic rule.
    /// </summary>
    class NaryTotalisticCyclicAutomaton : NaryTotalisticAutomaton
    {
        public NaryTotalisticCyclicAutomaton(int numberOfStates, int[] rule, int[] initialState) : base(numberOfStates, rule, initialState) {}

        protected override int getValueAt(int index)
        {
            if (index < 0)
            {
                return state[index + size];
            }
            return state[index % size];
        }

        public override string TellType()
        {
            return "Totalistic " + N + "-ary cyclic automaton";
        }
    }
}
