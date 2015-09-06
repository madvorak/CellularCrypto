using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cellular;

public partial class DemoForm : Form
{
    private const int columnCount = 36;
    private const int linesCount = 20;

    private GameOfLifeInteractive gol;
    private Panel[,] matrix;
    HashSet<int> visited;
    
    public DemoForm()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        gol = new GameOfLifeInteractive(columnCount, linesCount);
        visited = new HashSet<int>();
        int topCorner = 55;
        int leftCorner = 10;
        int p_width = (Width - 22) / columnCount;
        int p_height = (Height - 90) / linesCount;
        matrix = new Panel[linesCount, columnCount];
        for (int i = 0; i < linesCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                Panel p = new Panel();
                p.SetBounds(leftCorner + (j * p_width), topCorner + (i * p_height), p_width, p_height);
                if (gol.GetCell(i, j))
                {
                    p.BackColor = Color.Black;
                }
                else
                {
                    p.BackColor = Color.White;
                }
                Controls.Add(p);
                p.Visible = true;
                p.Show();
                p.Tag = new Tuple<int, int>(i, j);
                p.Click += panel_Click;
                matrix[i, j] = p;
            }
        }
        btnDisplay.Enabled = false;
        btnStep.Enabled = true;
        btnPlay.Enabled = true;
    }

    private void panel_Click(object sender, EventArgs e)
    { 
        Tuple<int, int> pair = (Tuple<int, int>)(((Panel)sender).Tag);
        gol.FlipCell(pair.Item1, pair.Item2);
        ((Panel)sender).BackColor = gol.GetCell(pair.Item1, pair.Item2) ? Color.Black : Color.White;
        visited = new HashSet<int>();
        label1.Visible = false;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        drawStep();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        drawStep();
    }

    private void drawStep()
    {
        gol.Step();
        for (int i = 0; i < linesCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if (gol.GetCell(i, j))
                {
                    matrix[i, j].BackColor = Color.Black;
                }
                else
                {
                    matrix[i, j].BackColor = Color.White;
                }
            }
        }
        int h = gol.GetHashCode();
        if (visited.Contains(h))
        {
            label1.Visible = true;
        }
        else
        {
            visited.Add(h);
        }
    }

    private void button3_Click(object sender, EventArgs e)
    {
        timer.Enabled = true;
        btnStop.Enabled = true;
        btnPlay.Enabled = false;
        btnStep.Enabled = false;
    }

    private void button4_Click(object sender, EventArgs e)
    {
        timer.Enabled = false;
        btnPlay.Enabled = true;
        btnStop.Enabled = false;
        btnStep.Enabled = true;
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        timer.Interval = 10000 / (trackBar1.Value * trackBar1.Value);
    }
}
