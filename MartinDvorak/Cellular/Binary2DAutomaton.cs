using System;
using System.Text;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// This class contains base-constructors for all binary 2D automata and implementation of the <c>BinaryCA</c> interface.
    /// The state is kept in an array of <c>BitArray</c>s reprezenting rows. It used to be bool[,] originally.
    /// </summary>
    abstract class Binary2DAutomaton : Automaton2D, BinaryCA
    {
        protected BitArray[] state; //represents rows

        /// <summary>
        /// Creates a new <c>Binary2DAutomaton</c> of given size with one living cell in the middle, the rest is dead.
        /// </summary>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
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

        /// <summary>
        /// Creates a new <c>Binary2DAutomaton</c> of given initial state.
        /// </summary>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
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

        /// <summary>
        /// Flattens the inner state of the CA and converts it into an array of <c>System.UInt32</c>.
        /// So every 32 cells are saved into one uint (where the original array is treated as MSB-first).
        /// </summary>
        /// <returns>Condensed state of the CA in a simple one-dimensional array.</returns>
        uint[] BinaryCA.GetPacked()
        {
            uint[] packed = new uint[(width*height + 31) / 32];       // works like ceil(size/32)
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
