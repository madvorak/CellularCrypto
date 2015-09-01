using Cellular;
using System;
using System.Collections;

namespace Crypto
{
    static class Factory
    {
        public static BinaryCA CreateAutomaton(string code, BitArray input)
        { 
            string[] s = code.Split();
            byte number = byte.Parse(s[1]);
            if (s[0] == "B")
            {
                return new BasicAutomaton(number, input);
            }
            else if (s[0] == "G")
            { 
                int width = (int)(Math.Ceiling(Math.Sqrt(input.Length)));
                BitArray[] twoD = new BitArray[width];
                for (int i = 0; i < width; i++) twoD[i] = new BitArray(width);
                for (int i = 0; i < input.Length; i++)
                { 
                    twoD[i / width][i % width] = input[i];
                }
                return new GameOfLife(twoD);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
