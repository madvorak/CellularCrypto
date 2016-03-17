using System;
using System.Collections;
using Cellular;

namespace Crypto
{
    class KeyExtenderUncertain : KeyExtenderAbstractN
    {
        private readonly IBinaryCA ca;

        public KeyExtenderUncertain(IBinaryCA binaryCA)
        {
            ca = binaryCA.CloneEverything();
        }

        public override BitArray ExtendKey(BitArray shortKey, int targetLength)
        {
            InnerGenerator generator = new InnerGenerator(ca.CloneTemplate(shortKey));
            BitArray longKey = new BitArray(targetLength);
            int index = 0;
            while (index < targetLength)
            {
                bool bitL = generator.GetNext();
                bool bitR = generator.GetNext();
                if (bitL && !bitR)
                {
                    longKey[index++] = true;
                }
                if (!bitL && bitR)
                {
                    longKey[index++] = false;
                }
            }
            return longKey;
        }

        private class InnerGenerator
        {
            private IBinaryCA innerCA;
            private int position;
            private int size;

            public InnerGenerator(IBinaryCA generatorCA)
            {
                innerCA = generatorCA;
                innerCA.Step();
                position = 0;
                size = innerCA.GetSize();
            }

            public bool GetNext()
            {
                if (position < size)
                {
                    return innerCA.GetValueAt(position++);
                }
                else
                {
                    int hash = innerCA.GetHashCode();
                    innerCA.Step();
                    if (hash == innerCA.GetHashCode())
                    {
                        innerCA.Step();
                        if (hash == innerCA.GetHashCode())
                        {
                            // the state of the CA no longer changes
                            throw new CannotGenerateException();
                        }
                    }
                    position = 1;
                    return innerCA.GetValueAt(0);
                }
            }
        }
    }
}
