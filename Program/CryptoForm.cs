using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using Crypto;

namespace Program
{
    public partial class CryptoForm : Form
    {
        private IKeyExtender xtend;
        private RNGCryptoServiceProvider rng;
        private const int saltBytes = 16;

        public CryptoForm()
        {
            InitializeComponent();
            xtend = Export.GetKeyExtender();
            rng = new RNGCryptoServiceProvider();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
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
                    int passLength = password.Length;
                    byte[] pswdWithSalt = new byte[passLength + saltBytes];
                    Array.Copy(password, pswdWithSalt, passLength);
                    rng.GetBytes(pswdWithSalt, passLength, saltBytes);
                    BitArray hash = new BitArray((new SHA256Cng()).ComputeHash(pswdWithSalt));
                    BitArray oneTimePad = xtend.ExtendKey(hash, length * 8);
                    BitArray encrypted = oneTimePad.Xor(wholeFile);

                    Stream output = saveDialog.OpenFile();
                    output.Write(pswdWithSalt, passLength, saltBytes);
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

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    byte[] password = Encoding.UTF8.GetBytes(textBoxPassword.Text);
                    int passLength = password.Length;
                    byte[] pswdWithSalt = new byte[passLength + saltBytes];
                    Array.Copy(password, pswdWithSalt, passLength);

                    Stream stream = openDialog.OpenFile();
                    int length = (int)stream.Length - saltBytes;
                    stream.Read(pswdWithSalt, passLength, saltBytes);
                    byte[] buffer = new byte[length];
                    stream.Read(buffer, 0, length);
                    BitArray wholeFile = new BitArray(buffer);
                    stream.Dispose();

                    BitArray hash = new BitArray((new SHA256Cng()).ComputeHash(pswdWithSalt));
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
