using System;
using Crypto;

namespace Testing
{
    static class FactoryTest
    {
        public static void RunTest()
        {
            foreach (KeyExtenderAbstractD kead in Factory.GatherSuccessfulExtenders(@"c:\Martin\MFF\_baka\xtenderSearch\"))
            {
                Console.WriteLine(kead.GetInfo());
            }
        }
    }
}
