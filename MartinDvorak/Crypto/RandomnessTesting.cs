using System;
using System.Collections;
using System.IO;
using System.IO.Compression;

namespace Crypto
{
    static class RandomnessTesting
    {
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
            return score / maxScore;   //weighted average of entropies for block from 1 to length; actual vs maximal
        }

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
            return (double)(compressedSize - 30) / (double)length;
        }

        public static double RateSequence(BitArray sequence)
        {
            byte lengthLimit = 10;
            double entropyResult = EntropyTest(sequence, lengthLimit);
            double compressionResult = CompressionTest(sequence);
            return (entropyResult + compressionResult) / 2;
        }
    }
}
