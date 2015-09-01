using System;

namespace Crypto
{
    class SimpleGen
    {
        private Cellular.BinaryCA ca;           //the size of the inner CA must be at least 32
                                                //ideally, the size should be a multiple of 32
        public SimpleGen(Cellular.BinaryCA CA)
        {
            ca = CA;
        }

        public uint[] Generate(uint chunkCount)
        {
            uint[] array = new uint[chunkCount];
            int readSize = ca.GetSize() / 32;
            if (readSize < 1) throw new InvalidOperationException("The size of the underlying CA is not sufficient.");

            int index = 0;
            while (index + readSize < chunkCount)
            {
                ca.Step();
                uint[] append = ca.GetPacked();
                Array.Copy(append, 0, array, index, readSize);
                index += readSize;
            }
            uint[] rest = ca.GetPacked();
            Array.Copy(rest, 0, array, index, chunkCount - index);

            return array;               // TODO : test absence of period
        }
    }
}
