using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APP_CircPrintServer.Forms
{
    public partial class AskTransitLibrary : Form
    {
        public Form1 frm1;
        public AskTransitLibrary()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.TextLength > 5)
            {
                frm1.toLibrary = textBox1.Text;
                this.Close();
            }
            else { MessageBox.Show("Please Enter the name of the library that this item is going to"); }
        }
    }
}
