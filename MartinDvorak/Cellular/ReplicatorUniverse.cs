using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing the Replicator Universe (2D Moore neighborhood automaton 1357/1357).
    /// </summary>
    class ReplicatorUniverse : Totalistic2DAutomaton
    {
        /// <summary>
        /// Creates a new Replicator Universe with a random initial state.
        /// </summary>
        /// <param name="width">The width of the new CA (length of rows).</param>
        /// <param name="height">The height of the new CA (number of rows).</param>
        public ReplicatorUniverse(int width, int height) : base(
                 new bool[] { false, true, false, true, false, true, false, true, false },
//number of neighbours alive:   0      1      2     3     4     5      6     7      8
                 new bool[] { false, true, false, true, false, true, false, true, false }
                 , width, height, Program.rnd) { }

        /// <summary>
        /// Creates a new Replicator Universe with givin initial state.
        /// </summary>
        /// <param name="initialState">An array of <c>BitArray</c>s describing the initial state of the CA.
        /// This also determines the size (width, height) of the new CA.</param>
        public ReplicatorUniverse(BitArray[] initialState) : base(
                 new bool[] { false, true, false, true, false, true, false, true, false },
                 new bool[] { false, true, false, true, false, true, false, true, false }
                 , initialState) { }

        public override string TellType()
        {
            return "Replicator Universe";
        }
    }
}
