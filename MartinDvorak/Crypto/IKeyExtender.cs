using System.Collections;

namespace Crypto
{
    /// <summary>
    /// Interface for all algorithms that can perform key stretching.
    /// </summary>
    public interface IKeyExtender
    {
        BitArray DoubleKey(BitArray shortKey);

        BitArray ExtendKey(BitArray shortKey, int targetLength);
    }
}
