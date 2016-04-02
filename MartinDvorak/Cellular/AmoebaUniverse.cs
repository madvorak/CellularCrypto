using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing the Amoeba Universe (2D Moore neighborhood automaton 1358/357).
    /// </summary>
    class AmoebaUniverse : Totalistic2DAutomaton
    {
        /// <summary>
        /// Creates a new Amoeba Universe with a random initial state.
        /// </summary>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
        public AmoebaUniverse(int width, int height) : base(
                 new bool[] { false, true, false, true, false, true, false, false, true },
//number of neighbours alive:   0      1      2     3     4     5      6      7      8
                 new bool[] { false, false, false, true, false, true, false, true, false }
                 , width, height, Program.rnd) { }

        /// <summary>
        /// Creates a new Amoeba Universe with givin initial state.
        /// </summary>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
        public AmoebaUniverse(BitArray[] initialState) : base(
                 new bool[] { false, true, false, true, false, true, false, false, true },
                 new bool[] { false, false, false, true, false, true, false, true, false }
                 , initialState) { }

        public override string TellType()
        {
            return "Amoeba Universe";
        }
    }
}
