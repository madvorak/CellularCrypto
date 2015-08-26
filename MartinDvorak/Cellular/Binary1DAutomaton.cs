using System;
using System.Text;
using System.Collections;

namespace Cellular
{
    abstract class Binary1DAutomaton : Automaton1D, BinaryCA
    {
        protected BitArray state;               //originally bool[]

        public Binary1DAutomaton(int size)
        {
            this.size = size;
            state = new BitArray(size);
            for (int i = 0; i < size; i++) state[i] = false;        //unnecessary
            state[size / 2] = true;                                 //000...00100...000 is default initial state
        }

        public Binary1DAutomaton(BitArray initialState)
        {
            size = initialState.Length;
            state = initialState;
        }

        public Binary1DAutomaton(int size, int seed)
        {
            this.size = size;
            Random r = new Random(seed);
            state = new BitArray(size);
            for (int i = 0; i < size; i++) state[i] = r.Next(2) == 1;
        }

        int BinaryCA.GetSize()
        {
            return size;
        }

        string BinaryCA.StateAsString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                sb.Append(state[i] ? '█' : ' ');
            }
            return sb.ToString();
        }

        uint[] BinaryCA.GetPacked()
        {
            uint[] packed = new uint[(size + 31) / 32];       //works like ceil(size/32)
            for (int i = 0; i < packed.Length; i++) packed[i] = 0;
            for (int i = 0; i < size; i++)
            {
                if (state[i]) packed[i / 32] |= 1u << (31 - i % 32);
            }
            return packed;
        }

        bool BinaryCA.GetValueAt(int index)
        {
            return state[index];
        }

        void BinaryCA.Step()
        {
            this.Step();
        }

        BinaryCA BinaryCA.Clone()
        {
            return (BinaryCA)this.Clone();
        }

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
