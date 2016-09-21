using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DLL_CircPrintServer.Classes.controlFunctions;

namespace APP_CircPrintServer.Forms
{
    public partial class changePrinter : Form
    {
        public modelSettings mS;
        public string printer = "";
        public string name = "";
        public changePrinter()
        {
            InitializeComponent();
        }

        private void changePrinter_Load(object sender, EventArgs e)
        {
            this.Visible = true;
            using (ImpersonationUtils.ImpersonateCurrentUser())
            {
                foreach (string str in PrinterSettings.InstalledPrinters)
                {
                    cbListofPrinters.Items.Add(str);
                }
            }
        }

        private void changePrinter_Leave(object sender, EventArgs e)
        {

        }

        private void btnSavePrinter_Click(object sender, EventArgs e)
        {
            printer = cbListofPrinters.SelectedItem.ToString();
            controlSettings.writeSetting("Printing", name, printer);
            this.Close();
        }
    }
}
