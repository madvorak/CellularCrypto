namespace Crypto
{
    class Mask
    {
        private int[] seq;
        private int next;

        public Mask(int[] mask)
        {
            seq = mask;
            next = 0;
        }

        public int GetIndex()
        {
            if (next >= seq.Length) next = 0;
            return seq[next++];
        }

        public int GetLength()
        {
            return seq.Length;
        }

        public void Reset()
        {
            next = 0;
        }
    }
}
