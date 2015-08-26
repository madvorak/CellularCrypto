using System.Collections;

namespace Cellular
{
    class BinaryAutomatonCyclicRangeN : BinaryAutomatonRangeN
    {
        public BinaryAutomatonCyclicRangeN(byte scope, bool[] rule, int size) : base(scope, rule, size) { }

        public BinaryAutomatonCyclicRangeN(byte scope, bool[] rule, BitArray initialState) : base(scope, rule, initialState) { }

        protected override bool ValueAt(int index)
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
