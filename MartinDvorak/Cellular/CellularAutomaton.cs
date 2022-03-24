namespace Cellular
{
    /// <summary>
    /// The top class of the CA hierarchy.
    /// Every constructor should follow this logical order when considering its parametres:
    /// 1) specification of the type (usually not needed), 2) rule, 3) initial state / size (& rng).
    /// </summary>
    public abstract class CellularAutomaton
    {
        protected uint time = 0;

        /// <summary>
        /// Performs one step applying the specific rule for creating a new state.
        /// </summary>
        public abstract void Step();

        /// <summary>
        /// Only returns how many steps have been done.
        /// </summary>
        /// <returns>Number of executed steps.</returns>
        public uint GetTime()
        {
            return time;
        }

        /// <summary>
        /// Performs the step n times.
        /// </summary>
        /// <param name="times">How many times <c>Step()</c> should be called.</param>
        public void Step(uint times)
        {
            for (int i = 0; i < times; i++)
            {
                Step();         //the method Step() must increase "time" counter on its own
            }
        }

        /// <summary>
        /// Creates an appropriate copy of the CA. Its type and the specific rule are always preserved. 
        /// However, the time (number of steps executed on the specific instance) is set back to 0.
        /// </summary>
        /// <returns>A copy of the <c>CellularAutomaton</c> as <c>System.Object</c></returns>
        public abstract object Clone();

        /// <summary>
        /// Announces the runtime type of the CA including info about its rule.
        /// </summary>
        /// <returns>Type of the CA as a string.</returns>
        public abstract string TellType();
    }
}
