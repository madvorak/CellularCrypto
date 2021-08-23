using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing any binary 1D automaton with symmetric scope.
    /// The automaton is cyclic - its edges are connected. Therefore, all positions are equivalent.
    /// </summary>
    class BinaryRangeCyclicAutomaton : BinaryRangeAutomaton
    {
        /// <summary>
        /// Creates a new cyclic CA of a general rule with 000...00100...000 as its initial state.
        /// </summary>
        /// <param name="scope">How many cells on each side from the center determine the next state of the cell.
        /// Value 1 makes it equivalent to a <c>ElementaryAutomaton</c> which has rule of size 8.
        /// Value 2 means that each new state of any cell depends on five total cells => size of rule must be 32.</param>
        /// <param name="rule">Array representing the rule for creating a new state.
        /// rule[0] is rule for 0..0, therefore opposite order to the rules of basic automata.</param>
        /// <param name="size">The size of the new CA.</param>
        public BinaryRangeCyclicAutomaton(byte scope, bool[] rule, int size) : base(scope, rule, size) { }

        /// <summary>
        /// Creates a new cyclic CA of a general rule with defined inital state.
        /// </summary>
        /// <param name="scope">How many cells on each side from the center determine the next state of the cell.
        /// Value 1 makes it equivalent to a <c>ElementaryAutomaton</c> which has rule of size 8.
        /// Value 2 means that each new state of any cell depends on five total cells => size of rule must be 32.</param>
        /// <param name="rule">Array representing the rule for creating a new state.
        /// rule[0] is rule for 0..0, therefore opposite order to the rules of basic automata.</param>
        /// <param name="initialState">A <c>BitArray</c> describing the initial state of the CA.
        /// This also determines the size of the new CA.</param>
        public BinaryRangeCyclicAutomaton(byte scope, bool[] rule, BitArray initialState) : base(scope, rule, initialState) { }

        protected override bool getValueAt(int index)
        {
            if (index < 0)
            {
                return state[index + size];
            }
            else
            {
                return state[index % size];
            }
        }

        public override object Clone()
        {
            return new BinaryRangeCyclicAutomaton(range, rule, state);
        }

        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            return new BinaryRangeCyclicAutomaton(range, rule, newInstanceState);
        }

        public override string TellType()
        {
            return "Cyclic " + base.TellType();
        }

        public ReversibleAutomaton ConvertToReversible(BitArray previousState)
        {
            return new ReversibleAutomaton(range, rule, previousState, state);
        }
    }
}
