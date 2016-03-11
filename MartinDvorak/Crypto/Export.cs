using Cellular;

namespace Crypto
{
    public static class Export
    {
        public static IKeyExtender GetKeyExtender()
        {
            return new KeyExtenderSimpleLinear(new ElementaryFastAutomaton());
        }
    }
}
