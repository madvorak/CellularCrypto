using System;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// This class can work with 256 elementary CA with firmly set borders.
    /// This specific implementation calculates every bit separately.
    /// </summary>
    class BasicAutomaton: Binary1DAutomaton
    {
        protected byte ruleNumber;
        protected bool[, ,] rule;

        /// <summary>
        /// Creates a new basic CA of size 100 with 000...00100...000 as its initial state.
        /// The new CA will use rule No.30 : asymmetric, pseudo-chaotic behaviour.
        /// </summary>
        public BasicAutomaton(): this(100) {}

        /// <summary>
        /// Creates a new basic CA with 000...00100...000 as its initial state.
        /// The new CA will use rule No.30 : asymmetric, pseudo-chaotic behaviour.
        /// </summary>
        /// <param name="size">The size of the new CA.</param>
        public BasicAutomaton(int size) : this(30, size) {}

        /// <summary>
        /// Creates a new basic CA with given rule and 000...00100...000 as its initial state.
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="size">The size of the new CA.</param>
        public BasicAutomaton(byte ruleNo, int size) : base(size)
        {
            ruleNumber = ruleNo;
            ruleFromNumber(ruleNo);
        }

        /// <summary>
        /// Creates a new basic CA with given rule and initial state.
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="initialState">A <c>BitArray</c> describing the initial state of the CA.
        /// This also determines the size of the new CA.</param>
        public BasicAutomaton(byte ruleNo, BitArray initialState) : base(initialState)
        {
            ruleNumber = ruleNo;
            ruleFromNumber(ruleNo);
        }

        /// <summary>
        /// Creates a new basic CA.
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="size">The size of the new CA.</param>
        /// <param name="rnd">PseudoRNG instance used to generate the original state.</param>
        public BasicAutomaton(byte ruleNo, int size, Random rnd) : base(size, rnd)
        {
            ruleNumber = ruleNo;
            ruleFromNumber(ruleNo);
        }

        private void ruleFromNumber(byte ruleNo)
        {
            rule = new bool[2, 2, 2];
            rule[0, 0, 0] = ruleNo % 2 == 1 ? true : false;
            ruleNo /= 2;
            rule[0, 0, 1] = ruleNo % 2 == 1 ? true : false;
            ruleNo /= 2;
            rule[0, 1, 0] = ruleNo % 2 == 1 ? true : false;
            ruleNo /= 2;
            rule[0, 1, 1] = ruleNo % 2 == 1 ? true : false;
            ruleNo /= 2;
            rule[1, 0, 0] = ruleNo % 2 == 1 ? true : false;
            ruleNo /= 2;
            rule[1, 0, 1] = ruleNo % 2 == 1 ? true : false;
            ruleNo /= 2;
            rule[1, 1, 0] = ruleNo % 2 == 1 ? true : false;
            ruleNo /= 2;
            rule[1, 1, 1] = ruleNo % 2 == 1 ? true : false;
        }

        public override void Step()
        {
            BitArray newState = new BitArray(size);

            for (int i = 1; i < size - 1; i++)
            {
                newState[i] = rule[state[i - 1]?1:0, state[i]?1:0, state[i + 1]?1:0];
            }
            newState[0] = rule[0, state[0] ? 1 : 0, state[1] ? 1 : 0];
            newState[size - 1] = rule[state[size - 2] ? 1 : 0, state[size - 1] ? 1 : 0, 0];

            state = newState;
            time++;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() << 8 + ruleNumber;
        }

        public override object Clone()
        {
            return new BasicAutomaton(ruleNumber, state);
        }

        public override string TellType()
        {
            return "Basic Automaton No. " + ruleNumber;
        }

        private bool[] getRangeRule()
        {
            bool[] ruleRange = new bool[8];
            byte ruleCopy = ruleNumber;
            for (int i = 0; i < 8; i++)
            {
                ruleRange[i] = ruleCopy % 2 == 1;
                ruleCopy /= 2;
            }
            return ruleRange;
        }

        /// <summary>
        /// Creates an equivalent CA, which only has a "more general" type.
        /// </summary>
        /// <returns>A "copy" of this CA as <c>BinaryAutomatonRangeN</c>.</returns>
        public BinaryAutomatonRangeN ConvertToRangeN()
        {           
            return new BinaryAutomatonRangeN(1, getRangeRule(), state);
        }

        /// <summary>
        /// Creates a new CA with the same state and the same rule, but connected boundaries.
        /// </summary>
        /// <returns>A modified "copy" of this CA as <c>BinaryAutomatonCyclicRangeN</c>.</returns>
        public BinaryAutomatonCyclicRangeN ConvertToCyclicN()
        {
            return new BinaryAutomatonCyclicRangeN(1, getRangeRule(), state);
        }
    }
}
