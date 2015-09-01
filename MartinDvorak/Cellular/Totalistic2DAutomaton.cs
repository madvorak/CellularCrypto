using System;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// This class represents a totalistic 2D automaton that uses Moore Neighborhood.
    /// </summary>
    class Totalistic2DAutomaton : Binary2DAutomaton
    {
        protected bool[] ruleLive, ruleDead;

        public Totalistic2DAutomaton(int width, int height) : base(width, height)
        {
            ruleLive = ruleDead = new bool[9];
        }

        public Totalistic2DAutomaton(int width, int height, int seed) : base(width, height, seed)
        {
            ruleLive = ruleDead = new bool[9];
        }

        public Totalistic2DAutomaton(bool[] ruleLive, bool[] ruleDead, int width, int height, int seed) : base(width, height, seed)
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

        public override string TellType()
        {
            return "Totalistic 2D automaton";
        }
    }
}
