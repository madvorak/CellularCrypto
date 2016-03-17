using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cellular;

namespace Crypto
{
    class KeyExtenderGenetic : KeyExtenderAbstractN
    {
        private class Individual
        {
            public KeyExtenderAbstractD[] genome;
            public double fitness;

            public Individual(int rounds)
            {
                genome = new KeyExtenderAbstractD[rounds];
            }

            public BitArray Run(BitArray shortKey)
            {
                BitArray key = shortKey;
                foreach (KeyExtenderAbstractD doubler in genome)
                {
                    key = doubler.DoubleKey(key);
                }
                return key;
            }

            public double RunToRate(BitArray shortKey)
            {
                BitArray result = Run(shortKey);
                fitness = RandomnessTesting.RateSequence(result);
                return fitness;
            }

            public Individual Copy()
            {
                Individual copy = new Individual(genome.Length);
                for (int i = 0; i < genome.Length; i++)
                {
                    copy.genome[i] = genome[i];
                }
                copy.fitness = fitness;
                return copy;
            }
        }

        private const int popSize = 100;
        private const int iterations = 200;
        private const double breedProb = 0.3;
        private const double mutProb = 0.8;

        private int rounds;
        private Individual savedBest;

        public override BitArray ExtendKey(BitArray shortKey, int targetLength)
        {
            // initiation
            Random rng = new Random(0);
            rounds = (int) Math.Ceiling(Math.Log(targetLength / shortKey.Length, 2));
            Individual[] population = new Individual[popSize];
            Individual bestSoFar = new Individual(rounds);
            for (int i = 0; i < popSize; i++)
            {
                population[i] = new Individual(rounds);
                for (int j = 0; j < rounds; j++)
                {
                    population[i].genome[j] = Primitives.GetRandomExtender(rng);
                }
                if (population[i].RunToRate(shortKey) > bestSoFar.fitness)
                {
                    bestSoFar = population[i];
                }
            }

            // evolution
            for (int i = 0; i < iterations; i++)
            {
                Individual[] nextGeneration = new Individual[popSize];
                
                // selection
                Individual a, b;
                for (int j = 0; j < popSize; j++)
                {
                    a = population[rng.Next(popSize)];
                    b = population[rng.Next(popSize)];
                    if (a.fitness > b.fitness && rng.NextDouble() < 0.8)    //gives 90% chance picking the better
                    {
                        nextGeneration[j] = a;
                    }
                    else
                    {
                        nextGeneration[j] = b;
                    }
                }

                // breeding
                for (int j = 0; j < popSize - 1; j += 2)
                {
                    if (rng.NextDouble() < breedProb)
                    {
                        Individual mother = nextGeneration[j];
                        Individual father = nextGeneration[j + 1];
                        Individual son, daughter;
                        breedParents(mother, father, out son, out daughter, rng);
                        nextGeneration[j] = son;
                        nextGeneration[j + 1] = daughter;
                    }
                    else
                    {
                        nextGeneration[j] = nextGeneration[j].Copy();
                        nextGeneration[j + 1] = nextGeneration[j + 1].Copy();
                    }
                }

                // mutation and evaluation
                for (int j = 0; j < popSize; j++)
                {
                    if (rng.NextDouble() < mutProb)
                    {
                        mutate(nextGeneration[j], rng);
                    }
                    nextGeneration[j].RunToRate(shortKey);
                    if (nextGeneration[j].fitness > bestSoFar.fitness)
                    {
                        bestSoFar = nextGeneration[j].Copy();
                    }
                }

                population = nextGeneration;
            }

            // output
            savedBest = bestSoFar;
            BitArray toOutput = bestSoFar.Run(shortKey);
            BitArray clipped = new BitArray(targetLength);
            int bias = rng.Next(toOutput.Length - targetLength + 1);
            for (int i = 0; i < targetLength; i++)
            {
                clipped[i] = toOutput[i + bias];
            }
            return clipped;
        }

        private static class Primitives
        {
            private static List<IBinaryCA> automata;
            private static List<KeyExtenderAbstractD> extenders;

            static Primitives()
            {
                automata = new List<IBinaryCA>();
                for (int i = 0; i < 256; i++)
                {
                    automata.Add(new ElementaryFastAutomaton((byte)i, 1));
                }
                for (int i = 0; i < 100; i++)
                {
                    automata.Add(new BinaryRangeAutomaton(2, Utilities.RandomBoolArr(32), 1));
                    automata.Add(new BinaryRangeCyclicAutomaton(2, Utilities.RandomBoolArr(32), 1));
                }
                automata.Add(new GameOfLife(1, 1));

                extenders = new List<KeyExtenderAbstractD>();
                foreach (IBinaryCA aut in automata)
                {
                    extenders.Add(new KeyExtenderSimpleLinear(aut));
                    extenders.Add(new KeyExtenderInterlaced(aut, 4, 1));
                }
            }

            public static KeyExtenderAbstractD GetRandomExtender(Random r)
            {
                return extenders[r.Next(extenders.Count)];
            }
        }

        private void breedParents(Individual mother, Individual father, 
            out Individual son, out Individual daughter, Random r)
        {
            int crossOver = r.Next(rounds - 1) + 1;
            son = new Individual(rounds);
            daughter = new Individual(rounds);
            for (int i = 0; i < crossOver; i++)
            {
                son.genome[i] = mother.genome[i];
                daughter.genome[i] = father.genome[i];
            }
            for (int i = crossOver; i < rounds; i++)
            {
                son.genome[i] = father.genome[i];
                daughter.genome[i] = mother.genome[i];
            }
        }

        private void mutate(Individual ind, Random r)
        {
            ind.genome[r.Next(ind.genome.Length)] = Primitives.GetRandomExtender(r);
        }

        internal string getInfoAboutWinner()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyExtenderAbstractD doubler in savedBest.genome)
            {
                sb.AppendLine(doubler.GetInfo());
            }
            return sb.ToString();
        }
    }
}
