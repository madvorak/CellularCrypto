﻿using System;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// This class can work with any binary 1D automaton with symmetric scope.
    /// The automata have firmly set borders. Referencing a cell beyond borders acts as referencing a dead cell.
    /// </summary>
    class BinaryAutomatonRangeN : Binary1DAutomaton
    {
        protected bool[] rule;
        protected byte range;

        /// <summary>
        /// Creates a new CA of a general rule with 000...00100...000 as its initial state.
        /// </summary>
        /// <param name="scope">How many cells on each side from the center determine the next state of the cell.
        /// Value 1 makes it equivalent to a <c>BasicAutomaton</c> which have rule of size 8.
        /// Value 2 means that each new state of any cell depends on five total cells => size of rule must be 32.</param>
        /// <param name="rule">Array representing the rule for creating a new state.
        /// rule[0] is rule for 0..0, therefore opposite order to the rules of basic automata.</param>
        /// <param name="size">The size of the new CA.</param>
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

        /// <summary>
        /// Creates a new CA of a general rule with defined inital state.
        /// </summary>
        /// <param name="scope">How many cells on each side from the center determine the next state of the cell.
        /// Value 1 makes it equivalent to a <c>BasicAutomaton</c> which have rule of size 8.
        /// Value 2 means that each new state of any cell depends on five total cells => size of rule must be 32.</param>
        /// <param name="rule">Array representing the rule for creating a new state.
        /// rule[0] is rule for 0..0, therefore opposite order to the rules of basic automata.</param>
        /// <param name="initialState">A <c>BitArray</c> describing the initial state of the CA.
        /// This also determines the size of the new CA.</param>
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

        /// <summary>
        /// This method simplifies boundary conditions.
        /// </summary>
        /// <param name="index">Zero-based index (which bit is required).</param>
        /// <returns>One bit.</returns>
        protected virtual bool getValueAt(int index)
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
                    if (getValueAt(j)) bin++;   // depending on the ref in VMT, this behaves as limited or cyclic impl
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
