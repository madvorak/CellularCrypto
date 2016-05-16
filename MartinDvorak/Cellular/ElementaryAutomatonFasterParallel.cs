using System;
using System.Collections;
using System.Threading;

namespace Cellular
{
    class ElementaryAutomatonFasterParallel : ElementaryAutomatonFaster
    {
        private int threadCount;

        public ElementaryAutomatonFasterParallel(byte ruleNo, BitArray initialState) : base(ruleNo, initialState)
        {
            countProcessors();
        }

        public ElementaryAutomatonFasterParallel(byte ruleNo, int size) : base(ruleNo, size)
        {
            countProcessors();
        }

        private void countProcessors()
        {
            threadCount = Environment.ProcessorCount;
        }

        public override void Step()
        {
            // TODO: threading
            base.Step();
        }

        public override object Clone()
        {
            return new ElementaryAutomatonFasterParallel(ruleNumber, state);
        }

        protected override IBinaryCA cloneTemplate(BitArray newInstanceState)
        {
            return new ElementaryAutomatonFasterParallel(ruleNumber, newInstanceState);
        }
    }
}
