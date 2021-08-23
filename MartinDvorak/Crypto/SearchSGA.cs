using System;
using System.Collections;
using System.IO;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Static class that can be used to pre-generate good key extenders for the genetic algorithm.
    /// </summary>
    static class SearchSGA
    {
        /// <summary>
        /// Infinite loop of KeyExtenderGenetic for collecting data about good key extending primitives.
        /// Generates text output into the same directory.
        /// </summary>
        public static void SearchForGoodExtenders()
        {
            while (true)
            {
                KeyExtenderGenetic genetic = new KeyExtenderGenetic();
                BitArray input = Utilities.RandomBitArr(100);
                BitArray output = genetic.ExtendKey(input, 25000);
                File.WriteAllText(DateTime.Now.ToFileTime() + ".xca", 
                    RandomnessTesting.RateSequence(output).ToString() + "\n\n" +  genetic.getInfoAboutWinner());
                Console.WriteLine("Done.");
            }
        }
    }
}
