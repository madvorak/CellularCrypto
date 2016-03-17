using System;
using System.Collections;
using System.Collections.Generic;
using Cellular;

namespace Crypto
{
    class KeyExtenderGenetic : KeyExtenderAbstractN
    {
        private class Individual
        {
            public readonly KeyExtenderAbstractD[] genome;
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
        }

        private const int popSize = 100;
        private const int iterations = 500;

        public override BitArray ExtendKey(BitArray shortKey, int targetLength)
        {
            Random rng = new Random(0);
            int rounds = (int) Math.Ceiling(Math.Log(targetLength / shortKey.Length, 2));
            Individual[] population = new Individual[popSize];
            Individual bestSoFar = new Individual(rounds) { fitness = 0 };
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

            for (int i = 0; i < iterations; i++)
            {
                //TODO
            }

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
    }
}
