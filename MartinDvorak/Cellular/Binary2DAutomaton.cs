using System;
using System.Text;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class containing base-constructors for all binary 2D automata and implementation of the <c>IBinaryCA</c> interface.
    /// The state is kept in an array of <c>BitArray</c>s reprezenting rows.
    /// </summary>
    abstract class Binary2DAutomaton : Automaton2D, IBinaryCA
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
                state[i] = new BitArray(width, false);
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

        /// <summary>
        /// Creates a new <c>Binary2DAutomaton</c> of given size with a random initial state.
        /// </summary>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
        /// <param name="rnd">PseudoRNG instance that will be used to generate the original state.</param>
        public Binary2DAutomaton(int width, int height, Random rnd)
        {
            this.width = width;
            this.height = height;
            state = new BitArray[height];
            for (int i = 0; i < height; i++)
            {
                state[i] = new BitArray(width);
                for (int j = 0; j < width; j++)
                    state[i][j] = rnd.Next(2) == 1;
            }
        }

        int IBinaryCA.GetSize()
        {
            return width * height;
        }

        string IBinaryCA.StateAsString()
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
        uint[] IBinaryCA.GetPacked()
        {
            uint[] packed = new uint[(width*height + 31) / 32];       // works like ceil(size/32)
            for (int i = 0; i < packed.Length; i++) packed[i] = 0;
            int index = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (state[i][j])
                    {
                        packed[index / 32] |= 1u << (31 - index % 32);          // index = i * width + j
                    }
                    index++;
                }
            }
            return packed;
        }

        bool IBinaryCA.GetValueAt(int index)
        {
            int i = index / width;
            int j = index % width;
            return state[i][j];
        }

        IBinaryCA IBinaryCA.CloneEverything()
        {
            return (IBinaryCA)this.Clone();
        }

        /// <summary>
        /// Creates a new binary CA with the same type and the same rules.
        /// </summary>
        /// <param name="newInstanceState">Desired initial state of the new (returned) instance.
        /// It is transformed into a square/rectangular array and padded with zeros/false.</param>
        /// <returns>New binary CA with copied behaviour, but newly given initial state.</returns>
        IBinaryCA IBinaryCA.CloneTemplate(BitArray newInstanceState)
        {
            int newInSize = newInstanceState.Length;
            int newWidth = (int)Math.Ceiling(Math.Sqrt(newInSize));
            int newHeight = newWidth;
            //size 49 -> 7x7 ; size 50 -> 8x7 ; size 57 -> 8x8
            if (newWidth * (newHeight - 1) >= newInSize)
            {
                newHeight--;
            }
            BitArray[] bitArray2D = new BitArray[newHeight];
            for (int i = 0; i < newHeight; i++)
            {
                bitArray2D[i] = new BitArray(newWidth);
                for (int j = 0; j < newWidth; j++)
                {
                    int inputIndex = i * newWidth + j;
                    bool bit;
                    if (inputIndex < newInSize)
                    {
                        bit = newInstanceState[inputIndex];
                    }
                    else
                    {
                        bit = false;
                    }
                    bitArray2D[i][j] = bit;
                }
            }
            return this.cloneTemplate(bitArray2D);
        }

        abstract protected IBinaryCA cloneTemplate(BitArray[] newInstanceState);

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
