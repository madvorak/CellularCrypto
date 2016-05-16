using System.Collections;

namespace Cellular
{
    class ElementaryAutomatonFaster : ElementaryAutomaton
    {
        protected bool[] rule1D;

        public ElementaryAutomatonFaster(byte ruleNo, BitArray initialState) : base(ruleNo, initialState)
        {
            createRule1D();
        }

        public ElementaryAutomatonFaster(byte ruleNo, int size) : base(ruleNo, size)
        {
            createRule1D();
        }

        private void createRule1D()
        {
            rule1D = new bool[8];
            byte number = ruleNumber;
            for (int i = 0; i < 8; i++)
            {
                rule1D[i] = number % 2 == 1 ? true : false;
                number /= 2;
            }
        }

        public override void Step()
        {
            BitArray newState = new BitArray(size);

            int foo = state[0] ? 1 : 0;
            for (int i = 0; i < size - 1; i++)
            {
                foo = (foo << 1) & 7;
                if (state[i + 1]) foo++;
                newState[i] = rule1D[foo];
            }
            newState[size - 1] = rule1D[(foo << 1) & 7];

            state = newState;
            time++;
        }

        public override object Clone()
        {
            return new ElementaryAutomatonFaster(ruleNumber, state);
        }

        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            return new ElementaryAutomatonFaster(ruleNumber, newInstanceState);
        }
    }
}
