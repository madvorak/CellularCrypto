using System;
using Crypto;
using Cellular;

namespace Testing
{
    static class CryptoTest
    {
        public static void RunTest()
        {
            BinaryCA ba = new BasicAutomaton(30, 128, 1234567);
            SimpleGen generator = new SimpleGen(ba);
            uint[] generatedSeq = generator.Generate(100);
            foreach (uint u in generatedSeq) Console.WriteLine(u);
            Console.ReadKey();
        }
    }
}
