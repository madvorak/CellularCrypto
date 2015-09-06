using System;
using System.Collections;

namespace Cellular
{
    /// <summary>
    /// Class representing 256 elementary CA with firmly set borders.
    /// This specific implementation maps 10 cells onto 8 cells at once, so it should be faster a little.
    /// </summary>
    class ElementaryFastAutomaton : ElementaryAutomaton
    {
        private bool[][] lookupT;

        /// <summary>
        /// Creates a new basic CA of size 100 with 000...00100...000 as its initial state (faster variant).
        /// The new CA will use rule No.30 : asymmetric, pseudo-chaotic behaviour.
        /// </summary>
        public ElementaryFastAutomaton(): base() 
        {
            fillDictionary();
        }

        /// <summary>
        /// Creates a new basic CA with 000...00100...000 as its initial state (faster variant).
        /// The new CA will use rule No.30 : asymmetric, pseudo-chaotic behaviour.
        /// </summary>
        /// <param name="size">The size of the new CA.</param>
        public ElementaryFastAutomaton(int size) : base(30, size) 
        {
            fillDictionary();
        }

        /// <summary>
        /// Creates a new basic CA with given rule and 000...00100...000 as its initial state (faster variant).
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="size">The size of the new CA.</param>
        public ElementaryFastAutomaton(byte ruleNo, int size) : base(ruleNo, size)  
        {
            fillDictionary();
        }

        /// <summary>
        /// Creates a new basic CA with given rule and initial state (faster variant).
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="initialState">A <c>BitArray</c> describing the initial state of the CA.
        /// This also determines the size of the new CA.</param>
        public ElementaryFastAutomaton(byte ruleNo, BitArray initialState) : base(ruleNo, initialState)  
        {
            fillDictionary();
        }

        /// <summary>
        /// Creates a new basic CA (faster variant).
        /// </summary>
        /// <param name="ruleNo">The code of the elementary rule (from 0 to 255).</param>
        /// <param name="size">The size of the new CA.</param>
        /// <param name="rnd">PseudoRNG instance that will be used to generate the original state.</param>
        public ElementaryFastAutomaton(byte ruleNo, int size, Random rnd) : base(ruleNo, size, rnd)  
        {
            fillDictionary();
        }

        public override void Step()
        {
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
                for (int j = 0; j < 8; j++)
                {
                    newState[i + j] = lookupT[old10][j];
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

        private void fillDictionary()
        {
            lookupT = new bool[1024][];
            for (int i = 0; i < 1024; i++)
            {
                byte[] tenCells = new byte[10];
                int x = i;
                for (int j = 9; j >= 0; j--)
                {
                    tenCells[j] = (byte)(x % 2);
                    x /= 2;
                }                                   //tenCells now contains a binary representation of i
                lookupT[i] = new bool[8];
                for (int j = 0; j < 8; j++)
                {
                    lookupT[i][j] = rule[tenCells[j], tenCells[j + 1], tenCells[j + 2]];
                }
            }
        }

        public override object Clone()
        {
            return new ElementaryFastAutomaton(ruleNumber, state);
        }

        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            return new ElementaryFastAutomaton(ruleNumber, newInstanceState);
        }
    }
}
