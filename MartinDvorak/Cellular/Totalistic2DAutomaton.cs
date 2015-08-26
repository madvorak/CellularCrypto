using System.Collections;

namespace Cellular
{
    class Totalistic2DAutomaton : Binary2DAutomaton              //uses von Neumann Neighborhood
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
            this.ruleLive = ruleLive;
            this.ruleDead = ruleDead;
        }

        public Totalistic2DAutomaton(bool[] ruleLive, bool[] ruleDead, BitArray[] initialState) : base(initialState)
        {
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
