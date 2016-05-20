using System;
using System.IO;
using System.Windows.Forms;
using Crypto;

namespace Program
{
    /// <summary>
    /// Windows application for users who want to encrypt their data using a cellular automata based algorithm.
    /// </summary>
    public partial class CryptoForm : Form
    {
        private EncryptionProvider crypto;

        public CryptoForm()
        {
            InitializeComponent();
            crypto = Export.GetEncrypterStreamCA();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "cry";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    Text = "In progress...";
                    Stream input = openDialog.OpenFile();
                    Stream output = saveDialog.OpenFile();
                    crypto.Encrypt(input, output, textBoxPassword.Text);
                    input.Dispose();
                    output.Dispose();
                    Text = "Done";
                }
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.DefaultExt = "cry";
            SaveFileDialog saveDialog = new SaveFileDialog();

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    Stream input = openDialog.OpenFile();
                    Stream output = saveDialog.OpenFile();
                    crypto.Decrypt(input, output, textBoxPassword.Text);
                    input.Dispose();
                    output.Dispose();
                }
            }
        }
    }
}
