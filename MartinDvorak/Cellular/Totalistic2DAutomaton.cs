using System;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing a totalistic 2D automaton that uses Moore Neighborhood.
    /// </summary>
    class Totalistic2DAutomaton : Binary2DAutomaton
    {
        protected bool[] ruleLive, ruleDead;

        /// <summary>
        /// Creates a new totalistic 2D automaton of given ruleset with given initial state.
        /// </summary>
        /// <param name="ruleLive">Array that must contain 9 logical values.
        /// The value in <c>ruleLive[i]</c> says what happens to a living cell when it has exactly i neighbours alive.</param>
        /// <param name="ruleDead">Array that must contain 9 logical values.
        /// The value in <c>ruleDead[i]</c> says what happens to a dead cell when it has exactly i neighbours alive.</param>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
        /// <param name="rnd">PseudoRNG instance that will be used to generate the original state.</param>
        public Totalistic2DAutomaton(bool[] ruleLive, bool[] ruleDead, int width, int height, Random rnd) : base(width, height, rnd)
        {
            if (ruleLive == null || ruleLive.Length != 9 || ruleDead == null || ruleLive.Length != 9)
            {
                throw new ArgumentException("The array describing the rule must have have 9 elements!");
            }
            this.ruleLive = ruleLive;
            this.ruleDead = ruleDead;
        }

        /// <summary>
        /// Creates a new totalistic 2D automaton of given ruleset with given initial state.
        /// </summary>
        /// <param name="ruleLive">Array that must contain 9 logical values.
        /// The value in <c>ruleLive[i]</c> says what happens to a living cell when it has exactly i neighbours alive.</param>
        /// <param name="ruleDead">Array that must contain 9 logical values.
        /// The value in <c>ruleDead[i]</c> says what happens to a dead cell when it has exactly i neighbours alive.</param>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
        public Totalistic2DAutomaton(bool[] ruleLive, bool[] ruleDead, BitArray[] initialState) : base(initialState)
        {
            if (ruleLive == null || ruleLive.Length != 9 || ruleDead == null || ruleLive.Length != 9)
            {
                throw new ArgumentException("The array describing the rule must have have 9 elements!");
            }
            this.ruleLive = ruleLive;
            this.ruleDead = ruleDead;
        }

        public override void Step()
        {
            BitArray[] newState = new BitArray[height];
            for (int i = 0; i < height; i++) newState[i] = new BitArray(width);
            byte liveNeig;

            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                { 
                    liveNeig = 0;
                    if (state[i - 1][j - 1]) liveNeig++;
                    if (state[i - 1][j]) liveNeig++;
                    if (state[i - 1][j + 1]) liveNeig++;
                    if (state[i][j - 1]) liveNeig++;
                    if (state[i][j + 1]) liveNeig++;
                    if (state[i + 1][j - 1]) liveNeig++;
                    if (state[i + 1][j]) liveNeig++;
                    if (state[i + 1][j + 1]) liveNeig++;

                    if (state[i][j])                            //inside
                    {
                        newState[i][j] = ruleLive[liveNeig];
                    }
                    else
                    {
                        newState[i][j] = ruleDead[liveNeig];
                    }
                }                           

                liveNeig = 0;
                if (state[i - 1][0]) liveNeig++;
                if (state[i + 1][0]) liveNeig++;
                if (state[i - 1][1]) liveNeig++;
                if (state[i][1]) liveNeig++;
                if (state[i + 1][1]) liveNeig++;
                if (state[i][0])                                //left corner
                {
                    newState[i][0] = ruleLive[liveNeig];
                }
                else
                {
                    newState[i][0] = ruleDead[liveNeig];
                }

                liveNeig = 0;
                if (state[i - 1][width - 1]) liveNeig++;
                if (state[i + 1][width - 1]) liveNeig++;
                if (state[i - 1][width - 2]) liveNeig++;
                if (state[i][width - 2]) liveNeig++;
                if (state[i + 1][width - 2]) liveNeig++;
                if (state[i][width - 1])                        //right corner
                {
                    newState[i][width - 1] = ruleLive[liveNeig];
                }
                else
                {
                    newState[i][width - 1] = ruleDead[liveNeig];
                }
            }

            for (int j = 1; j < width - 1; j++)
            {
                liveNeig = 0;
                if (state[0][j - 1]) liveNeig++;
                if (state[0][j + 1]) liveNeig++;
                if (state[1][j - 1]) liveNeig++;
                if (state[1][j]) liveNeig++;
                if (state[1][j + 1]) liveNeig++;
                if (state[0][j])                                //top corner
                {
                    newState[0][j] = ruleLive[liveNeig];
                }
                else
                {
                    newState[0][j] = ruleDead[liveNeig];
                }

                liveNeig = 0;
                if (state[height - 1][j - 1]) liveNeig++;
                if (state[height - 1][j + 1]) liveNeig++;
                if (state[height - 2][j - 1]) liveNeig++;
                if (state[height - 2][j]) liveNeig++;
                if (state[height - 2][j + 1]) liveNeig++;
                if (state[height - 1][j])                       //bottom corner
                {
                    newState[height - 1][j] = ruleLive[liveNeig];
                }
                else
                {
                    newState[height - 1][j] = ruleDead[liveNeig];
                }
            }

            liveNeig = 0;
            if (state[0][1]) liveNeig++;
            if (state[1][0]) liveNeig++;
            if (state[1][1]) liveNeig++;
            if (state[0][0])                                //top left corner
            {
                newState[0][0] = ruleLive[liveNeig];
            }
            else
            {
                newState[0][0] = ruleDead[liveNeig];
            }

            liveNeig = 0;
            if (state[0][width - 2]) liveNeig++;
            if (state[1][width - 1]) liveNeig++;
            if (state[1][width - 2]) liveNeig++;
            if (state[0][width - 1])                        //top right corner
            {
                newState[0][width - 1] = ruleLive[liveNeig];
            }
            else
            {
                newState[0][width - 1] = ruleDead[liveNeig];
            }

            liveNeig = 0;
            if (state[height - 1][1]) liveNeig++;
            if (state[height - 2][0]) liveNeig++;
            if (state[height - 2][1]) liveNeig++;
            if (state[height - 1][0])                        //bottom left corner
            {
                newState[height - 1][0] = ruleLive[liveNeig];
            }
            else
            {
                newState[height - 1][0] = ruleDead[liveNeig];
            }

            liveNeig = 0;
            if (state[height - 1][width - 2]) liveNeig++;
            if (state[height - 2][width - 1]) liveNeig++;
            if (state[height - 2][width - 2]) liveNeig++;
            if (state[height - 1][width - 1])                //bottom right corner
            {
                newState[height - 1][width - 1] = ruleLive[liveNeig];
            }
            else
            {
                newState[height - 1][width - 1] = ruleDead[liveNeig];
            }

            state = newState;
            time++;
        }

        public override object Clone()
        {
            return new Totalistic2DAutomaton(ruleLive, ruleDead, state);
        }

        /// <summary>
        /// Creates a new binary CA with the same type and the same rules.
        /// </summary>
        /// <param name="newInstanceState">Desired initial state of the new (returned) instance.
        /// It is transformed into a square/rectangular array and padded with zeros/false.</param>
        /// <returns>New binary CA with copied behaviour, but newly given initial state.</returns>
        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            // TODO : the transformation should be maybe pulled outside (ideally to a higher class or Utilities)
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
            return new Totalistic2DAutomaton(ruleLive, ruleDead, bitArray2D);
        }

        public override string TellType()
        {
            return "Totalistic 2D automaton";
        }
    }
}