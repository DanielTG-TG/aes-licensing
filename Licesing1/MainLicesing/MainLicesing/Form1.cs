using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Licesing;

namespace MainLicesing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("License.txt"))
                {
                    byte[] encrypted = File.ReadAllBytes("License.txt");
                    string decrypted = SymmetricEncryptor.DecryptToString(encrypted, "k3bTn5dQ4DD8NHh6MBgGusuRAGNorsUH");
                    MessageBox.Show("Licesing key: " + decrypted);
                    //now you should verify the key in your host. Make sure to use end to end encryption.
                    //Good luck.
                }
                else
                {
                    MessageBox.Show("License file not found. If you registered already put the file in the same folder.", "Licesing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {

            }
        }
    }
}
