using System;
using Crypto;

namespace Testing
{
    /// <summary>
    /// This test demonstrates how <c>Crypto.Factory</c> works.
    /// </summary>
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
