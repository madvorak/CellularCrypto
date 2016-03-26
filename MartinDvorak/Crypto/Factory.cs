using System;
using System.Collections.Generic;
using System.IO;
using Cellular;

namespace Crypto
{
    class Factory
    {
        public static KeyExtenderAbstractD CreateExtender(string description)
        {
            string[] parts = description.Split(new string[] { " using " }, StringSplitOptions.None);

            IBinaryCA automaton = null;
            if (parts[1].StartsWith("Basic"))
            {
                byte number = byte.Parse(parts[1].Split(' ')[3]);
                automaton = new ElementaryFastAutomaton(number, 1);
            }
            else if (parts[1].StartsWith("Binary"))
            {
                string ruleString = parts[1].Split(' ')[4];
                bool[] rule = new bool[32];
                for (int i = 0; i < 32; i++)
                {
                    rule[i] = ruleString[i] == '1';
                }
                automaton = new BinaryRangeAutomaton(2, rule, 1);
            }
            else if (parts[1].StartsWith("Cyclic"))
            {
                string ruleString = parts[1].Split(' ')[5];
                bool[] rule = new bool[32];
                for (int i = 0; i < 32; i++)
                {
                    rule[i] = ruleString[i] == '1';
                }
                automaton = new BinaryRangeAutomaton(2, rule, 1);
            }
            else if (parts[1].StartsWith("Totalistic"))
            {
                automaton = new GameOfLife(1, 1);
            }
            else
            {
                throw new ArgumentException("Wrong format!");
            }

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
