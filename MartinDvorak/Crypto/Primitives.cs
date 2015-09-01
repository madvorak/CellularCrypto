using System;

namespace Crypto
{
    class Primitives        // Singleton class
    {
        // TODO: should be made flexible accoring to size of key
        private static Primitives instance;

        private string[] CAcodes;
        private int CAcodeCount;
        private Mask[] maskBase;
        private int maskCount;
        private const int maskLimit = 1 << 18;
        private Random r;

        private Primitives()
        {
            CAcodeCount = 300;
            CAcodes = new string[CAcodeCount];
            for (int i = 0; i < 256; i++) CAcodes[i] = "B " + i.ToString();
            for (int i = 256; i < CAcodeCount; i++) CAcodes[i] = "G 0";

            maskCount = 200;
            maskBase = new Mask[maskCount];
            maskBase[0] = new Mask(new int[] { 123456 });      //constant mask
            for (int i = 1; i < maskCount / 2; i++)
            {
                int[] asc = new int[maskLimit];
                for (int j = 0; j < maskLimit; j++)
                {
                    asc[i] = (i*j) % maskLimit;
                }
                maskBase[i] = new Mask(asc);             //linearly ascending mask
            }
            r = Program.rnd;
            for (int i = maskCount / 2; i < maskCount; i++)
            {
                int[] rand = new int[i * 100];
                for (int j = 0; j < rand.Length; j++)
                {
                    rand[j] = (int) r.Next(maskLimit);
                }
                maskBase[i] = new Mask(rand);             //pseudo-random mask
            }
        }

        public static Primitives Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Primitives();
                }
                return instance;
            }
        }

        public string RandomCAcode()
        {
            return CAcodes[r.Next(CAcodeCount)];
        }

        public Mask RandomMask()
        {
            return maskBase[r.Next(maskCount)];
        }
    }
}
