using System;
using System.Collections.Generic;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Basic store saving CA templates and masks for generating sequences.
    /// There is at most one instance for each size (how large CA will be created using given data).
    /// </summary>
    class Primitives
    {
        private static Dictionary<int, Primitives> instances;

        private static IBinaryCA[] CAinventory;     // "Singleton" object, one for all sizes
        private Mask[] maskBase;                    // collection of Masks adapted to the size

        private Primitives(int size)
        {
            if (CAinventory == null)
            { 
                // constructor is called the first time
                CAinventory = new IBinaryCA[600];
                for (int i = 0; i < 256; i++)
                {
                    CAinventory[i] = new ElementaryFastAutomaton((byte)i, 1);
                }
                for (int i = 256; i < 427; i++)
                {
                    bool[] rule = Utilities.RandomBoolArr(32);
                    CAinventory[i] = new BinaryRangeAutomaton(2, rule, 1);
                }
                for (int i = 427; i < 599; i++)
                {
                    bool[] rule = Utilities.RandomBoolArr(32);
                    CAinventory[i] = new BinaryRangeCyclicAutomaton(2, rule, 1);
                }
                CAinventory[599] = new GameOfLife(1, 1);
            }

            maskBase = new Mask[10];
            maskBase[0] = new Mask(new int[] { size / 2 });
            for (int i = 1; i < 10; i++)
            {
                int[] frames = new int[i + 1];
                for (int j = 0; j <= i; j++)
                {
                    frames[j] = Program.rnd.Next(size);
                }
                maskBase[i] = new Mask(frames);
            }
        }

        /// <summary>
        /// Method used instead of the constructor, similar to the principle of singleton.
        /// </summary>
        /// <param name="size">Size of the CA we want to work with.</param>
        /// <returns>Instance of this class for given size.</returns>
        public static Primitives GetInstance(int size)
        {
            if (instances.ContainsKey(size))
            {
                return instances[size];
            }
            else
            {
                Primitives p = new Primitives(size);
                instances.Add(size, p);
                return p;
            }
        }

        /// <summary>
        /// Gives a random template for creating a new CA.
        /// </summary>
        /// <returns>IBinaryCA template.</returns>
        public IBinaryCA RandomCA()
        {
            return CAinventory[Program.rnd.Next(CAinventory.Length)];
        }

        /// <summary>
        /// Gives a random mask to be used to generate data by a CA.
        /// </summary>
        /// <returns>Mask from collection adapted to the size.</returns>
        public Mask RandomMask()
        {
            return maskBase[Program.rnd.Next(maskBase.Length)];
        }
    }
}
