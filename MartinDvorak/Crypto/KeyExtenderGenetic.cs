using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Algorithms which runs a genetic algorithm to find the best sequence of extenders for each key.
    /// Generating the long key may take a very long time.
    /// </summary>
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

        private const int popSize = 200;
        private const int iterations = 300;
        private const double breedProb = 0.2;
        private const double mutProb = 0.9;
        private const double pressure = 0.6;    //value 0.6 gives 80% chance picking the better

        private readonly IPrimitives primitives;
        private int rounds;
        private Individual savedBest;

        public KeyExtenderGenetic()
        {
            primitives = Primitives.Instance;
        }

#if (DEBUG)
        public KeyExtenderGenetic(bool usePreGenerated, string path = @"..\..\..\..\Results\xtenderSearch\")
#else
        public KeyExtenderGenetic(bool usePreGenerated, string path = @"..\Results\xtenderSearch\")
#endif
        {
            if (usePreGenerated)
            {
                primitives = new GoodPrimitives(path);
            }
            else
            {
                primitives = Primitives.Instance;
            }
        }

        /// <summary>
        /// Runs a Simple Genetic Algorithm to find the best serie of extenders.
        /// </summary>
        /// <param name="shortKey">Short key (input - sequence to be stretched).</param>
        /// <param name="targetLength">How long the result sequence should be.</param>
        /// <returns>Long key generated using the best found serie of extenders.</returns>
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
                    population[i].genome[j] = primitives.GetRandomExtender(rng);
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
                    if (a.fitness > b.fitness && rng.NextDouble() < pressure)
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

        /// <summary>
        /// Interface for class that keeps extenders and automata that can be used to double the key in any step.
        /// </summary>
        private interface IPrimitives
        {
            /// <summary>
            /// Gives a random key extender (from the list) with already assigned CA.
            /// </summary>
            /// <param name="r">Random number generator to be used.</param>
            /// <returns>Key extender.</returns>
            KeyExtenderAbstractD GetRandomExtender(Random r);
        }
        
        private class Primitives : IPrimitives
        {
            private List<IBinaryCA> automata;
            private List<KeyExtenderAbstractD> extenders;
            private static Primitives instance;

            public static Primitives Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Primitives();
                    }
                    return instance;
                }
            }

            private Primitives()
            {
                automata = new List<IBinaryCA>();
                for (int i = 0; i < 256; i++)
                {
                    automata.Add(new ElementaryAutomatonFast((byte)i, 1));
                }
                for (int i = 0; i < 100; i++)
                {
                    automata.Add(new BinaryRangeAutomaton(2, Utilities.RandomBoolArr(32), 1));
                    automata.Add(new BinaryRangeCyclicAutomaton(2, Utilities.RandomBoolArr(32), 1));
                }
                automata.Add(Totalistic2DAutomaton.CreateGameOfLife(1, 1));
                automata.Add(Totalistic2DAutomaton.CreateAmoebaUniverse(1, 1));
                automata.Add(Totalistic2DAutomaton.CreateReplicatorUniverse(1, 1));

                extenders = new List<KeyExtenderAbstractD>();
                foreach (IBinaryCA aut in automata)
                {
                    extenders.Add(new KeyExtenderSimpleLinear(aut));
                    extenders.Add(new KeyExtenderInterlaced(aut, 4, 0));
                    extenders.Add(new KeyExtenderInterlaced(aut, 4, 1));
                    extenders.Add(new KeyExtenderInterlaced(aut, 4, 9));
                    extenders.Add(new KeyExtenderInterlaced(aut, 10, 0));
                    extenders.Add(new KeyExtenderInterlaced(aut, 10, 1));
                    extenders.Add(new KeyExtenderInterlaced(aut, 10, 9));
                }
            }

            public KeyExtenderAbstractD GetRandomExtender(Random r)
            {
                return extenders[r.Next(extenders.Count)];
            }
        }

        private class GoodPrimitives : IPrimitives
        {
            private List<KeyExtenderAbstractD> goodExtenders;

            public GoodPrimitives(string path)
            {
                goodExtenders = Factory.GatherSuccessfulExtenders(path);
            }

            public KeyExtenderAbstractD GetRandomExtender(Random r)
            {
                return goodExtenders[r.Next(goodExtenders.Count)];
            }
        }

        /// <summary>
        /// One-point crossover. Does not modify the parents.
        /// </summary>
        /// <param name="par1">First parent.</param>
        /// <param name="par2">Second parent.</param>
        /// <param name="off1">First offspring.</param>
        /// <param name="off2">Second offspring.</param>
        /// <param name="r">Random number generator to be used.</param>
        private void breedParents(Individual par1, Individual par2, out Individual off1, out Individual off2, Random r)
        {
            int crossOver = r.Next(rounds - 1) + 1;
            off1 = new Individual(rounds);
            off2 = new Individual(rounds);
            for (int i = 0; i < crossOver; i++)
            {
                off1.genome[i] = par1.genome[i];
                off2.genome[i] = par2.genome[i];
            }
            for (int i = crossOver; i < rounds; i++)
            {
                off1.genome[i] = par2.genome[i];
                off2.genome[i] = par1.genome[i];
            }
        }

        private void mutate(Individual ind, Random r)
        {
            ind.genome[r.Next(ind.genome.Length)] = primitives.GetRandomExtender(r);
        }

        /// <summary>
        /// Gives info about the best (generator&CA) sequence found during the last run of the algorithm.
        /// </summary>
        /// <returns>Info as a string.</returns>
        internal string getInfoAboutWinner()
        {
            if (savedBest == null)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            foreach (KeyExtenderAbstractD doubler in savedBest.genome)
            {
                sb.AppendLine(doubler.GetInfo());
            }
            return sb.ToString();
        }
    }
}
