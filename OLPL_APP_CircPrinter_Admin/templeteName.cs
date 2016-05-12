using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLPL_APP_CircPrinter_Admin
{
    public partial class templeteName : Form
    {
        public templeteName()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fc = (Form1)Application.OpenForms["Form1"];
            fc.label2.Text = comboBox1.SelectedItem.ToString() + " templete named " + textBox1.Text;
            fc.btnAddElement.Visible = true;
            fc.tempateType = comboBox1.SelectedItem.ToString();
            fc.templateName = textBox1.Text;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
