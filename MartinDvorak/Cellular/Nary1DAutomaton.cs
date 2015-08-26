using System;

namespace Cellular
{
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

        public Nary1DAutomaton(int numberOfStates, int size, int seed)
        {
            N = numberOfStates;
            this.size = size;
            Random r = new Random(seed);
            for (uint i = 0; i < size; i++)
            {
                state[i] = r.Next(N);
            }
        }
    }
}
