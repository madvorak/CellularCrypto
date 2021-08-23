using System.Collections;

namespace Crypto
{
    /// <summary>
    /// Interface for all algorithms that can perform key stretching. Can be used from any assembly.
    /// </summary>
    public interface IKeyExtender
    {
        /// <summary>
        /// Generates a longer key from a short key.
        /// </summary>
        /// <param name="shortKey">Short key to be stretched (doubled).</param>
        /// <returns>Long key (twice longer than input).</returns>
        BitArray DoubleKey(BitArray shortKey);

        /// <summary>
        /// Generates a longer key from a short key.
        /// </summary>
        /// <param name="shortKey">Short key to be stretched (multiplied).</param>
        /// <param name="targetLength">How many bits should the resulting long key have?</param>
        /// <returns>Long key (of specified length).</returns>
        BitArray ExtendKey(BitArray shortKey, int targetLength);
    }
}
