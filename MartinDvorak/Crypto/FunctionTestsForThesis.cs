using System.Text;

namespace Crypto
{
    class FunctionTestsForThesis
    {
        private FunctionTesting tester;

        public FunctionTestsForThesis(FunctionTesting functionTesting)
        {
            tester = functionTesting;
        }

        public FunctionTestsForThesis()
        {
            tester = new FunctionTesting();
        }

        private double rescale(double value)
        {
            if (value < 0.5)
            {
                return 2 * value;
            }
            else
            {
                return 2 * (1 - value);
            }
        }

        public double TestBitChange(IKeyExtender algorithm)
        {
            double result = tester.TestBitChange(algorithm, 7);
            return rescale(result);
        }

        public double TestAverageDistance(IKeyExtender algorithm)
        {
            double result = tester.TestAverageDistance(algorithm, 7);
            return rescale(result);
        }

        public double TestLargestBall(IKeyExtender algorithm)
        {
            double result = tester.TestLargestBallExactly(algorithm) + tester.TestLargestBallApprox(algorithm);
            return 1 - ((result - 0.6) / 0.4);
        }

        public double TestEntropy(IKeyExtender algorithm)
        {
            double result = tester.TestSystematicEntropy(algorithm, 500) + tester.TestRandomEntropy(algorithm, 60);
            return result / 2;
        }

        public double TestCompression(IKeyExtender algorithm)
        {
            double result = tester.TestSystematicCompression(algorithm, 500) + tester.TestRandomCompression(algorithm, 60);
            return result / 2;
        }

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
