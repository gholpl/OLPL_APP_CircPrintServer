using APP_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APP_CircPrintServer.Functions;
using DLL_CircPrintServer;
using DLL_CircPrintServer.Models;
using System.Diagnostics;

namespace APP_CircPrintServer.Forms
{
    public partial class CCRefund : Form
    {
        internal modelPOSTrans mPOS;
        internal modelSettings mS;
        internal modelPOSTransLine posTransLine;
        public CCRefund()
        {
            InitializeComponent();
        }

        private void CCRefund_Load(object sender, EventArgs e)
        {
            //Debugger.Launch();
            tbContactInfo.Text = controlSIP.getPatronInfo(mS,mPOS);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbCCDigits.Text.Length>2 && tbContactInfo.Text.Length > 2 && cbNotifyPreference.Text.Length > 2)
            { 
               controlPOS.postCCRefund(mS, mPOS, tbCCDigits.Text, cbNotifyPreference.Text, tbContactInfo.Text, tbNotes.Text, tbRecieptNumber.Text);
                if (mS.POSEmailEnable == true)
                {
                    string body = "Operator ID: " + mPOS.operatorID + Environment.NewLine + "User ID: " + mPOS.userID + Environment.NewLine
                        + "Contact Information:" + tbContactInfo.Text + Environment.NewLine + Environment.NewLine
                        + "Notification Preference: " + cbNotifyPreference.Text + Environment.NewLine
                        + "Original Reciept Number: " + tbRecieptNumber.Text + Environment.NewLine
                        + "Original Reciept Date: " + dpTransDate.Text + Environment.NewLine
                        + "Notes: " + tbNotes.Text + Environment.NewLine + "Credit card last 4 digits: " + tbCCDigits.Text + Environment.NewLine
                        + "Refund Amount: " + posTransLine.paymentAmount + Environment.NewLine
                        + "Description: " + posTransLine.itemTitle;
                    controlPOS.emailRefund(mS, mPOS, body);
                }
                this.Close();
            }
            else { MessageBox.Show("Please FIllout aff requered fields!!"); }
        }
    }
}
