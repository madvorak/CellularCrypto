using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cellular;

namespace GeneratePicture
{
    class Program
    {
        static void Main(string[] args)
        {
            const int ruleNo = 30;
            const int height = 112;
            const int coefficient = 8;
            const int width = 2 * height - 1;
            IBinaryCA automaton = new ElementaryAutomatonFaster(ruleNo, width);
            Bitmap image = new Bitmap(width * coefficient, height * coefficient);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int a = 0; a < coefficient; a++)
                        for (int b = 0; b < coefficient; b++)
                            image.SetPixel(j*coefficient + a, i*coefficient + b, automaton.GetValueAt(j) ? Color.Black : Color.White);
                }
                automaton.Step();
            }

            image.Save("rule" + ruleNo + "_" + height + "generations.png", ImageFormat.Png);
            image.Dispose();
        }
    }
}
