namespace Cellular
{
    class TotalisticNaryAutomaton : Nary1DAutomaton
    {
        int[] rule;

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
            throw new System.NotImplementedException();
        }
    }
}
