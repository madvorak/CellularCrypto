using System.Text;

namespace Crypto
{
    /// <summary>
    /// Wrapper for class <c>FunctionTesting</c>. It generates results in the form, which is suitable for my thesis.
    /// </summary>
    class FunctionTestsForThesis
    {
        private FunctionTesting tester;
        private double mean;

        /// <summary>
        /// If you use this constructor, you are able to inject Levenshtein Distance.
        /// </summary>
        public FunctionTestsForThesis(FunctionTesting functionTesting)
        {
            tester = functionTesting;
            if (tester.isLevenshteinInside())
            {
                mean = 0.29;
            }
            else
            {
                mean = 0.5;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FunctionTestsForThesis()
        {
            tester = new FunctionTesting();
        }

        /// <summary>
        /// Rescales 0.0 0.1 0.2 0.3 0.4 0.5 0.6 0.7 0.8 0.9 1.0
        /// Onto:    0.0 0.2 0.4 0.6 0.8 1.0 0.8 0.6 0.4 0.2 0.0
        /// </summary>
        private double rescale(double value)
        {
            if (value <= mean)
            {
                return value / mean;
            }
            else
            {
                return (1 - value) / (1 - mean);
            }
        }

        /// <summary>
        /// Tests how much the output changes (on average) when the input changes in a single bit.
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <returns>Value between 0 (worst) and 1 (good).</returns>
        public double TestBitChange(IKeyExtender algorithm)
        {
            double result = tester.TestBitChange(algorithm, 7);
            return rescale(result);
        }

        /// <summary>
        /// Tests average distance between two random results.
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <returns>Value between 0 (worst) and 1 (good).</returns>
        public double TestAverageDistance(IKeyExtender algorithm)
        {
            double result = tester.TestAverageDistance(algorithm, 7);
            return rescale(result);
        }

        /// <summary>
        /// Estimates the size of the largest ball that can be put into the space of long (output) keys
        /// without containing any key. 
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <returns>Value between 0 (worst) and 1 (good).</returns>
        public double TestLargestBall(IKeyExtender algorithm)
        {
            double result = tester.TestLargestBallExactly(algorithm) + tester.TestLargestBallApprox(algorithm);
            return 1 - ((result - 0.6) / 0.4);
        }

        /// <summary>
        /// Estimates the average Shannon entropy of an output.
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <returns>Value between 0 (worst) and 1 (good).</returns>
        public double TestEntropy(IKeyExtender algorithm)
        {
            double result = tester.TestSystematicEntropy(algorithm, 500) + tester.TestRandomEntropy(algorithm, 60);
            return result / 2;
        }

        /// <summary>
        /// Tests how much an output can be compressed (on average). Optimal compression level gzip is used.
        /// Estimates the upper bound of Kolmogorov complexity.
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <returns>Value between 0 (worst) and 1 (good).</returns>
        public double TestCompression(IKeyExtender algorithm)
        {
            double result = tester.TestSystematicCompression(algorithm, 500) + tester.TestRandomCompression(algorithm, 60);
            return result / 2;
        }

        /// <summary>
        /// Runs all customized tests (Entropy, Compression, AverageDistance, BitChange, LargestBall) on one algorithm
        /// and prints the results.
        /// </summary>
        /// <param name="algorithm">The (key stretching) algorithm to test.</param>
        /// <returns>Results on 5 text lines.</returns>
        public string CalculateAndPrintResults(IKeyExtender algorithm)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("   TestEntropy        : {0:0.000}", TestEntropy(algorithm)));
            sb.AppendLine(string.Format("   TestCompression    : {0:0.000}", TestCompression(algorithm)));
            sb.AppendLine(string.Format("   TestAverageDistance: {0:0.000}", TestAverageDistance(algorithm)));
            sb.AppendLine(string.Format("   TestBitChange      : {0:0.000}", TestBitChange(algorithm)));
            sb.AppendLine(string.Format("   TestLargestBall    : {0:0.000}", TestLargestBall(algorithm)));
            return sb.ToString();
        }
    }
}
