namespace Cellular
{
    interface BinaryCA
    {
        /// <summary>
        /// Tells the size of the CA.
        /// </summary>
        /// <returns>Number of bits.</returns>
        int GetSize();

        /// <summary>
        /// Converts the inner state of the CA into a well-printable string using <code>state[i] ? '█' : ' ';</code>
        /// </summary>
        /// <returns>State of the CA as a string.</returns>
        string StateAsString();

        /// <summary>
        /// Converts the inner state of the CA into an array of <c>System.UInt32</c>.
        /// So every 32 cells are saved into one uint (where the original array is treated as MSB-first).
        /// </summary>
        /// <returns>Condensed state of the CA.</returns>
        uint[] GetPacked();

        /// <summary>
        /// Tells the specific bit of the CA.
        /// </summary>
        /// <param name="index">Zero-based index (which bit is required).</param>
        /// <returns>One bit.</returns>
        /// <exception>Throws exception if index is out of range.</exception>
        bool GetValueAt(int index);

        /// <summary>
        /// Performs one step of the underlying CA. Always calls <code>CellularAutomaton.Step();</code>.
        /// </summary>
        void Step();

        /// <summary>
        /// Clones the underlying CA.
        /// </summary>
        /// <returns>The result is the same as when calling <code>CellularAutomaton.Clone();</code>,
        /// only the type is <c>BinaryCA</c> (which is useful).</returns>
        BinaryCA Clone();
    }
}
