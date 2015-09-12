using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Cellular;

namespace UserForms
{
    public partial class DemoForm : Form
    {
        private const int columnCount = 52;
        private const int linesCount = 30;

        private GameOfLifeInteractive gol;
        private Panel[,] matrix;
        HashSet<int> visited;

        public DemoForm()
        {
            InitializeComponent();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
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
            btnNewGame.Enabled = true;
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        /// <summary>
        /// Switching the state of the cell upon clicking.
        /// </summary>
        private void panel_Click(object sender, EventArgs e)
        {
            Tuple<int, int> pair = (Tuple<int, int>)(((Panel)sender).Tag);
            gol.FlipCell(pair.Item1, pair.Item2);
            ((Panel)sender).BackColor = gol.GetCell(pair.Item1, pair.Item2) ? Color.Black : Color.White;
            visited = new HashSet<int>();
            label1.Visible = false;
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            performStep();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            performStep();
        }

        /// <summary>
        /// Calling and drawing a new step. Checking whether any state repeats.
        /// </summary>
        private void performStep()
        {
            gol.Step();
            redrawState();
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

        private void redrawState()
        {
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
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            btnStop.Enabled = true;
            btnPlay.Enabled = false;
            btnStep.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            btnPlay.Enabled = true;
            btnStop.Enabled = false;
            btnStep.Enabled = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer.Interval = 7500 / (trackBar1.Value * trackBar1.Value);
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == comboBox1.Items[1])
            {
                //random initial state
                gol = new GameOfLifeInteractive(columnCount, linesCount);
            }
            else
            {
                //blank initial state
                BitArray[] initial = new BitArray[linesCount];
                for (int i = 0; i < linesCount; i++)
                {
                    initial[i] = new BitArray(columnCount, false);
                }
                if (comboBox1.SelectedItem == comboBox1.Items[2])
                {
                    //symmetric
                    for (int i = columnCount / 2 - 4; i < columnCount / 2 + 4; i++)
                    {
                        initial[linesCount / 2][i] = true;
                    }
                }
                if (comboBox1.SelectedItem == comboBox1.Items[3])
                {
                    //glider
                    initial[1][2] = true;
                    initial[2][3] = true;
                    initial[3][3] = true;
                    initial[3][2] = true;
                    initial[3][1] = true;
                }
                if (comboBox1.SelectedItem == comboBox1.Items[4])
                {
                    //glider gun
                    initial[5][1] = initial[5][2] = initial[6][1] = initial[6][2] = true;
                    initial[3][35] = initial[3][36] = initial[4][35] = initial[4][36] = true;
                    initial[5][11] = initial[6][11] = initial[7][11] = true;
                    initial[4][12] = initial[8][12] = true;
                    initial[3][13] = initial[3][14] = initial[9][13] = initial[9][14] = true;
                    initial[6][15] = initial[4][16] = initial[8][16] = true;
                    initial[6][17] = initial[6][18] = initial[5][17] = initial[7][17] = true;
                    for (int i = 3; i <= 5; i++) initial[i][21] = initial[i][22] = true;
                    initial[2][23] = initial[6][23] = true;
                    initial[1][25] = initial[2][25] = initial[6][25] = initial[7][25] = true;
                }
                gol = new GameOfLifeInteractive(initial);
            }
            visited = new HashSet<int>();
            redrawState();
        }
    }
}
