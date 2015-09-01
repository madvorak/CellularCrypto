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
            for (byte length = 1; length <= lengthLimit; length++)
            {
                int seqCount = 1 << length;
                int sampleCount = b.Length / length;
                double[] frequencies = new double[seqCount];
                for (int j = 0; j < seqCount; j++) frequencies[j] = 0d;
                for (int j = 0; j < sampleCount; j++)
                {
                    int index = 0;
                    for (int k = 0; k < length; k++)
                    {
                        index *= 2;
                        if (b[j*length + k]) index++;
                    }
                    frequencies[index] += 1d;
                }
                for (int j = 0; j < seqCount; j++) frequencies[j] /= sampleCount;
                for (int j = 0; j < seqCount; j++)
                {
                    double freq = frequencies[j];
                    if (freq > 0) score += freq * Math.Log(1 / freq, 2);
                }
            }
            return score;   //sum of entropies for parts of length from 1 to lengthLimit
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
            return (double)(compressedSize - 20) / (double)length;
        }

        public static double RateSequence(BitArray sequence)
        {
            byte lengthLimit = 10;
            double maxResult = (double)(lengthLimit + 1) * lengthLimit / 2;
            double entropyResult = EntropyTest(sequence, lengthLimit) / maxResult;
            double compressionResult = CompressionTest(sequence);
            return (entropyResult + compressionResult) / 2;
        }
    }
}
