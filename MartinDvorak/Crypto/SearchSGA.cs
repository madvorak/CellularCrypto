using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using Cellular;

namespace Crypto
{
    static class SearchSGA
    {
        public static void SearchForGoodExtenders()
        {
            while (true)
            {
                KeyExtenderGenetic genetic = new KeyExtenderGenetic();
                BitArray input = Utilities.RandomBitArr(50);
                BitArray output = genetic.ExtendKey(input, 6000);
                File.WriteAllText(DateTime.Now.ToFileTime() + ".xca", 
                    RandomnessTesting.RateSequence(output).ToString() + "\n\n" +  genetic.getInfoAboutWinner());
            }
        }
    }
}
