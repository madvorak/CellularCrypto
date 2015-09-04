using System;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing the classic Conway's Game of Life automaton.
    /// </summary>
    class GameOfLife : Totalistic2DAutomaton
    {
        /// <summary>
        /// Creates a new Game of Life with a random initial state.
        /// </summary>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
        /// <param name="rnd">PseudoRNG instance that will be used to generate the original state.</param>
        public GameOfLife(int width, int height, Random rnd) 
            : base(
                 new bool[] { false, false, true, true, false, false, false, false, false },
//number of neighbours alive:   0      1      2     3     4      5      6      7      8
                 new bool[] { false, false, false, true, false, false, false, false, false }
                 , width, height, rnd) { }

        /// <summary>
        /// Creates a new Game of Life with givin initial state.
        /// </summary>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
        public GameOfLife(BitArray[] initialState)
            : base(
                 new bool[] { false, false, true, true, false, false, false, false, false },
                 new bool[] { false, false, false, true, false, false, false, false, false }
                 , initialState) { }

        public override string TellType()
        {
            return "Game of Life";
        }
    }
}
