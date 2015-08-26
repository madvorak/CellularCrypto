using System.Collections;

namespace Cellular
{
    /// <summary>
    /// This class can work with 256 elementary CA with firmly set borders.
    /// This specific implementation maps 10 cells onto 8 cells at once.
    /// </summary>
    class BasicAutomaton2 : BasicAutomaton
    {
        private byte[] dictionary;

        public BasicAutomaton2(): base() {}

        public BasicAutomaton2(int size) : base(30, size) {}

        public BasicAutomaton2(byte ruleNo, int size) : base(ruleNo, size) {}

        public BasicAutomaton2(byte ruleNo, BitArray initialState) : base(ruleNo, initialState) {}

        public BasicAutomaton2(byte ruleNo, int size, int seed) : base(ruleNo, size, seed) {}

        public override void Step()
        {
            if (dictionary == null) FillDictionary();
            BitArray newState = new BitArray(size);

            // the first cell
            newState[0] = rule[0, state[0] ? 1 : 0, state[1] ? 1 : 0];
            // blocks of 8 from 10 cells
            int i = 1;
            while (i + 8 < size)
            {
                ushort old10 = 0;
                for (int j = i - 1; j < i + 9; j++)
                {
                    old10 *= 2;
                    if (state[j]) old10++;
                }
                byte new8 = dictionary[old10];
                for (int j = i + 7; j >= i; j--)
                {
                    newState[j] = new8 % 2 == 1;
                    new8 /= 2;
                }
                i += 8;
            }
            // the remaining cells
            for (int j = i; j < size - 1; j++)
            {
                newState[j] = rule[state[j - 1] ? 1 : 0, state[j] ? 1 : 0, state[j + 1] ? 1 : 0];
            }
            // the last cell
            newState[size - 1] = rule[state[size - 2] ? 1 : 0, state[size - 1] ? 1 : 0, 0];

            state = newState;
            time++;
        }

        private void FillDictionary()
        {
            dictionary = new byte[1024];
            for (int i = 0; i < 1024; i++)
            {
                byte[] tenCells = new byte[10];
                int x = i;
                for (int j = 9; j >= 0; j--)
                {
                    tenCells[j] = (byte)(x % 2);
                    x /= 2;
                }                                   //tenCells now contains a binary representation of i
                byte value = 0;
                for (int j = 0; j < 8; j++)
                {
                    value *= 2;
                    if (rule[tenCells[j], tenCells[j + 1], tenCells[j + 2]]) value++;
                }
                dictionary[i] = value;
            }
        }
    }
}
