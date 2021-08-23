using System;
using System.Collections;
using System.Collections.Generic;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Class for testing properties of key extending algorithms.
    /// </summary>
    class FunctionTesting
    {
        /// <summary>
        /// "Pointer" to a distance function which can be used to determine
        /// how much different two BitArrays of the same length are.
        /// </summary>
        /// <param name="u">First vector.</param>
        /// <param name="v">Second vector.</param>
        /// <returns>Number between 0 and 1.</returns>
        private delegate double DistanceDelegate(BitArray u, BitArray v);
        private DistanceDelegate distanceFunction;

        /// <summary>
        /// Creates a new instance of FunctionTesting.
        /// </summary>
        /// <param name="useLevenshteinDistance">Should it use Levenshtein distance instead of Hamming distance?
        /// Note that Levenshtein distance is much slower to calculate. Default is Hamming distance.</param>
        public FunctionTesting(bool useLevenshteinDistance = false)
        {
            if (useLevenshteinDistance)
            {
                distanceFunction = LevensteinDistance;
            }
            else
            {
                distanceFunction = HammingDistance;
            }
        }

        /// <summary>
        /// Calculates the Hamming distance of two BitArrays of the same length.
        /// </summary>
        /// <param name="u">First vector.</param>
        /// <param name="v">Second vector.</param>
        /// <returns>Number between 0 and 1.</returns>
        public static double HammingDistance(BitArray u, BitArray v)
        {
            if (u.Length != v.Length)
            {
                throw new ArgumentException("Both arrays must be of the same length!");
            }
            int diffCount = 0;
            for (int i = 0; i < u.Length; i++)
            {
                if (u[i] != v[i])
                {
                    diffCount++;
                }
            }
            return (double)diffCount / u.Length;
        }

        /// <summary>
        /// Calculates the Levenshtein distance of two BitArrays of the same length.
        /// Sources: 
        /// https://en.wikipedia.org/wiki/Levenshtein_distance ,
        /// https://en.wikibooks.org/wiki/Algorithm_Implementation/Strings/Levenshtein_distance#C.23
        /// </summary>
        /// <param name="u">First vector.</param>
        /// <param name="v">Second vector.</param>
        /// <returns>Number between 0 and 1.</returns>
        public static double LevensteinDistance(BitArray u, BitArray v)
        {
            if (u.Length != v.Length)
            {
                throw new ArgumentException("Both arrays must be of the same length!");
            }

            int upper = u.Length;

            int[,] d = new int[upper + 1, upper + 1];

            for (int i = 0; i <= upper; i++)
            {
                d[i, 0] = i;
            }

            for (int i = 0; i <= upper; i++)
            {
                d[0, i] = i;
            }

            for (int i = 1; i <= upper; i++)
            {
                for (int j = 1; j <= upper; j++)
                {
                    int cost = u[i - 1] == v[j - 1] ? 0 : 1;

                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return (double)d[upper, upper] / upper;
        }

        public bool isLevenshteinInside()
        {
            return (distanceFunction == LevensteinDistance);
        }

        /// <summary>
        /// Tests how much the output changes (on average) when the input changes in a single bit.
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <param name="ratio"></param>
        /// <returns>Value between 0 and 1. Both extrema are very bad, good result is 1/2.</returns>
        public double TestBitChange(IKeyExtender algorithm, int ratio)
        {
            const int length = 100;
            const int repetitions = 5;
            double sum = 0;
            for (int i = 0; i < repetitions; i++)
            {
                BitArray x = Utilities.RandomBitArr(length);
                BitArray y = new BitArray(x);
                BitArray result;
                try
                {
                    result = algorithm.ExtendKey(x, length * ratio);
                }
                catch (CannotGenerateException)
                {
                    continue;
                }
                for (int j = 0; j < length; j++)
                {
                    y[j] = !y[j];
                    try
                    {
                        sum += distanceFunction(result, algorithm.ExtendKey(y, length * ratio));
                    }
                    catch (CannotGenerateException) { }
                    y[j] = !y[j];
                }
            }
            return sum / (length * repetitions);
        }

        /// <summary>
        /// Tests average distance between two random results.
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <param name="ratio"></param>
        /// <returns>Value between 0 and 1. Both extrema are very bad, good result is 1/2.</returns>
        public double TestAverageDistance(IKeyExtender algorithm, int ratio)
        {
            const int length = 150;
            const int count = 50;
            BitArray[] samples = new BitArray[count];
            BitArray[] results = new BitArray[count];
            for (int i = 0; i < count; i++)
            {
                newArray:
                samples[i] = Utilities.RandomBitArr(length);
                for (int j = 0; j < i; j++)
                {
                    if (distanceFunction(samples[i], samples[j]) < 0.005)
                    {
                        goto newArray;
                    }
                }
                try
                {
                    results[i] = algorithm.ExtendKey(samples[i], length * ratio);
                }
                catch (CannotGenerateException)
                {
                    return 0;
                }
            }

            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    sum += distanceFunction(results[i], results[j]);
                }
            }

            return sum / ((count * (count - 1)) / 2);
        }

        public double TestLargestBallExactly(IKeyExtender algorithm)
        {
            const int length = 8;
            List<BitArray> results = new List<BitArray>();
            foreach (BitArray b in new Utilities.AllBinarySequences(length))
            {
                try
                {
                    results.Add(algorithm.DoubleKey(b));
                }
                catch (CannotGenerateException) { }
            }

            double largestRadius = 0;
            foreach (BitArray point in new Utilities.AllBinarySequences(2 * length))
            {
                double minDist = 1;
                foreach (BitArray result in results)
                {
                    double distance = distanceFunction(point, result);
                    if (distance < minDist)
                    {
                        minDist = distance;
                        if (minDist <= largestRadius)
                        {
                            break;
                        }
                    }
                }
                if (minDist > largestRadius)
                {
                    largestRadius = minDist;
                }
            }

            return largestRadius;
        }

        public double TestLargestBallApprox(IKeyExtender algorithm)
        {
            const int length = 100;
            const int count = 200;
            const int samples = 2000;
            List<BitArray> results = new List<BitArray>();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    results.Add(algorithm.DoubleKey(Utilities.RandomBitArr(length)));
                }
                catch (CannotGenerateException) { }
            }

            double largestRadius = 0;
            for (int i = 0; i < samples; i++)
            {
                BitArray point = Utilities.RandomBitArr(2 * length);
                double minDist = 1;
                foreach (BitArray result in results)
                {
                    double distance = distanceFunction(point, result);
                    if (distance < minDist)
                    {
                        minDist = distance;
                        if (minDist <= largestRadius)
                        {
                            break;
                        }
                    }
                }
                if (minDist > largestRadius)
                {
                    largestRadius = minDist;
                }
            }

            return largestRadius;
        }

        private delegate double RandomnessDelegate(BitArray b); 

        private double rateRandomSequences(IKeyExtender algorithm, int ratio, RandomnessDelegate method)
        {
            const int length = 500;
            const int count = 5;
            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    sum += method.Invoke(algorithm.ExtendKey(Utilities.RandomBitArr(length), length * ratio));
                }
                catch (CannotGenerateException) { }
            }
            return sum / count;
        }

        /// <summary>
        /// Tests how much a random output resembles a pseudo-random binary sequence (on random samples).
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <param name="ratio"></param>
        /// <returns>Value between 0 (worst) and 1 (good).</returns>
        public double TestRandomSequences(IKeyExtender algorithm, int ratio)
        {
            return rateRandomSequences(algorithm, ratio, RandomnessTesting.RateSequence);
        }

        public double TestRandomEntropy(IKeyExtender algorithm, int ratio)
        {
            return rateRandomSequences(algorithm, ratio, b => RandomnessTesting.EntropyTest(b, 16));
        }

        public double TestRandomCompression(IKeyExtender algorithm, int ratio)
        {
            return rateRandomSequences(algorithm, ratio, RandomnessTesting.CompressionTest);
        }

        private double rateSystematicSequences(IKeyExtender algorithm, int ratio, RandomnessDelegate method)
        {
            const int length = 10;
            double sum = 0;
            int count = 0;
            foreach (BitArray b in new Utilities.AllBinarySequences(length))
            {
                try
                {
                    sum += method.Invoke(algorithm.ExtendKey(b, length * ratio));
                }
                catch (CannotGenerateException) { }
                count++;
            }
            return sum / count;
        }

        /// <summary>
        /// Tests how much a random output resembles a pseudo-random binary sequence
        /// (on sequences resulting from expanding all very short keys).
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <param name="ratio"></param>
        /// <returns>Value between 0 (worst) and 1 (good).</returns>
        public double TestSystematicSequences(IKeyExtender algorithm, int ratio)
        {
            return rateSystematicSequences(algorithm, ratio, RandomnessTesting.RateSequence);
        }

        public double TestSystematicEntropy(IKeyExtender algorithm, int ratio)
        {
            return rateSystematicSequences(algorithm, ratio, b => RandomnessTesting.EntropyTest(b, 10));
        }

        public double TestSystematicCompression(IKeyExtender algorithm, int ratio)
        {
            return rateSystematicSequences(algorithm, ratio, RandomnessTesting.CompressionTest);
        }
    }
}
