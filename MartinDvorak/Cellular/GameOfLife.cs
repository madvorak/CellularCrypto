﻿using System;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// This class represents a classic Conway's Game of Life automaton.
    /// </summary>
    class GameOfLife : Totalistic2DAutomaton
    {
        public GameOfLife(int width, int height, Random rnd) 
            : base(
                 new bool[] { false, false, true, true, false, false, false, false, false },
//number of neighbours alive:   0      1      2     3     4      5      6      7      8
                 new bool[] { false, false, false, true, false, false, false, false, false }
                 , width, height, rnd)
        { }

        /// <summary>
        /// Creates a new Game of Life with givin initial state.
        /// </summary>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
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
