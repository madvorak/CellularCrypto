using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using Crypto;

namespace Program
{
    public partial class Form1 : Form
    {
        private IKeyExtender xtend;

        public Form1()
        {
            InitializeComponent();
            xtend = Export.GetKeyExtender();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    Stream stream = openDialog.OpenFile();
                    int length = (int)stream.Length;
                    byte[] buffer = new byte[length];
                    stream.Read(buffer, 0, length);
                    BitArray wholeFile = new BitArray(buffer);
                    stream.Dispose();

                    byte[] password = Encoding.UTF8.GetBytes(textBoxPassword.Text);
                    BitArray hash = new BitArray((new SHA256Cng()).ComputeHash(password));
                    BitArray oneTimePad = xtend.ExtendKey(hash, length * 8);
                    BitArray encrypted = oneTimePad.Xor(wholeFile);

                    Stream output = saveDialog.OpenFile();
                    for (int i = 0; i < length; i++)
                    {
                        buffer[i] = 0;
                    }
                    for (int i = length * 8 - 1; i >= 0; i--)
                    {
                        buffer[i / 8] *= 2;
                        if (encrypted[i])
                        {
                            buffer[i / 8] |= 1;
                        }
                    }
                    output.Write(buffer, 0, length);
                    output.Dispose();
                }
            }
        }
    }
}
