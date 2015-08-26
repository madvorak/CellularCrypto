using System;
using System.Text;
using System.Collections;

namespace Cellular
{
    abstract class Binary2DAutomaton : Automaton2D, BinaryCA
    {
        protected BitArray[] state;

        public Binary2DAutomaton(int width, int height)
        {
            this.width = width;
            this.height = height;
            state = new BitArray[height];
            for (int i = 0; i < height; i++)
            {
                state[i] = new BitArray(width);
                for (int j = 0; j < width; j++)
                    state[i][j] = false;
            }
            state[height / 2][width / 2] = true;
        }

        public Binary2DAutomaton(BitArray[] initialState)
        {
            height = initialState.Length;
            width = initialState[0].Length;
            state = initialState;
        }

        public Binary2DAutomaton(int width, int height, int seed)
        {
            this.width = width;
            this.height = height;
            Random r = new Random(seed);
            state = new BitArray[height];
            for (int i = 0; i < height; i++)
            {
                state[i] = new BitArray(width);
                for (int j = 0; j < width; j++)
                    state[i][j] = r.Next(2) == 1;
            }
        }

        int BinaryCA.GetSize()
        {
            return width * height;
        }

        string BinaryCA.StateAsString()
        {
            StringBuilder sb = new StringBuilder();
            for (int line = 0; line < height; line++)
            {
                for (int i = 0; i < width; i++)
                {
                    sb.Append(state[line][i] ? '█' : ' ');
                }
                sb.AppendLine();
            } 
            return sb.ToString();
        }

        uint[] BinaryCA.GetPacked()
        {
            uint[] packed = new uint[(width*height + 31) / 32];       //works like ceil(size/32)
            for (int i = 0; i < packed.Length; i++) packed[i] = 0;
            int index = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (state[i][j]) packed[index / 32] |= 1u << (31 - index % 32);          // index = i * width + j
                    index++;
                }
            }
            return packed;
        }

        bool BinaryCA.GetValueAt(int index)
        {
            int i = index / width;
            int j = index % width;
            return state[i][j];
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
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    hash = (hash * 2) % 8388593; // the greatest prime < int.MaxValue / 256
                    if (state[i][j]) hash++;
                }
            return hash;
        }
    }
}
