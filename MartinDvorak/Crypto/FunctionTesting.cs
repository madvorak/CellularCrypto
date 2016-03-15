using System;
using System.Collections;
using System.Collections.Generic;
using Cellular;

namespace Crypto
{
    class FunctionTesting
    {
        public delegate double DistanceDelegate(BitArray u, BitArray v);
        private DistanceDelegate distanceFunction;

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
        /// Sources: https://en.wikipedia.org/wiki/Levenshtein_distance ,
        /// https://en.wikibooks.org/wiki/Algorithm_Implementation/Strings/Levenshtein_distance#C.23
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
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
                    int cost = u[i - 1] == v[i - 1] ? 0 : 1;

                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return (double)d[upper, upper] / upper;
        }

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

        public double TestRandomSequences(IKeyExtender algorithm, int ratio)
        {
            const int length = 500;
            const int count = 5;
            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    sum += RandomnessTesting.RateSequence(algorithm.ExtendKey(Utilities.RandomBitArr(length), length * ratio));
                }
                catch (CannotGenerateException) { }
            }
            return sum / count;
        } 

        public double TestSystematicSequences(IKeyExtender algorithm, int ratio)
        {
            const int length = 10;
            double sum = 0;
            int count = 0;
            foreach (BitArray b in new Utilities.AllBinarySequences(length))
            {
                try
                {
                    sum += RandomnessTesting.RateSequence(algorithm.ExtendKey(b, length * ratio));
                }
                catch (CannotGenerateException) { }
                count++;
            }
            return sum / count;
        }
    }
}
