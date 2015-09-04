using System;
using Crypto;
using Cellular;

namespace Testing
{
    [Obsolete]
    static class CryptoTest
    {
        public static void RunTest()
        {
            IBinaryCA ba = new ElementaryAutomaton(30, 128, Program.rnd);
            SimpleGen generator = new SimpleGen(ba);
            uint[] generatedSeq = generator.Generate(100);
            foreach (uint u in generatedSeq) Console.WriteLine(u);
            Console.ReadKey();
        }
    }
}
