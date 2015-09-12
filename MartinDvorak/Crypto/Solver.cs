using System;
using System.Collections;
using System.Collections.Generic;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Stub.
    /// </summary>
    class Solver
    {
        private BitArray input;
        private Solution[] population;
        private int populationSize = 1000;
        private int algorithmLength = 5;
        private Solution finalAlgorithm;
        
        public Solver(int inputSize)
        {
            // TODO
        }

        public BitArray GetKey()
        {
            return input;
            // TODO: algorithm should be encoded and added to the key
        }

        public BitArray Encrypt(BitArray plaintext)
        {
            throw new NotImplementedException();
        }

        public void WriteWinningAutomata()
        {
            finalAlgorithm.WriteAutomata();
        }
    }
}
