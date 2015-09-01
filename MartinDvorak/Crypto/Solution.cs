using System;
using System.Collections;
using System.Collections.Generic;
using Cellular;

namespace Crypto
{
    struct Couple
    {
        public string CAcode;       // "B 30" for Basic Automaton No. 30 ;  "G 0" for Game Of Life
        public Mask mask;
        public Couple(string CAcode, Mask mask)
        {
            this.CAcode = CAcode;
            this.mask = mask;
        }
    }

    class Solution
    {
        private List<Couple> algorithm;
        private double rank;

        public Solution(List<Couple> algorithm)
        {
            this.algorithm = algorithm;
        }

        public BitArray RunSolution(BitArray initial)
        {
            BitArray key = new BitArray(initial);
            BinaryCA ca;
            SeqGen seqGen;
            Mask mask;
            foreach (Couple couple in algorithm)
            {
                ca = Factory.CreateAutomaton(couple.CAcode, key);
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

        public Solution Mutate(double instability, Random r)
        {
            Solution changed = new Solution(algorithm);
            Primitives primitives = Primitives.Instance;
            for (int i = 0; i < algorithm.Count; i++)
            {
                if (r.NextDouble() < instability)
                {
                    changed.algorithm[i] = new Couple(primitives.RandomCAcode(), changed.algorithm[i].mask);
                }
                if (r.NextDouble() < instability)
                {
                    changed.algorithm[i] = new Couple(changed.algorithm[i].CAcode, primitives.RandomMask());
                }
            }
            return changed;
        }

        public Solution BreedWith(Solution parent, Random r)         //uniform breeding
        {
            Solution offspring = new Solution(algorithm);
            for (int i = 0; i < algorithm.Count; i++)
            {
                if (r.Next(2) == 1)
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
                CellularAutomaton aut = (CellularAutomaton)Factory.CreateAutomaton(couple.CAcode, new BitArray(8));
                Console.WriteLine(aut.TellType());
            }
        }
    }
}
