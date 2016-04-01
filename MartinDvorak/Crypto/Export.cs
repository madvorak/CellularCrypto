using Cellular;

namespace Crypto
{
    /// <summary>
    /// Public static class that gives access to our cryptographical tools. Can be used from any assembly.
    /// </summary>
    public static class Export
    {
        /// <summary>
        /// Exports a key extender for use by any external program.
        /// </summary>
        /// <returns>Key extender.</returns>
        public static IKeyExtender GetKeyExtender()
        {
            return new KeyExtenderSimpleLinear(new ElementaryFastAutomaton());
        }
    }
}
