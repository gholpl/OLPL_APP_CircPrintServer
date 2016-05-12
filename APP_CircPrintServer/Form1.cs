using APP_CircPrintServer.Forms;
using APP_CircPrintServer.Functions;
using APP_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace APP_CircPrintServer
{
    public partial class Form1 : Form
    {
        static string dataStr = "";
        string arg;
        public string toLibrary = "";
        public string dueDate = "";
        public Form1(string args)
        {
            InitializeComponent();
            arg = args;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            //string[] args = Environment.GetCommandLineArgs();
            //MessageBox.Show(args[0]);
            modelSettings1 mS = FileControl.proccessSettingFile();
            //MessageBox.Show(mS.fileLog);
            #region instreamcapture
            if (arg == "none")
            {
                //MessageBox.Show("in cap");
                string str = "";
                string result = "";
                using (Stream stdin = Console.OpenStandardInput())
                {
                    byte[] buffer = new byte[2048];
                    int bytes;
                    while ((bytes = stdin.Read(buffer, 0, buffer.Length)) > 0)
                    {

                        str = str + System.Text.Encoding.Default.GetString(buffer);
                    }
                    if (mS.switchAdminMode != "0") { FileControl.fileWriteLog(str, mS); }


                    if (str.Contains("Date due"))
                    {
                        int check = 0;
                        if (str.Contains('\f')) { check = 1; }
                        try
                        {
                            FileControl.fileWriteTempData(str, mS);
                            if (check == 1)
                            {
                                int checkRec = 1;
                                if (mS.switchAskCheckOut == "1")
                                {
                                    if (MessageBox.Show("Does the patron want a checkout reciept?", "Confirm Checkout Reciept", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                                    {
                                        checkRec = 1;
                                    }
                                    else
                                    {
                                        dataStr = File.ReadAllText(mS.fileTempData);
                                        if (mS.switchAdminMode == "0") { File.Delete(mS.fileTempData); }
                                        foreach (string str44 in dataStr.Split(Environment.NewLine.ToCharArray()))
                                        {
                                            if (str44.Length > 5)
                                            {
                                                statsControl.tickStats(mS, "Items Checked out");
                                                if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Items Checked out", mS); }
                                            }
                                        }
                                        checkRec = 0;
                                        statsControl.tickStats(mS, "Checkout Slips - Skipped");
                                        if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Checkout Slips - Skipped", mS); }
                                    }
                                }

                                if (checkRec == 1)
                                {
                                    statsControl.tickStats(mS, "Checkout Slips - Printed");
                                    if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Checkout Slips - Printed", mS); }
                                    dataStr = File.ReadAllText(mS.fileTempData);
                                    if (mS.switchAdminMode == "0") { File.Delete(mS.fileTempData); }
                                    printControl.printPage(mS, dataStr, "checkout",this);
                                }
                            }
                        }
                        catch (Exception e1)
                        {
                            FileControl.fileWriteLog(e1.ToString(), mS);
                        }
                    }
                    else if (str.Contains('\f') && str.Contains("HOLD SLIP START"))
                    {
                        try
                        {
                            bool printRec = true;
                            if (mS.switchAskHolds == "1")
                            {
                                if (MessageBox.Show("Do you want to print a holds slip?", "Confirm Holds Slip", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                                { 
                                    printRec = false;
                                    statsControl.tickStats(mS, "Skipped Hold Slips");
                                }
                            }
                            if (printRec == true)
                            {
                                statsControl.tickStats(mS, "Hold Slips");
                                if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Hold Slips", mS); }
                                dataStr = str;
                                printControl.printPage(mS, dataStr, "holdsP1",this);
                                if (mS.switchTwoPageHolds == "1")
                                {
                                    printControl.printPage(mS, dataStr, "holdsP2",this);
                                }
                            }
                        }
                        catch (Exception e1)
                        {
                            FileControl.fileWriteLog(e1.ToString(), mS);
                        }
                    }
                    else if (str.Contains("Transit Slip"))
                    {
                        bool printRec = true;
                        if (mS.switchAskTransit == "1")
                        {
                            if (MessageBox.Show("Do you want to print a transit slip?", "Confirm Transit Slip", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                            {
                                statsControl.tickStats(mS, "Transit Slips Skipped");
                                printRec = false;
                            }
                        }
                        if(printRec== true)
                        {
                            dataStr = str;
                            
                            statsControl.tickStats(mS, "Transit Slips");
                            if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Transit Slips" + dataStr, mS); }
                            printControl.printPage(mS, dataStr, "intransit",this);
                        }
                    }

                    else if (str.Contains("Payment"))
                    {
                        string[] str_Run1 = str.Split(Environment.NewLine.ToCharArray());
                        foreach (string str4 in str_Run1)
                        {
                            if (!string.IsNullOrEmpty(str4) && !str4.Contains('\f'))
                            {
                                result = result + str4 + Environment.NewLine;
                            }
                        }
                        dataStr = result;
                        if (mS.POSEnable)
                        {
                            FormTest ft = new FormTest();
                            ft.Show();
                            modelPOSTrans posTrans = new modelPOSTrans();
                            //modelPOS mPOS = new modelPOS();
                            posTrans.stationType = "Workstation";
                            List<modelPOSTransLine> lPOS = new List<modelPOSTransLine>();
                            string dataUpload = "";
                            foreach(string str12 in dataStr.Split(Environment.NewLine.ToCharArray()))
                            {
                                if (!string.IsNullOrEmpty(str12))
                                {
                                    dataUpload = dataUpload + str12 + "|";
                                }
                            }
                            posTrans.strData = dataUpload;
                            bool nonCashMan = false;
                            if (!dataStr.Contains("Operator ID"))
                            {
                                try
                                {
                                    string operatorID = File.ReadAllText(System.IO.Path.GetTempPath() + "\\sirsiLogin");
                                    posTrans.operatorID = operatorID.Split('|')[0];
                                    nonCashMan = true;
                                }
                                catch (Exception)
                                {
                                    InputBoxValidation validation = delegate (string val) {
                                        if (val == "")
                                            return "Value cannot be empty.";
                                        return "";
                                    };
                                    string value = "";
                                    if (inP.Show("Enter Operator ID", "Operator ID:", ref value, validation) == DialogResult.OK)
                                    {
                                        posTrans.operatorID = value;
                                    }


                                }
                            }
                            if (!dataStr.Contains("User ID"))
                            {
                                try
                                {
                                    InputBoxValidation validation = delegate (string val)
                                    {
                                        if (val == "")
                                            return "Value cannot be empty.";
                                        if (val.Length<8)
                                            return "Invalid barcode.";
                                        return "";
                                    };
                                    string value = "";
                                    
                                    //if (inP.Show("Enter User ID", "User ID:", ref value,validation) == DialogResult.OK)
                                    //{
                                    //    posTrans.userID = value;
                                    //}
                                   // MessageBox.Show(mPOS.operatorID + " " + mPOS.userID);
                                }
                                catch (Exception e1)
                                {
                                    MessageBox.Show("Error Getting userid " + e1.Message);
                                    FileControl.fileWriteLog(e1.ToString(), mS);
                                }
                            }
                            else
                            {
                                foreach (string str11 in dataStr.Split(Environment.NewLine.ToCharArray()))
                                {
                                    if (!string.IsNullOrEmpty(str11))
                                    {
                                        if (str11.Contains("User ID:")) { posTrans.userID = str11.Replace("User ID: ", ""); }
                                        if (str11.Contains("Operator ID:")) { posTrans.operatorID = str11.Replace("Operator ID: ", ""); } 
                                    }
                                }
                            }
                            if (dataStr.Contains("Sale")) { posTrans.transType = "Sale"; }
                            else if (dataStr.Contains("Refund")) { posTrans.transType = "Refund"; }
                            else if (dataStr.Contains("Void")) { posTrans.transType = "Void"; }
                            else { posTrans.transType = "Sale"; }
                            posTrans.transID = controlPOS.getTransID(mS, posTrans);
                            try
                            {
                                if (nonCashMan)
                                {
                                    DateTime paymentDate;
                                    string recieptData = "";
                                    #region NonCashManagementTrans
                                    string[] stringSeparators = new string[] { "Original bi" };
                                    int line = 1;
                                    string[] dataStr1 = dataStr.Split(stringSeparators, StringSplitOptions.None);
                                    foreach (string str1 in dataStr1)
                                    {
                                        if (str1.Contains("Payment date:"))
                                        {
                                            string[] intStr = str1.Split(Environment.NewLine.ToCharArray());
                                            foreach (string str2 in intStr)
                                            {
                                                //MessageBox.Show(str.Replace("Payment date: ", ""));
                                                if (str2.Contains("Payment date:")) { paymentDate = DateTime.Parse(str2.Replace("Payment date: ", "")); }
                                            }
                                        }
                                        else
                                        {
                                            modelPOSTransLine posTransLine = new modelPOSTransLine();
                                            posTransLine.idTrans = posTrans.transID;
                                            posTransLine.itemQuantity = 1;
                                            posTransLine.idLine = line;
                                            bool nextLine = false;
                                            bool change = false;
                                            bool exceptionData = false;
                                            string[] intStr = str1.Split(Environment.NewLine.ToCharArray());
                                            if (dataStr.Contains("Remaining balance:")) { exceptionData = true; }
                                            foreach (string str2 in intStr)
                                            {
                                                if (str2.Contains("ll: $")) { posTransLine.billTotal = Decimal.Parse(str2.Replace("ll: $", "")); }
                                                if (str2.Contains("Author:")) { posTransLine.itemAuthor = str2.Replace("Author: ", ""); }
                                                if (str2.Contains("Change:")) { posTransLine.paymentChange = Decimal.Parse(str2.Replace("Change: $", "")); change = true; }
                                                if (str2.Contains("Call number:")) { posTransLine.itemCallNumber = str2.Replace("Call number: ", ""); }
                                                if (str2.Contains("Title:")) { posTransLine.itemTitle = str2.Replace("Title: ", ""); }
                                                if (str2.Contains("Item ID:")) { posTransLine.itemID = str2.Replace("Item ID: ", ""); }
                                                if (nextLine && !String.IsNullOrEmpty(str2)&&!exceptionData)
                                                {
                                                    string[] intStr1 = str2.Split(' ');
                                                    posTransLine.paymentType = intStr1[0];
                                                    posTransLine.paymentAmount = Decimal.Parse(intStr1[1]);
                                                    nextLine = false;
                                                }
                                                if (str2.Contains("Payment status:"))
                                                {
                                                    //mPOS1.status = str2.Replace("Payment status: ", "");
                                                    nextLine = true;
                                                }
                                                if (!change) { posTransLine.paymentChange = Decimal.Parse("0.00"); }
                                                if (str2.Contains("Bill reason:")) { posTransLine.billReason = str2.Replace("Bill reason: ", ""); }
                                                if (exceptionData && str2.Contains("Payment type:")) { posTransLine.paymentType = (str2.Replace("Payment type: ", "")); }
                                                if (exceptionData && str2.Contains("Amount paid:")) { posTransLine.paymentAmount = Decimal.Parse(str2.Replace("Amount paid: $", "")); }
                                            }
                                            posTransLine.paymentTotal = posTransLine.paymentAmount - posTransLine.paymentChange;
                                            posTransLine.itemPrice = posTransLine.paymentTotal;
                                            controlPOS.postTransLine(mS, posTransLine);
                                            lPOS.Add(posTransLine);
                                            line++;
                                        }

                                    }
                                    recieptData = "Transaction Number: " + posTrans.transID + Environment.NewLine + "Date: " + DateTime.Now.ToLongDateString() + Environment.NewLine + Environment.NewLine
                                        //+ "Operator ID: " + lPOS[0].operatorID + Environment.NewLine
                                        + "User ID: " + posTrans.userID + Environment.NewLine
                                        + "Transaction Type: " + posTrans.transType + Environment.NewLine + Environment.NewLine;
                                    foreach (modelPOSTransLine mPOS3 in lPOS)
                                    {
                                        recieptData = recieptData + "Bill Reason: " + mPOS3.billReason + Environment.NewLine;
                                        if (!string.IsNullOrEmpty(mPOS3.itemID)) { recieptData = recieptData + "ItemID: " + mPOS3.itemID + Environment.NewLine; }
                                        if (!string.IsNullOrEmpty(mPOS3.itemTitle)) { recieptData = recieptData + "Title: " + mPOS3.itemTitle + Environment.NewLine; }
                                        recieptData = recieptData + Environment.NewLine;
                                        recieptData = recieptData + "Payment Type: " + mPOS3.paymentType + Environment.NewLine;
                                        recieptData = recieptData + "Amount Paid: " + mPOS3.paymentAmount + Environment.NewLine;
                                        recieptData = recieptData + "Change: " + mPOS3.paymentChange + Environment.NewLine + Environment.NewLine;

                                    }
                                    dataStr = recieptData;
                                    controlPOS.closeTrans(mS, posTrans);
                                    #endregion
                                }
                                else
                                {
                                    #region cashmanagemnttrans
                                    if (dataStr.Contains("Sale"))
                                    {
                                        DateTime dt1 = new DateTime();
                                        //modelPOS mPOS = new modelPOS();
                                        #region CashManagementTransSale
                                        bool libName = false;
                                        modelPOSTransLine posTempLine = new modelPOSTransLine();
                                        bool paymentLines = false;
                                        List<string> itemSet = new List<string>();
                                        List<string> itemSet2 = new List<string>();
                                        List<string> paymentSet = new List<string>();
                                        foreach (string str11 in dataStr.Split(Environment.NewLine.ToCharArray()))
                                        {
                                            if (!string.IsNullOrEmpty(str11))
                                            {
                                                if (str11.Contains("User ID:")) { posTrans.userID = str11.Replace("User ID: ", ""); }
                                                if (str11.Contains("Operator ID:")) { posTrans.operatorID = str11.Replace("Operator ID: ", ""); }
                                                if (str11.Contains("Grand Total:")) { libName = false; paymentLines = true; }
                                                if (str11.Contains("Total Payments:")) { paymentLines = false; }
                                                if (libName)
                                                {
                                                    itemSet.Add(str11);
                                                    int count = str11.TakeWhile(c => c == '$').Count();
                                                    if (count > 1)
                                                    {

                                                    }
                                                    else { posTempLine.itemTitle = str11; }
                                                }
                                                if (paymentLines)
                                                {
                                                    if (str11.Contains('$') && !str11.Contains("Grand Total:"))
                                                    {
                                                        paymentSet.Add(str11);
                                                    }
                                                }
                                                if (str11.Contains("Library name:")) { libName = true; }
                                                if (str11.Contains("Change:")) { posTempLine.paymentChange = Decimal.Parse(str11.Replace("Change: $", "")); }

                                            }
                                        }
                                        List<modelPOSTransLine> posPayments = new List<modelPOSTransLine>();
                                        List<modelPOSTransLine> posLines = new List<modelPOSTransLine>();
                                        int size = paymentSet.Count;
                                        int count1 = 1;
                                        foreach (string str121 in paymentSet)
                                        {
                                            modelPOSTransLine tl1 = new modelPOSTransLine();
                                            tl1.paymentType = str121.Split(' ')[0];
                                            tl1.paymentAmount = Decimal.Parse(str121.Split(' ')[1].Replace("$", ""));
                                            if (size == count1) { tl1.paymentChange = posTempLine.paymentChange; }
                                            else { tl1.paymentChange = 0; }
                                            tl1.paymentTotal = tl1.paymentAmount - tl1.paymentChange;
                                            posPayments.Add(tl1);
                                            count1++;
                                        }
                                        string recieptData = "Transaction Number: " + posTrans.transID + Environment.NewLine + "Date: "
                                            + DateTime.Now.ToLongDateString() + Environment.NewLine + Environment.NewLine
                                            // + "Operator ID: " + mPOS.operatorID + Environment.NewLine
                                            + "User ID: " + posTrans.userID + Environment.NewLine
                                            + "Transaction Type: " + posTrans.transType + Environment.NewLine + Environment.NewLine;
                                        string title = "";
                                        foreach (string str12 in itemSet)
                                        {
                                            int count = str12.Split('$').Length - 1;
                                            if (count < 2) { title = str12; }
                                            else { itemSet2.Add(title + "|" + str12); title = ""; }
                                        }
                                        
                                        foreach (string str12 in itemSet2)
                                        {
                                            modelPOSTransLine tl1 = new modelPOSTransLine();
                                            tl1.paymentChange = posTempLine.paymentChange;
                                            tl1.itemTitle = posTempLine.itemTitle;
                                            string str13 = str12.Replace("  ", " ");
                                            tl1.itemTitle = str12.Split('|')[0];
                                            tl1.billReason = str13.Split('|')[1].Split(' ')[0];
                                            if (str13.Split('|')[1].Split(' ')[1].Contains('$'))
                                            {
                                                tl1.itemQuantity = 1;
                                                tl1.itemPrice = Decimal.Parse(str13.Split('|')[1].Split(' ')[1].Replace("$", ""));
                                                tl1.billTotal = Decimal.Parse(str13.Split('|')[1].Split(' ')[1].Replace("$", ""));
                                            }
                                            else
                                            {
                                                tl1.itemQuantity = int.Parse(str13.Split('|')[1].Split(' ')[1]);
                                                tl1.itemPrice = Decimal.Parse(str13.Split('|')[1].Split(' ')[2].Replace("$", "")) / tl1.itemQuantity;
                                                tl1.billTotal = Decimal.Parse(str13.Split('|')[1].Split(' ')[2].Replace("$", ""));
                                            }

                                            posLines.Add(tl1);
                                        }
                                        foreach(modelPOSTransLine tl11 in posLines)
                                        {
                                            recieptData = recieptData + "Bill Reason: " + tl11.billReason + Environment.NewLine;
                                            if (!string.IsNullOrEmpty(tl11.itemTitle)) { recieptData = recieptData + "Title: " + tl11.itemTitle + Environment.NewLine; }
                                            recieptData = recieptData + "Item Quantity: " + tl11.itemQuantity + Environment.NewLine
                                                + "Item Price: " + tl11.itemPrice.ToString() + Environment.NewLine + Environment.NewLine;
                                            recieptData = recieptData + Environment.NewLine;
                                        }
                                        foreach(modelPOSTransLine tl11 in posPayments)
                                        {
                                            recieptData = recieptData + "Payment Type: " + tl11.paymentType + Environment.NewLine;
                                            recieptData = recieptData + "Amount Paid: " + tl11.paymentAmount + Environment.NewLine;
                                            recieptData = recieptData + "Change: " + tl11.paymentChange + Environment.NewLine;
                                            recieptData = recieptData + "Total: " + tl11.paymentTotal + Environment.NewLine + Environment.NewLine;
                                        }
                                        if (posPayments.Count == 1)
                                        {
                                            int countLines = posLines.Count;
                                            int count2 = 1;

                                            foreach(modelPOSTransLine tl1 in posLines)
                                            {
                                                modelPOSTransLine tl2 = new modelPOSTransLine();
                                                tl2 = tl1;
                                                tl2.paymentType = posPayments[0].paymentType;
                                                tl2.idTrans = posTrans.transID;
                                                tl2.idLine = count2;
                                                if (count2 == countLines)
                                                {
                                                    tl2.paymentAmount = posPayments[0].paymentAmount;
                                                    tl2.paymentChange = posPayments[0].paymentChange;
                                                    tl2.paymentTotal = posPayments[0].paymentTotal;
                                                }
                                                else
                                                {
                                                    tl2.paymentAmount = tl2.billTotal;
                                                    tl2.paymentTotal = tl2.billTotal;
                                                    tl2.paymentChange = 0;
                                                    posPayments[0].paymentAmount = posPayments[0].paymentAmount - tl2.paymentAmount;
                                                    posPayments[0].paymentTotal = posPayments[0].paymentTotal - tl2.paymentAmount;
                                                }
                                                
                                                controlPOS.postTransLine(mS, tl2);
                                                count2++;
                                            }
                                        }
                                        else
                                        {
                                            if (posLines.Count == 1)
                                            {
                                                int count2 = 1;
                                                foreach(modelPOSTransLine tl1 in posPayments)
                                                {
                                                    modelPOSTransLine tl2 = new modelPOSTransLine();
                                                    tl2 = posLines[0];
                                                    tl2.idTrans = posTrans.transID;
                                                    tl2.idLine = count2;
                                                    tl2.paymentAmount = tl1.paymentAmount;
                                                    tl2.paymentTotal = tl1.paymentTotal;
                                                    tl2.paymentType = tl1.paymentType;
                                                    tl2.paymentChange = tl2.paymentAmount - tl2.paymentTotal;
                                                    controlPOS.postTransLine(mS, tl2);
                                                }
                                            }
                                            else
                                            {
                                                
                                                formPOSPayment frm = new formPOSPayment();
                                                frm.posLines = posLines;
                                                frm.posPayments = posPayments;
                                                frm.mS = mS;
                                                frm.posTrans = posTrans;
                                                frm.ShowDialog();
                                            }
                                        }
                                        //recieptData = recieptData + "Bill Reason: " + mPOS.reason + Environment.NewLine;
                                        //if (!string.IsNullOrEmpty(mPOS.title)) { recieptData = recieptData + "Title: " + mPOS.title + Environment.NewLine; }
                                        //recieptData = recieptData + "Item Quantity: " + mPOS.quantity + Environment.NewLine
                                        //    + "Item Price: " + mPOS.amtCol.ToString() + Environment.NewLine + Environment.NewLine;
                                        //recieptData = recieptData + Environment.NewLine;
                                        //recieptData = recieptData + "Payment Type: " + mPOS1.type + Environment.NewLine;
                                        //recieptData = recieptData + "Amount Paid: " + mPOS1.amtCol + Environment.NewLine;
                                        //recieptData = recieptData + "Change: " + mPOS1.change + Environment.NewLine + Environment.NewLine;
                                        controlPOS.closeTrans(mS, posTrans);
                                        dataStr = recieptData;
                                        #endregion
                                    }
                                    else if (dataStr.Contains("Refund"))
                                    {
                                        DateTime dt1 = new DateTime();
                                        #region CashManagementTransRefund
                                        //bool libName = false;
                                        //bool paymentLines = false;
                                        //List<string> itemSet = new List<string>();
                                        //List<string> itemSet2 = new List<string>();
                                        //List<string> paymentSet = new List<string>();
                                        //foreach (string str11 in dataStr.Split(Environment.NewLine.ToCharArray()))
                                        //{
                                        //    if (!string.IsNullOrEmpty(str11))
                                        //    {

                                        //        if (DateTime.TryParse(str11, out dt1)) { mPOS.paymentDate = dt1; }
                                        //        if (str11.Contains("User ID:")) { mPOS.userID = str11.Replace("User ID: ", ""); }
                                        //        if (str11.Contains("Operator ID:")) { mPOS.operatorID = str11.Replace("Operator ID: ", ""); }
                                        //        if (str11.Contains("Grand Total:")) { libName = false; paymentLines = true; }
                                        //        if (str11.Contains("Total Payments:")) { paymentLines = false; }
                                        //        if (libName)
                                        //        {
                                        //            itemSet.Add(str11);
                                        //            //MessageBox.Show(str11);
                                        //            int count = str11.TakeWhile(c => c == '$').Count();
                                        //            //MessageBox.Show(count.ToString());
                                        //            if (count > 1)
                                        //            {

                                        //            }
                                        //            else { mPOS.title = str11; }
                                        //        }
                                        //        if (paymentLines)
                                        //        {
                                        //            if (str11.Contains('$') && !str11.Contains("Grand Total:"))
                                        //            {
                                        //                paymentSet.Add(str11);
                                        //            }
                                        //        }
                                        //        if (str11.Contains("Library name:")) { libName = true; }
                                        //        if (str11.Contains("Change:")) { mPOS.change = Decimal.Parse(str11.Replace("Change: $", "")); }

                                        //    }
                                        //}
                                        //mPOS.transType = "Refund";
                                        //string recieptData = "Transaction Number: " + mPOS.transID + Environment.NewLine + "Date: "
                                        //    + mPOS.paymentDate + Environment.NewLine + Environment.NewLine
                                        //    // + "Operator ID: " + mPOS.operatorID + Environment.NewLine
                                        //    + "User ID: " + mPOS.userID + Environment.NewLine
                                        //    + "Transaction Type: " + mPOS.transType + Environment.NewLine + Environment.NewLine;
                                        //string title = "";
                                        //foreach (string str12 in itemSet)
                                        //{
                                        //    int count = str12.Split('$').Length - 1;
                                        //    if (count < 2) { title = str12; }
                                        //    else { itemSet2.Add(title + "|" + str12); title = ""; }
                                        //}
                                        //foreach (string str12 in itemSet2)
                                        //{
                                        //    string str13 = str12.Replace("  ", " ");
                                        //    modelPOS mPOS1 = mPOS;
                                        //    mPOS1.title = str12.Split('|')[0];

                                        //    mPOS1.reason = str13.Split('|')[1].Split(' ')[0];
                                        //    if (str13.Split('|')[1].Split(' ')[1].Contains('$'))
                                        //    {
                                        //        mPOS1.quantity = 1;
                                        //        mPOS1.amtDrawer = Decimal.Parse("-" + str13.Split('|')[1].Split(' ')[1].Replace("$", ""));
                                        //    }
                                        //    else
                                        //    {
                                        //        mPOS1.quantity = int.Parse(str13.Split('|')[1].Split(' ')[1]);
                                        //        mPOS1.amtCol = Decimal.Parse("-" + str13.Split('|')[1].Split(' ')[2].Replace("$", "")) / mPOS1.quantity;
                                        //        mPOS1.totalBill = Decimal.Parse("-" + str13.Split('|')[1].Split(' ')[2].Replace("$", ""));
                                        //    }
                                        //    controlPOS.postItemType(mS, mPOS);
                                        //    recieptData = recieptData + "Item Quantity: " + mPOS.quantity + Environment.NewLine
                                        //        + "Item Price: " + mPOS.amtCol.ToString() + Environment.NewLine + Environment.NewLine;
                                        //    recieptData = recieptData + Environment.NewLine;
                                        //}
                                        //int size = paymentSet.Count;
                                        //int count1 = 1;
                                        //Decimal change = mPOS.change;
                                        ////MessageBox.Show(mPOS.change.ToString());
                                        //foreach (string str12 in paymentSet)
                                        //{
                                        //    modelPOS mPOS1 = mPOS;
                                        //    mPOS1.type = str12.Split(' ')[0];
                                        //    mPOS1.amtCol = Decimal.Parse("-" + str12.Split(' ')[1].Replace("$", ""));
                                        //    if (size == count1) { mPOS1.change = change; }
                                        //    else { mPOS1.change = 0; }
                                        //    mPOS1.amtDrawer = mPOS1.amtCol - mPOS1.change;
                                        //    if (mPOS1.type.Contains("OLSBCC2"))
                                        //    {
                                        //        CCRefund cref = new CCRefund();
                                        //        cref.mPOS = mPOS;
                                        //        cref.mS = mS;
                                        //        cref.ShowDialog();
                                        //    }
                                        //    // MessageBox.Show(mPOS1.transType);
                                        //    controlPOS.postPayment(mS, mPOS1);
                                        //    recieptData = recieptData + "Payment Type: " + mPOS1.type + Environment.NewLine;
                                        //    recieptData = recieptData + "Amount Paid: " + mPOS1.amtCol + Environment.NewLine;
                                        //    recieptData = recieptData + "Change: " + mPOS1.change + Environment.NewLine + Environment.NewLine;
                                        //    count1++;
                                        //}
                                        //controlPOS.closeTrans(mS, mPOS);
                                        //dataStr = recieptData;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region CashManagementTransOther



                                        #endregion
                                    }

                                    #endregion
                                }
                            }
                            catch(Exception e1) {
                                FileControl.fileWriteLog(e1.ToString(), mS);
                                FileControl.fileWriteLog(dataStr,mS);
                                var st = new StackTrace(e1, true);
                                var frame = st.GetFrame(0);
                                var line = frame.GetFileLineNumber();
                                FileControl.fileWriteLog("Exception line number: " + line, mS);
                            }
                        }
                        bool printRec = true;
                        if (mS.switchAskTransit == "1")
                        {
                            if (MessageBox.Show("Does the patron want a reciept for this payment?", "Confirm Payment Reciept", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                            {
                                statsControl.tickStats(mS, "Payment Slips - Skipped");
                                if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Payment Slips - Skipped", mS); }
                                printRec = false;
                            }
                        }
                        if (printRec == true)
                        {
                            statsControl.tickStats(mS, "Payment Slips - Printed");
                            if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Payment Slips - Printed", mS); }
                            printControl.printPage(mS, dataStr, "fine",this);
                        }
                    }
                    else if (str.Contains('\f'))
                    {
                        int checkRec = 1;
                        if (mS.switchAskCheckOut == "1")
                        {
                            if (MessageBox.Show("Does the patron want a checkout reciept?", "Confirm Checkout Reciept", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                            {
                                checkRec = 1;
                            }
                            else
                            {
                                dataStr = File.ReadAllText(mS.fileTempData);
                                if (mS.switchAdminMode == "0") { File.Delete(mS.fileTempData); }
                                foreach (string str44 in dataStr.Split(Environment.NewLine.ToCharArray()))
                                {
                                    if (str44.Length > 5)
                                    {
                                        statsControl.tickStats(mS, "Items Checked out");
                                        if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Items Checked out", mS); }
                                    }
                                }
                                checkRec = 0;
                                statsControl.tickStats(mS, "Checkout Slips - Skipped");
                                if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Checkout Slips - Skipped", mS); }
                            }
                        }

                        if (checkRec == 1)
                        {
                            statsControl.tickStats(mS, "Checkout Slips - Printed");
                            if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Checkout Slips - Printed", mS); }
                            dataStr = File.ReadAllText(mS.fileTempData);
                            printControl.printPage(mS, dataStr, "checkout",this);
                        }
                    }
                    else
                    {
                        FileControl.fileWriteLog("No Templete Applied to last job", mS);
                        FileControl.fileWriteTempData(str, mS);
                    }
                }

            }
            else
            #endregion
            {
                //MessageBox.Show("in args");
                try
                {
                    List<string> sList = new List<string>();

                    FileControl.fileWriteLog(arg, mS);
                    string screenCapture = File.ReadAllText(arg);
                    if (screenCapture.Contains("Check In : Issue Received"))
                    {
                        String converted = FileControl.HTMLtoTXT(screenCapture);
                        foreach (String str11 in converted.Split('^'))
                        {
                            if (str11.Contains("Copy:"))
                            {
                                string result = "";
                                int i = 1;
                                foreach (string str in converted.Split(Environment.NewLine.ToCharArray()))
                                {
                                    if (i == 3) { result = result + "Title: " + str + Environment.NewLine; }
                                    if (str.Contains("Enumeration:")) { result = result + str + Environment.NewLine; }
                                    if (str.Contains("Chronology:")) { result = result + str + Environment.NewLine; }
                                    i++;
                                }
                                foreach (string str12 in str11.Split(Environment.NewLine.ToCharArray()))
                                {
                                    if (str12.Length > 4 && !str12.Contains("Copy")) { result = result + "RouteTo: " + str12 + Environment.NewLine; }
                                }
                                bool printRec = true;
                                if (mS.switchAskTransit == "1")
                                {
                                    if (MessageBox.Show("Do you want to print a serial routing slip?", "Confirm Serial Routing Slip", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                                    {
                                        if (mS.switchAdminMode == "1") { FileControl.fileWriteLog(result, mS); }
                                        statsControl.tickStats(mS, "Serial Routing Slips - Skipped");
                                        printRec = false;
                                    }
                                }
                                if (printRec == true)
                                {
                                    if (mS.switchAdminMode == "1") { FileControl.fileWriteLog(result, mS); }
                                    statsControl.tickStats(mS, "Serial Routing Slips - Printed");
                                    if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("Serial Routing Slips - Printed", mS); }
                                    printControl.printPage(mS, result, "serialRoute",this);
                                }
                            }
                        }
                    }
                    else if (screenCapture.Contains("Display User : Print User"))
                    {
                        bool printRec = true;
                        if (mS.switchAskTransit == "1")
                        {
                            if (MessageBox.Show("Do you want to print a serial routing slip?", "Confirm Serial Routing Slip", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                            {
                                if (mS.switchAdminMode == "1") { FileControl.fileWriteLog(screenCapture, mS); }
                                statsControl.tickStats(mS, "User Record - Skipped");
                                printRec = false;
                            }
                        }
                        if (printRec == true)
                        {
                            if(mS.switchAdminMode == "1") { FileControl.fileWriteLog(screenCapture, mS); }
                            statsControl.tickStats(mS, "User Record - Printed");
                            if (mS.switchAdminMode == "1") { FileControl.fileWriteLog("User Record - Printed", mS); }
                            printControl.printPage(mS, screenCapture, "UserRecord",this);
                        }
                    }
                    else
                    {
                        FileControl.fileWriteLog("Template not found", mS);
                        if (mS.switchAdminMode == "1")
                        {
                            FileControl.fileWriteLog(screenCapture, mS);
                        }
                        Process process = new Process();
                        process.StartInfo.FileName = arg;
                        process.Start();
                    }
                }
                catch (Exception e1)
                {
                    FileControl.fileWriteLog(e1.ToString(), mS);
                }
            }










            //MessageBox.Show("Done");
            this.Close();
        }
    }
}
