using APP_CircPrintServer.Functions;
using APP_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLL_CircPrintServer;
using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;

namespace APP_CircPrintServer.Forms
{
    public partial class formPOSPayment : Form
    {
        internal List<modelPOSTransLine> posPayments = new List<modelPOSTransLine>();
        internal List<modelPOSTransLine> posLines = new List<modelPOSTransLine>();
        internal modelSettings mS = new modelSettings();
        internal modelPOSTrans posTrans = new modelPOSTrans();
        int countPayments = 0;
        int countLines = 0;
        bool allowClose = false;
        public formPOSPayment()
        {
            InitializeComponent();
        }

        private void formPOSPayment_Load(object sender, EventArgs e)
        {
            foreach(modelPOSTransLine tlPatments in posPayments)
            {
                int controlNumber = countPayments + 1;
                Label lbl = this.Controls.Find("lblPayment" + controlNumber + "Name",true).FirstOrDefault() as Label;
                lbl.Visible = true;
                lbl.Text = tlPatments.paymentType;
                countPayments++;
            }
            int controlNumber1 = countPayments + 1;
            Label lbl2 = this.Controls.Find("lblPayment" + controlNumber1 + "Name", true).FirstOrDefault() as Label;
            lbl2.Visible = true;
            lbl2.Text = "Not Alocated";
            int count = 1;
            countPayments++;
            while (count <= countPayments)
            {
                int countInner = 1;
                while (countInner <= countLines)
                {
                    NumericUpDown tbx = this.Controls.Find("tbLine" + count + "Payment" + countInner, true).FirstOrDefault() as NumericUpDown;
                    tbx.Visible = true;
                    countInner++;
                }
                count++;
            }
            foreach (modelPOSTransLine tlLine in posLines)
            {
                int controlNumber = countLines + 1;
                Label lbl = this.Controls.Find("lblList" + controlNumber + "Name", true).FirstOrDefault() as Label;
                lbl.Text = tlLine.billReason;
                lbl.Visible = true;
                countLines++;
            }
            controlNumber1 = countLines + 1;
            lbl2 = this.Controls.Find("lblList" + controlNumber1 + "Name", true).FirstOrDefault() as Label;
            lbl2.Visible = true;
            lbl2.Text = "Not Alocated";
            count = 1;
            countLines++;
            while(count <= countLines)
            {
                int countInner = 1;
                while (countInner <= countPayments)
                {
                    NumericUpDown tbx = this.Controls.Find("tbLine" + count + "Payment" + countInner, true).FirstOrDefault() as NumericUpDown;
                    tbx.Visible = true;
                    countInner++;
                }
                count++;
            }
                          //Debugger.Launch();
            timerValidation.Start();
        }

        private void timerValidation_Tick(object sender, EventArgs e)
        {
            try
            {
                int ready = 0;
                int count = 0;
                foreach (modelPOSTransLine tlPayment in posPayments)
                {
                    int PaymentNumber = count + 1;
                    decimal totalPayment = tlPayment.paymentTotal;
                    int count2 = 1;
                    while (count2 < countLines)
                    {
                        NumericUpDown tbx = this.Controls.Find("tbLine" + count2 + "Payment" + PaymentNumber, true).FirstOrDefault() as NumericUpDown;
                        totalPayment = totalPayment - decimal.Parse(tbx.Text);
                        count2++;
                    }

                    NumericUpDown tbx2 = this.Controls.Find("tbLine" + count2 + "Payment" + PaymentNumber, true).FirstOrDefault() as NumericUpDown;
                    tbx2.Text = totalPayment.ToString();
                    if (tbx2.Text != "0.00") { tbx2.BackColor = Color.Red; } else { tbx2.BackColor = Color.Green; ready++; }
                    count++;
                }
                count = 1;

                foreach (modelPOSTransLine tlLine in posLines)
                {
                    decimal totalBill = tlLine.itemPrice * tlLine.itemQuantity;
                    int count2 = 1;
                    while (count2 < countPayments)
                    {

                        NumericUpDown tbx = this.Controls.Find("tbLine" + count + "Payment" + count2, true).FirstOrDefault() as NumericUpDown;
                        totalBill = totalBill - decimal.Parse(tbx.Text);
                        count2++;
                    }

                    NumericUpDown tbx2 = this.Controls.Find("tbLine" + count + "Payment" + countPayments, true).FirstOrDefault() as NumericUpDown;
                    tbx2.Text = totalBill.ToString();
                    if (tbx2.Text != "0.00") { tbx2.BackColor = Color.Red; } else { tbx2.BackColor = Color.Green; ready++; }
                    count++;
                }
                if (ready == countLines - 1 + countPayments - 1) { btnOK.Enabled = true; } else { btnOK.Enabled = false; }
            }
            catch (Exception) { }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int count = 1;
            int countTransLine = 1;
            foreach (modelPOSTransLine tlLine in posLines)
            {
                int countInter = 1;
                int paymentLimit = posPayments.Count;
                int lineLimit = posLines.Count;
                while (countInter <= paymentLimit)
                {
                    NumericUpDown tbx2 = this.Controls.Find("tbLine" + count + "Payment" + countInter, true).FirstOrDefault() as NumericUpDown;
                    if (tbx2.Text != "0.00")
                    {
                        if(count==lineLimit && countInter == paymentLimit)
                        {
                            tlLine.paymentTotal = decimal.Parse(tbx2.Text);
                            tlLine.paymentChange = posPayments[posPayments.Count - 1].paymentChange;
                            tlLine.paymentAmount = tlLine.paymentTotal + tlLine.paymentChange;
                            //lblPayment1Name
                            Label lbl = this.Controls.Find("lblPayment" + countInter + "Name", true).FirstOrDefault() as Label;
                            tlLine.paymentType = lbl.Text;
                            tlLine.idTrans = posTrans.transID;
                            tlLine.idLine = countTransLine;
                            controlPOS.postTransLine(mS, tlLine);
                            countTransLine++;
                        }
                        else
                        {
                            tlLine.paymentTotal = decimal.Parse(tbx2.Text);
                            tlLine.paymentChange = 0;
                            tlLine.paymentAmount = decimal.Parse(tbx2.Text);
                            Label lbl = this.Controls.Find("lblPayment" + countInter + "Name", true).FirstOrDefault() as Label;
                            tlLine.paymentType = lbl.Text;
                            tlLine.idTrans = posTrans.transID;
                            tlLine.idLine = countTransLine;
                            controlPOS.postTransLine(mS, tlLine);
                            countTransLine++;
                        }
                    }
                    countInter++;
                }

                count++;
            }
            allowClose = true;
            this.Close();
        }

        private void formPOSPayment_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!allowClose)
            {
                formPOSPayment frm = new formPOSPayment();
                frm.posLines = posLines;
                frm.posPayments = posPayments;
                frm.mS = mS;
                frm.posTrans = posTrans;
                frm.ShowDialog();
            }
        }
    }
}
