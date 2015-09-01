using System;
using System.Collections;
using System.Collections.Generic;
using Cellular;

namespace Crypto
{
    class Solver
    {
        private BitArray input;
        private Primitives primitives;
        private Solution[] population;
        private int populationSize = 1000;
        private int algorithmLength = 5;
        private Solution finalAlgorithm;
        
        public Solver(uint inputSize)
        {
            input = Utilities.RandomBitArr((int)inputSize, new Random());
            primitives = Primitives.Instance;

            population = new Solution[populationSize];
            Solution bestSoFar = null;
            for (int i = 0; i < populationSize; i++)
            {
                List<Couple> alg = new List<Couple>(algorithmLength);
                for (int j = 0; j < algorithmLength; j++)
                {
                    Couple c = new Couple(primitives.RandomCAcode(), primitives.RandomMask());
                    alg.Add(c);
                }
                population[i] = new Solution(alg);
                double rank = population[i].EvaluateSolution(input);
                if (bestSoFar == null) bestSoFar = population[i];
                else
                {
                    if (bestSoFar.GetRank() < rank) bestSoFar = population[i];
                }
            }

            // TODO: search
            finalAlgorithm = bestSoFar;
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
