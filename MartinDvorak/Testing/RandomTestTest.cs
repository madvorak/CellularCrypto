using System;
using System.Collections;
using Crypto;
using Cellular;

namespace Testing
{
    class RandomTestTest
    {
        private static void TestBitArray(BitArray b)
        {
            Console.WriteLine("  Entropy test 3 max length: " + RandomnessTesting.EntropyTest(b, 3));
            Console.WriteLine("  Entropy test 6 max length: " + RandomnessTesting.EntropyTest(b, 6));
            Console.WriteLine("  Compression Test: " + RandomnessTesting.CompressionTest(b));
            Console.WriteLine("  Combined Test: " + RandomnessTesting.RateSequence(b));
        }

        public static void RunTest()
        {
            int size = 100000;
            BitArray b = new BitArray(size);
            for (int i = 0; i < size; i++) b[i] = true;
            Console.WriteLine("Samé 1");
            TestBitArray(b);
            for (int i = 0; i < size; i++) b[i] = false;
            Console.WriteLine("Samé 0");
            TestBitArray(b);
            for (int i = 0; i < size; i++) b[i] = i % 2 == 0;
            Console.WriteLine("Každá druhá 1");
            TestBitArray(b);
            for (int i = 0; i < size; i++) b[i] = i % 5 == 0;
            Console.WriteLine("Každá pátá 1");
            TestBitArray(b);
            Random r = Program.rnd;
            for (int i = 0; i < size; i++) b[i] = r.Next(2) == 1;
            Console.WriteLine("Náhodně s p 1/2");
            TestBitArray(b);
            for (int i = 0; i < size; i++) b[i] = r.Next(3) == 1;
            Console.WriteLine("Náhodně s p 1/3");
            TestBitArray(b);
            for (int i = 0; i < size; i++) b[i] = (i%2 == 0) ? true : (r.Next(2) == 1);
            Console.WriteLine("Sudé 1, liché náhodně s p 1/2");
            TestBitArray(b);
            for (int i = 0; i < size; i++) b[i] = (i % 2 == 0) ? (i % 4 == 0) : (r.Next(2) == 1);
            Console.WriteLine("Sudé střídavě, liché náhodně s p 1/2");
            TestBitArray(b);

            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
