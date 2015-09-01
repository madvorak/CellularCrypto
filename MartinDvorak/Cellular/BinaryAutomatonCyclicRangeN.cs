using System.Collections;

namespace Cellular
{
    /// <summary>
    /// This class can work with any binary 1D automaton with symmetric scope.
    /// The automata are cyclic - edges are connected. Therefore, all positions are equivalent.
    /// </summary>
    class BinaryAutomatonCyclicRangeN : BinaryAutomatonRangeN
    {
        public BinaryAutomatonCyclicRangeN(byte scope, bool[] rule, int size) : base(scope, rule, size) { }

        public BinaryAutomatonCyclicRangeN(byte scope, bool[] rule, BitArray initialState) : base(scope, rule, initialState) { }

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

        public override string TellType()
        {
            return "Cyclic " + base.TellType();
        }
    }
}
