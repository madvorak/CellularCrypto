using System.Collections;

namespace Cellular
{
    class GameOfLife : Totalistic2DAutomaton
    {          
        public GameOfLife(int width, int height, int seed) 
            : base(
                 new bool[] { false, false, true, true, false, false, false, false, false },
//number of neighbours alive:   0      1      2     3     4      5      6      7      8
                 new bool[] { false, false, false, true, false, false, false, false, false }
                 , width, height, seed)
        { }

        public GameOfLife(BitArray[] initialState)
            : base(
                 new bool[] { false, false, true, true, false, false, false, false, false },
                 new bool[] { false, false, false, true, false, false, false, false, false }
                 , initialState)
        { }

        public override string TellType()
        {
            return "Game of Life";
        }
    }
}
