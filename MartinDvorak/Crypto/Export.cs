using Cellular;

namespace Crypto
{
    public static class Export
    {
        /// <summary>
        /// Exports a key extender for use by external program.
        /// </summary>
        /// <returns></returns>
        public static IKeyExtender GetKeyExtender()
        {
            return new KeyExtenderSimpleLinear(new ElementaryFastAutomaton());
        }
    }
}
