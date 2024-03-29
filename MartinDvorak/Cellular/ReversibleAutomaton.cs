﻿using System;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing any binary 1D automaton with any symmetric rule which is made reversible.
    /// This is very inefficient (possibly throwaway) implementation,
    /// because it reuses <c>BinaryRangeCyclicAutomaton</c> for every step.
    /// </summary>
    class ReversibleAutomaton : Binary1DAutomaton
    {
        private byte range;
        private bool[] rule;
        private BitArray prevState;

        /// <summary>
        /// Creates a new <c>ReversibleAutomaton</c>.
        /// </summary>
        /// <param name="range">How many cells on each side from the center determine the next state of the cell.</param>
        /// <param name="rule">Array representing the rule for creating a new state. 
        /// Description of combination with the previous state is not included.</param>
        /// <param name="previousState">A <c>BitArray</c> describing the "previous" (not really) state of the CA
        /// (needed to calculate the next state).</param>
        /// <param name="currentState">A <c>BitArray</c> describing the current state of the CA.</param>
        public ReversibleAutomaton(byte range, bool[] rule, BitArray previousState, BitArray currentState) 
            : base(currentState)
        {
            if (rule.Length != (1 << (2 * range + 1)))
            {
                throw new ArgumentException("Wrong size of the array describing the rule.");
            }
            this.range = range;
            this.rule = rule;
            prevState = previousState;
            state = currentState;
        }

        /// <summary>
        /// Creates a new <c>ReversibleAutomaton</c>.
        /// </summary>
        /// <param name="range">How many cells on each side from the center determine the next state of the cell.</param>
        /// <param name="rule">Array representing the rule for creating a new state. 
        /// Description of combination with the previous state is not included.</param>
        /// <param name="previousState">A <c>BitArray</c> describing the "previous" (not really) state of the CA
        /// (needed to calculate the next state).</param>
        /// <param name="currentState">A <c>BitArray</c> describing the current state of the CA.</param>
        public ReversibleAutomaton(byte range, BitArray rule, BitArray previousState, BitArray currentState)
            : this(range, Utilities.BitArrToBoolArr(rule), previousState, currentState)
        { }

        public override void Step()
        {
            IBinaryCA helpCA = new BinaryRangeCyclicAutomaton(range, rule, state);
            helpCA.Step();
            BitArray newState = new BitArray(state.Length);
            for (int i = 0; i < state.Length; i++)
            {
                newState[i] = helpCA.GetValueAt(i) ^ prevState[i];
            }
            prevState = state;
            state = newState;
            time++;
        }

        public override string TellType()
        {
            return "Binary 1D reversible automaton with scope " + range;
        }

        public override object Clone()
        {
            return new ReversibleAutomaton(range, rule, prevState, state);
        }

        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            return new ReversibleAutomaton(range, rule, state, newInstanceState);
        }
    }
}
