namespace Cellular
{
    /// <summary>
    /// Stub. Cyclic variant.
    /// </summary>
    class TotalisticNaryCyclicAutomaton : TotalisticNaryAutomaton
    {
        public TotalisticNaryCyclicAutomaton(int numberOfStates, int[] rule, int[] initialState) : base(numberOfStates, rule, initialState) {}

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
