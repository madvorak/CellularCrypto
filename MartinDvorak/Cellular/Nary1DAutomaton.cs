using System;

namespace Cellular
{
    /// <summary>
    /// Abstract class for general 1D N-ary automata.
    /// </summary>
    abstract class Nary1DAutomaton : Automaton1D
    {
        protected int N;    //number of possible states in a cell
        protected int[] state;

        public Nary1DAutomaton(int numberOfStates, int[] initialState)
        {
            N = numberOfStates;
            state = initialState;
            size = initialState.Length;
        }

        public Nary1DAutomaton(int numberOfStates, int size, Random rnd)
        {
            N = numberOfStates;
            this.size = size;
            for (uint i = 0; i < size; i++)
            {
                state[i] = rnd.Next(N);
            }
        }

        /// <summary>
        /// This method simplifies boundary conditions.
        /// </summary>
        /// <param name="index">Zero-based index (which value is required).</param>
        /// <returns>Value from 0 to N-1.</returns>
        protected virtual int getValueAt(int index)
        {
            if (index < 0 || index >= size)
            { 
                return 0;
            }
            return state[index];
        }
    }
}
