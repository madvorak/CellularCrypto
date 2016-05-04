using System;
using System.Collections;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Class for symmetric cryptography performed by a reversible CA.
    /// </summary>
    class EncrypterReversibleCA : IEncrypter
    {
        private int stepCount;

        /// <summary>
        /// Creates a new <c>EncrypterReversibleCA</c>.
        /// </summary>
        /// <param name="stepsToPerform">How many steps of the CA should be performed
        /// during every encryption / decryption.</param>
        public EncrypterReversibleCA(int stepsToPerform)
        {
            if (stepsToPerform < 2)
            {
                throw new ArgumentException("Insufficient number of steps!");
            }
            stepCount = stepsToPerform;
        }

        /// <summary>
        /// Performs encryption of a message using a reversible cellular automaton. Can be used for decryption as well.
        /// </summary>
        /// <param name="message">Plaintext / message to encrypt.</param>
        /// <param name="key">Allowed key sizes: 8, 32, 128, 512, 2048, ... (odd power of two).</param>
        /// <returns>Ciphertext / encrypted message.</returns>
        public BitArray Encrypt(BitArray message, BitArray key)
        {
            int halfLength;
            if (message.Length % 2 == 0)
            {
                halfLength = message.Length / 2;
            }
            else
            {
                halfLength = message.Length / 2 + 1;
            }
            BitArray firstHalf = new BitArray(halfLength);
            for (int i = 0; i < halfLength; i++)
            {
                firstHalf[i] = message[i];
            }
            BitArray secondHalf = new BitArray(halfLength);
            for (int i = 0; i < halfLength; i++)
            {
                try
                {
                    secondHalf[i] = message[halfLength + i];
                }
                catch (IndexOutOfRangeException)
                {
                    secondHalf[i] = false;
                }
            }

            byte range = (byte)((((int)Math.Round(Math.Log(key.Length, 2))) - 1) / 2);
            IBinaryCA ca = new ReversibleAutomaton(range, key, firstHalf, secondHalf);
            for (int i = 0; i < stepCount; i++)
            {
                ca.Step();
            }
            BitArray result = new BitArray(2 * halfLength);
            // the same as message.Length if pair (+1 if odd ... doesn't happen for bytes)
            for (int i = 0; i < halfLength; i++)
            {
                result[halfLength + i] = ca.GetValueAt(i);
            }
            ca.Step();
            // first half of the result comes from the last state for symmetry of encryption / decryption
            for (int i = 0; i < halfLength; i++)
            {
                result[i] = ca.GetValueAt(i);
            }
            return result;
        }
    }
}
