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

        public Solution BreedUniformWith(Solution parent)
        {
            if (this.algorithm.Count != parent.algorithm.Count)
            {
                throw new ArgumentException("You were going to breed two solutions of unequal lengths!");
            }
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

        public static Solution BreedUniform(Solution fst, Solution snd)
        {
            return fst.BreedUniformWith(snd);
        }

        public Solution BreedCrossoverWith(Solution parent)
        {
            if (this.algorithm.Count != parent.algorithm.Count)
            {
                throw new ArgumentException("You were going to breed two solutions of unequal lengths!");
            }
            List<Couple> crossbred = new List<Couple>(algorithm.Count);
            // at least one from this, at least one from the other
            int crossoverPoint = Program.rnd.Next(algorithm.Count - 1) + 1;
            for (int i = 0; i < crossoverPoint; i++)
            {
                crossbred[i] = this.algorithm[i];
            }
            for (int i = crossoverPoint; i < algorithm.Count; i++)
            {
                crossbred[i] = parent.algorithm[i];
            }
            return new Solution(crossbred, inputSize);
        }

        public static Solution BreedCrossover(Solution fst, Solution snd)
        {
            if (Program.rnd.Next(2) == 1)
            {
                return fst.BreedCrossoverWith(snd);
            }
            else
            {
                return snd.BreedCrossoverWith(fst);
            }
        }

        public static Solution BreedCrazy(Solution fst, Solution snd)
        {
            Solution result = null;
            if (Program.rnd.Next(2) == 1)
            {
                result = BreedCrossover(fst, snd);
            }
            else
            {
                result = BreedUniform(fst, snd);
            }
            result.Mutate(0.25d);
            return result;
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
