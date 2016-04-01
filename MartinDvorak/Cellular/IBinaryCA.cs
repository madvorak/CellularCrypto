using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Common interface for all binary cellular automata. Most work with CA is done through this interface.
    /// Only subclasses of <c>CellularAutomaton</c> are supposed to implement this interface.
    /// </summary>
    interface IBinaryCA
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
        /// Tells the specified bit of the CA.
        /// </summary>
        /// <param name="index">Zero-based index (which bit is required).</param>
        /// <returns>One bit.</returns>
        /// <exception>Throws exception if index is out of range.</exception>
        bool GetValueAt(int index);

        /// <summary>
        /// Performs one step of the cellular automaton. Always calls <code>CellularAutomaton.Step();</code>.
        /// </summary>
        void Step();

        /// <summary>
        /// Clones the underlying CA including its state at the moment.
        /// </summary>
        /// <returns>Identical copy. The result is the same as when calling <code>CellularAutomaton.Clone();</code>,
        /// only its type is <c>IBinaryCA</c> (which is useful).</returns>
        IBinaryCA CloneEverything();

        /// <summary>
        /// Creates a new binary CA with the same type and the same rules.
        /// </summary>
        /// <param name="newInstanceState">Initial state of the new (returned) instance.</param>
        /// <returns>New binary CA with identical behaviour, but newly given initial state.</returns>
        IBinaryCA CloneTemplate(BitArray newInstanceState);

        /// <summary>
        /// Announces the runtime type of the CA including info about its rule. It serves for debugging purposes.
        /// </summary>
        /// <returns>The same string as calling <code>CellularAutomaton.TellType();</code>.</returns>
        string TellType();
    }
}
