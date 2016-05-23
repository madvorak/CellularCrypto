using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing a totalistic 2D automaton that uses Moore Neighborhood with enhanced functionalities.
    /// For instance, this can be used as Conway's Game of Life automaton for interactive use (e.g. in Winforms).
    /// </summary>
    class Totalistic2DAutomatonInteractive : Totalistic2DAutomaton
    {
        /// <summary>
        /// Creates a new interactive totalistic 2D automaton of given ruleset with a random initial state.
        /// </summary>
        /// <param name="ruleLive">Array that must contain 9 logical values.
        /// The value in <c>ruleLive[i]</c> says what happens to a living cell when it has exactly i neighbours alive.</param>
        /// <param name="ruleDead">Array that must contain 9 logical values.
        /// The value in <c>ruleDead[i]</c> says what happens to a dead cell when it has exactly i neighbours alive.</param>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
        public Totalistic2DAutomatonInteractive(bool[] ruleLive, bool[] ruleDead, int width, int height)
            : base(ruleLive, ruleDead, width, height) {}

        /// <summary>
        /// Creates a new interactive totalistic 2D automaton of given ruleset with given initial state.
        /// </summary>
        /// <param name="ruleLive">Array that must contain 9 logical values.
        /// The value in <c>ruleLive[i]</c> says what happens to a living cell when it has exactly i neighbours alive.</param>
        /// <param name="ruleDead">Array that must contain 9 logical values.
        /// The value in <c>ruleDead[i]</c> says what happens to a dead cell when it has exactly i neighbours alive.</param>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
        public Totalistic2DAutomatonInteractive(bool[] ruleLive, bool[] ruleDead, BitArray[] initialState)
            : base(ruleLive, ruleDead, initialState) {}

        /// <summary>
        /// Creates a new interactive Game of Life with a random initial state.
        /// </summary>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
        public static Totalistic2DAutomatonInteractive CreateGameOfLifeInteractive(int width, int height)
        {
            return new Totalistic2DAutomatonInteractive(liveGameOfLife, deadGameOfLife, width, height);
        }

        /// <summary>
        /// Creates a new interactive Game of Life with a given initial state.
        /// </summary>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
        public static Totalistic2DAutomatonInteractive CreateGameOfLifeInteractive(BitArray[] initialState)
        {
            return new Totalistic2DAutomatonInteractive(liveGameOfLife, deadGameOfLife, initialState);
        }

        /// <summary>
        /// Creates a new interactive Replicator Universe with a random initial state.
        /// </summary>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
        /// <returns></returns>
        public static Totalistic2DAutomatonInteractive CreateReplicatorUniverseInteractive(int width, int height)
        {
            return new Totalistic2DAutomatonInteractive(liveReplicatorUniverse, deadReplicatorUniverse, width, height);
        }

        /// <summary>
        /// Creates a new interactive Replicator Universe with a given initial state.
        /// </summary>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
        public static Totalistic2DAutomatonInteractive CreateReplicatorUniverseInteractive(BitArray[] initialState)
        {
            return new Totalistic2DAutomatonInteractive(liveReplicatorUniverse, deadReplicatorUniverse, initialState);
        }

        /// <summary>
        /// Tells one bit of the CA, specified by 2D coordinates.
        /// </summary>
        /// <param name="row">Vertical coordinate.</param>
        /// <param name="column">Horizontal coordinate.</param>
        /// <returns>State of the cell.</returns>
        public bool GetCell(int row, int column)
        {
            return state[row][column];
        }

        /// <summary>
        /// Sets a new value to the specified cell. Immutability of inner data structures is not broken.
        /// </summary>
        /// <param name="row">Vertical coordinate.</param>
        /// <param name="column">Horizontal coordinate.</param>
        /// <param name="value">New value.</param>
        public void SetCell(int row, int column, bool value)
        {
            BitArray[] newState = new BitArray[height];
            state.CopyTo(newState, 0);
            newState[row] = new BitArray(state[row]);
            newState[row][column] = value;
            state = newState;
        }

        /// <summary>
        /// Inverts the specified cell. Immutability of inner data structures is not broken.
        /// </summary>
        /// <param name="row">Vertical coordinate.</param>
        /// <param name="column">Horizontal coordinate.</param>
        public void FlipCell(int row, int column)
        {
            SetCell(row, column, !state[row][column]);
        }
    }
}
