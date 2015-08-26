using System;
using System.Collections;

namespace Cellular
{
    class BinaryAutomatonRangeN : Binary1DAutomaton
    {
        protected bool[] rule;      //rule[0] is rule for 0..0, therefore opposite order to the rules of basic automata
        protected byte range;

        public BinaryAutomatonRangeN(byte scope, bool[] rule, int size) : base(size)
        {
            if (rule.Length == (1 << (2 * scope + 1)))
            {
                range = scope;
                this.rule = rule;
            }
            else
            {
                throw new ArgumentException("Wrong size of the array describing the rule.");
            }
        }

        public BinaryAutomatonRangeN(byte scope, bool[] rule, BitArray initialState) : base(initialState)
        {
            if (rule.Length == (1 << (2 * scope + 1)))
            {
                range = scope;
                this.rule = rule;
            }
            else
            {
                throw new ArgumentException("Wrong size of the array describing the rule.");
            }
        }

        protected virtual bool ValueAt(int index)        //simplifies boundary conditions
        {
            if (index >= 0 && index < size)
            {
                return state[index];
            }
            else
            {
                return false;
            }
        }

        public override void Step()
        {
            BitArray newState = new BitArray(size);

            for (int i = 0; i < size; i++)
            {
                uint bin = 0;
                for (int j = i - range; j <= i + range; j++)       //can be optimized to only one reading of state
                {
                    bin *= 2;
                    if (ValueAt(j)) bin++;
                }
                newState[i] = rule[bin];
            }

            state = newState;
            time++;
        }

        public override object Clone()
        {
            return new BinaryAutomatonRangeN(range, rule, state);
        }

        public override string TellType()
        {
            var sb = new System.Text.StringBuilder();
            foreach (bool b in rule)
            {
                if (b) sb.Append("1");
                else sb.Append("0");
            }
            return "Binary automaton with rule " + sb.ToString();
        }
    }
}
