using System;
using System.Collections.Generic;
using System.IO;
using Cellular;

namespace Crypto
{
    /// <summary>
    /// Factory class containing methods for building cellular automata and key extenders from textual description.
    /// Sometimes used together with <c>CellularAutomaton.TellType()</c> resp. <c>IBinaryCA.TellType()</c> method
    /// as a replacement for missing serialization / deserialization features.
    /// </summary>
    static class Factory
    {
        public static IBinaryCA CreateAutomaton(string description)
        {
            if (description.StartsWith("Basic"))
            {
                byte number = byte.Parse(description.Split(' ')[3]);
                return new ElementaryFastAutomaton(number, 1);
            }
            else if (description.StartsWith("Binary"))
            {
                string ruleString = description.Split(' ')[4];
                bool[] rule = new bool[32];
                for (int i = 0; i < 32; i++)
                {
                    rule[i] = ruleString[i] == '1';
                }
                return new BinaryRangeAutomaton(2, rule, 1);
            }
            else if (description.StartsWith("Cyclic"))
            {
                string ruleString = description.Split(' ')[5];
                bool[] rule = new bool[32];
                for (int i = 0; i < 32; i++)
                {
                    rule[i] = ruleString[i] == '1';
                }
                return new BinaryRangeAutomaton(2, rule, 1);
            }
            else if (description.StartsWith("Totalistic"))
            {
                return Totalistic2DAutomaton.CreateGameOfLife(1, 1);
            }
            else if (description.StartsWith("Game"))
            {
                return Totalistic2DAutomaton.CreateGameOfLife(1, 1);
            }
            else if (description.StartsWith("Amoeba"))
            {
                return Totalistic2DAutomaton.CreateAmoebaUniverse(1, 1);
            }
            else if (description.StartsWith("Replicator"))
            {
                return Totalistic2DAutomaton.CreateReplicatorUniverse(1, 1);
            }
            else
            {
                throw new ArgumentException("Wrong format!");
            }
        }

        public static KeyExtenderAbstractD CreateExtender(string description)
        {
            string[] parts = description.Split(new string[] { " using " }, StringSplitOptions.None);

            IBinaryCA automaton = CreateAutomaton(parts[1]);

            if (parts[0].StartsWith("SimpleLinear"))
            {
                return new KeyExtenderSimpleLinear(automaton);
            }
            else if (parts[0].StartsWith("Interlaced"))
            {
                string[] foo = parts[0].Split('(');
                string[] bar = foo[1].Split(')');
                string[] parameters = bar[0].Split(',');
                int first = int.Parse(parameters[0]);
                int second = int.Parse(parameters[1].Substring(1));
                return new KeyExtenderInterlaced(automaton, first, second);
            }
            else
            {
                throw new ArgumentException("Wrong format!");
            }
        }

        /// <summary>
        /// Gathers successful key extenders from all files in given directory.
        /// </summary>
        /// <param name="directory">The directory with ".xca" files.</param>
        /// <returns>
        /// List of (fully build) key extenders that were successful during previous runs of the genetic algorithm.
        /// </returns>
        public static List<KeyExtenderAbstractD> GatherSuccessfulExtenders(string directory)
        {
            List<KeyExtenderAbstractD> collection = new List<KeyExtenderAbstractD>();
            foreach (string fileName in Directory.EnumerateFiles(directory))
            {
                if (fileName.EndsWith(".xca"))
                {
                    StreamReader reader = new StreamReader(fileName);
                    reader.ReadLine();
                    reader.ReadLine();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        collection.Add(CreateExtender(line));
                    }
                }
            }
            return collection;
        }
    }
}
