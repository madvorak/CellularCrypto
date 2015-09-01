using Cellular;
using System.Collections;

namespace Crypto
{
    class SeqGen
    {
        private BinaryCA automaton;
        private Mask mask;
        private int sizeCA;

        public SeqGen(BinaryCA automaton, Mask mask)
        {
            this.automaton = automaton;
            this.mask = mask;
            sizeCA = automaton.GetSize();
        }

        public bool OneBit()
        {
            automaton.Step();
            int index = mask.GetIndex();
            return automaton.GetValueAt(index % sizeCA);
        }

        public BitArray Generate(int length)
        {
            BitArray array = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = this.OneBit();
            }
            return array;
        }

        public BitArray DoubleSize()
        {
            return this.Generate(automaton.GetSize() * 2);
        }
    }
}
