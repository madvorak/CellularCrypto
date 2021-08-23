using System;
using System.Collections;
using System.IO;
using System.IO.Compression;

namespace Crypto
{
    /// <summary>
    /// Static class that contains some tests of randomness for binary sequences. The tests operate on BitArray.
    /// Use <c>Cellular.Utilities.UintArrToBitArr(uint[] input)</c> to convert to BitArray if needed.
    /// </summary>
    static class RandomnessTesting
    {
        /// <summary>
        /// Calculates the Shannon entropy of the BitArray.
        /// </summary>
        /// <param name="b">Vector to rate.</param>
        /// <param name="lengthLimit">Blocks of size from 1 to <c>lengthLimit</c> will be counted.</param>
        /// <returns>Number between 0 and 1. Only values very close to 1 are good.</returns>
        public static double EntropyTest(BitArray b, byte lengthLimit)
        {
            double score = 0;
            double maxScore = 0;
            for (byte length = 1; length <= lengthLimit; length++)
            {
                int seqPossible = 1 << length;
                int sampleCount = b.Length / length;
                int[] frequencies = new int[seqPossible];
                int[] optimal = new int[seqPossible];
                int optInd = 0;
                for (int j = 0; j < seqPossible; j++)
                {
                    frequencies[j] = 0;
                    optimal[j] = 0;
                }
                for (int j = 0; j < sampleCount; j++)
                {
                    int index = 0;
                    for (int k = 0; k < length; k++)
                    {
                        index *= 2;
                        if (b[j * length + k]) index++;
                    }
                    frequencies[index]++;
                    optimal[optInd++]++;
                    if (optInd == seqPossible) optInd = 0;
                }
                for (int j = 0; j < seqPossible; j++)
                {
                    double freq = (double)frequencies[j] / sampleCount;
                    if (freq > 0)
                    {
                        score += freq * Math.Log(1 / freq, 2) / length;
                    }
                    freq = (double)optimal[j] / sampleCount;
                    if (freq > 0)
                    {
                        maxScore += freq * Math.Log(1 / freq, 2) / length;
                    }
                }
            }
            return score / maxScore;   //weighted average of entropies for block from 1 to lengthLimit; actual vs maximal
        }

        /// <summary>
        /// Tests how much the BitArray can be compressed using gzip.
        /// </summary>
        /// <param name="b">Vector to rate.</param>
        /// <returns>Ratio between new and old size.</returns>
        public static double CompressionTest(BitArray b)
        {
            int length = (int)Math.Ceiling((double)b.Length / 8);
            byte[] array = new byte[length];
            b.CopyTo(array, 0);
            var ms = new MemoryStream();
            Stream gzip = new GZipStream(ms, CompressionLevel.Optimal);
            gzip.Write(array, 0, length);
            gzip.Dispose();
            int compressedSize = ms.ToArray().Length;
            return (double)(compressedSize - 30) / length;
        }

        /// <summary>
        /// Rates how much the BitArray is pseudorandom.
        /// Return the average between <c>CompressionTest(b)</c> and <c>EntropyTest(b, 10).</c>
        /// </summary>
        /// <param name="sequence">Vector to rate.</param>
        /// <returns>Number between 0 and 1. Only values very close to 1 are good.</returns>
        public static double RateSequence(BitArray sequence)
        {
            byte lengthLimit = 10;
            double entropyResult = EntropyTest(sequence, lengthLimit);
            double compressionResult = CompressionTest(sequence);
            return (entropyResult + compressionResult) / 2;
        }
    }
}
