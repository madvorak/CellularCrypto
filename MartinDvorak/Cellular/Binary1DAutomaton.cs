using System;
using System.Text;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class containing base-constructors for all binary 1D automata and implementation of the <c>IBinaryCA</c> interface.
    /// The state is kept in a <c>BitArray</c>.
    /// </summary>
    abstract class Binary1DAutomaton : Automaton1D, IBinaryCA
    {
        protected BitArray state;

        /// <summary>
        /// Creates a new <c>Binary1DAutomaton</c> of given size with 000...00100...000 as its initial state.
        /// </summary>
        /// <param name="size">The size of the new CA.</param>
        public Binary1DAutomaton(int size)
        {
            this.size = size;
            state = new BitArray(size, false);
            state[size / 2] = true;
        }

        /// <summary>
        /// Creates a new <c>Binary1DAutomaton</c> of given initial state.
        /// </summary>
        /// <param name="initialState">A <c>BitArray</c> describing the initial state of the CA.
        /// This also determines the size of the new CA.</param>
        public Binary1DAutomaton(BitArray initialState)
        {
            size = initialState.Length;
            state = initialState;
        }

        /// <summary>
        /// Creates a new <c>Binary1DAutomaton</c> of given size with a random initial state.
        /// </summary>
        /// <param name="size">The size of the new CA.</param>
        /// <param name="rnd">PseudoRNG instance that will be used to generate the original state.</param>
        public Binary1DAutomaton(int size, Random rnd)
        {
            this.size = size;
            state = new BitArray(size);
            for (int i = 0; i < size; i++) state[i] = rnd.Next(2) == 1;
        }

        int IBinaryCA.GetSize()
        {
            return size;
        }

        string IBinaryCA.StateAsString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                sb.Append(state[i] ? '█' : ' ');
            }
            return sb.ToString();
        }

        uint[] IBinaryCA.GetPacked()
        {
            uint[] packed = new uint[(size + 31) / 32];       //works like ceil(size/32)
            for (int i = 0; i < packed.Length; i++) packed[i] = 0;
            for (int i = 0; i < size; i++)
            {
                if (state[i]) packed[i / 32] |= 1u << (31 - i % 32);
            }
            return packed;
        }

        bool IBinaryCA.GetValueAt(int index)
        {
            return state[index];
        }

        void IBinaryCA.Step()
        {
            this.Step();
        }

        IBinaryCA IBinaryCA.CloneEverything()
        {
            return (IBinaryCA)this.Clone();
        }

        IBinaryCA IBinaryCA.CloneTemplate(BitArray newInstanceState)
        {
            return this.cloneTemplate(newInstanceState);
        }

        abstract protected IBinaryCA cloneTemplate(BitArray newInstanceState);

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < size; i++)
            {
                hash = (hash * 2) % 8388593; // the greatest prime < int.MaxValue / 256
                if (state[i]) hash++;
            }
            return hash;
        }
    }
}
