using System;
using System.Collections;
using System.Collections.Generic;
using Cellular;

namespace Crypto
{
    struct Couple
    {
        public readonly IBinaryCA template;
        public readonly Mask mask;
        public Couple(IBinaryCA exampleCA, Mask frames)
        {
            template = exampleCA;
            mask = frames;
        }
    }

    /// <summary>
    /// that consists of doubling the key by every couple of CA and mask
    /// </summary>
    class Solution
    {
        private List<Couple> algorithm;
        private double rank;         // quality of the last generated key
        private int inputSize;

        /// <summary>
        /// Creates a new Solution.
        /// </summary>
        /// <param name="automataMaskSequence">Sequence of <c>IBinaryCA</c> with masks
        /// describing an algorithm for generating keys.</param>
        public Solution(List<Couple> automataMaskSequence, int futureInputSize)
        {
            algorithm = automataMaskSequence;
            inputSize = futureInputSize;
        }

        public BitArray RunSolution(BitArray initial)
        {
            if (initial.Length != inputSize)
            {
                throw new ArgumentException("The size of the actual input does not correspond to the declared input size.");
            }
            BitArray key = new BitArray(initial);
            IBinaryCA ca;
            SeqGen seqGen;
            Mask mask;
            foreach (Couple couple in algorithm)
            {
                ca = couple.template;
                mask = couple.mask;
                mask.Reset();
                seqGen = new SeqGen(ca, mask);
                key = seqGen.DoubleSize();
            }
            return key;
        }

        public double EvaluateSolution(BitArray initial)
        {
            BitArray sequence = RunSolution(initial);
            rank = RandomnessTesting.RateSequence(sequence);
            return rank;
        }

        public double GetRank()
        {
            return rank;
        }

        public Solution Mutate(double instability)
        {
            Solution changed = new Solution(new List<Couple>(algorithm), inputSize);
            int size = inputSize;
            for (int i = 0; i < algorithm.Count; i++)
            {
                Primitives primitives = Primitives.GetInstance(size);
                if (Program.rnd.NextDouble() < instability)
                {
                    // randomly changing the CA
                    changed.algorithm[i] = new Couple(primitives.RandomCA(), changed.algorithm[i].mask);
                }
                if (Program.rnd.NextDouble() < instability)
                {
                    // randomly changing the mask
                    changed.algorithm[i] = new Couple(changed.algorithm[i].template, primitives.RandomMask());
                }
                size *= 2;
            }
            return changed;
        }

        public Solution BreedWith(Solution parent)         //uniform breeding
        {
            Solution offspring = new Solution(new List<Couple>(algorithm), inputSize);
            for (int i = 0; i < algorithm.Count; i++)
            {
                if (Program.rnd.Next(2) == 1)
                {
                    offspring.algorithm[i] = parent.algorithm[i];
                }
            }
            return offspring;
        }

        public void WriteAutomata()
        {
            foreach (Couple couple in algorithm)
            {
                CellularAutomaton aut = (CellularAutomaton)couple.template;
                Console.WriteLine(aut.TellType());
            }
        }
    }
}
