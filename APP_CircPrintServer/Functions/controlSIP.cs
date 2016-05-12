using DLL_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clc;
using Clc.Sip;
using APP_CircPrintServer.Models;
using APP_CircPrintServer.SIP2;

namespace APP_CircPrintServer.Functions
{
    class controlSIP
    {
        private sip2 sipclient;

        public static string getPatronInfo(modelSettings1 mS, modelPOS mPOS)
        {
            string name = "";
            string address = "";
            string phone = "";
            string email = "";

            sip2 sipclient;
            try
            {
                sipclient = new sip2(mS.SIPServerIP, int.Parse(mS.SIPServerPort), "", "");
                sipclient.Connect();
            }
            catch (Exception ex)
            {
                return ex.Message;
                // MessageBox.Show(string.Format("Error connecting to SIP Server: {0}", ex.Message));
            }
            try
            {
                var result = sipclient.Login(mS.SIPUserName, mS.SIPUserPassword, "", "");
                //MessageBox.Show(LogOutput(result));
                string strUser = LogOutput(sipclient.PatronInformation(mPOS.userID, "", "none"));
                if (strUser.Contains("User not found"))
                {
                    return "User Not found in swan";
                    //MessageBox.Show("User Not found in swan");
                }
                else
                {
                    //MessageBox.Show(strUser);
                    string[] strValues = strUser.Split('|');
                    foreach (string str in strValues)
                    {
                        if (str.Remove(2).Contains("AE") && str.Contains(",")) { name = str.Remove(0, 2); }
                        if (str.Remove(2).Contains("BF") && str.Length > 9) { phone = str.Remove(0, 2); }
                        if (str.Remove(2).Contains("BD")) { address = str.Remove(0, 2); }
                        if (str.Remove(2).Contains("BE")) { email = str.Remove(0, 2); }
                        //if (str.Remove(2).Contains("BV")) { fc.tbFineSwan.Text = str.Remove(0, 2); }
                        //if (str.Remove(2).Contains("PH")) { fc.tbPatronTypeSwan.Text = str.Remove(0, 2); }
                        //if (str.Remove(2).Contains("PE")) { if (fc.tbGenderSwan.Text == "") { fc.tbGenderSwan.Text = str.Remove(0, 2); } }
                        //if (str.Remove(2).Contains("PF")) { if (fc.tbAgeGroupSwan.Text == "") { fc.tbAgeGroupSwan.Text = str.Remove(0, 2); } }
                        //if (str.Remove(2).Contains("PD"))
                        //{
                        //    DateTime dt = new DateTime();
                        //    string date1 = str.Remove(0, 2);
                        //    if (date1.Length > 6)
                        //    {
                        //        date1 = date1.Insert(4, "/").Insert(7, "/");
                        //        dt = DateTime.Parse(date1);
                        //        fc.tbBDateSwan.Text = dt.ToShortDateString();
                        //    }
                        //}
                        //if (str.Remove(2).Contains("PA") && !str.Contains("PATRON"))
                        //{
                        //    DateTime dt = new DateTime();
                        //    string date = str.Remove(0, 2);
                        //    if (date.Length > 8)
                        //    {
                        //        date = date.Insert(4, "/").Insert(7, "/").Remove(10);
                        //        dt = DateTime.Parse(date);
                        //        fc.tbExpDateSwan.Text = dt.ToShortDateString();
                        //    }
                        //}
                    }
                }
                return name + Environment.NewLine + address + Environment.NewLine + phone + Environment.NewLine + email;
                //MessageBox.Show(LogOutput(sipclient.PatronInformation(fc.tbBarcode.Text, "", "fine")));
            }
            catch (Exception ex1)
            {
                return (ex1.StackTrace);
            }
        }

        public static string LogOutput(SipTransaction txn)
        {
            var message = txn.Message.Replace("\r", "").Replace("\n", "");
            var response = txn.Response.Replace("\r", "").Replace("\n", "");
            return response;
        }
        internal string checkoutItem(string barcode, string item, modelSettings1 mS)
        {
            try
            {
                sipclient = new sip2(mS.SIPServerIP, int.Parse(mS.SIPServerPort), "", "");
                sipclient.Connect();
            }
            catch (Exception ex)
            {
                FileControl.fileWriteLog(ex.Message + " textline intransit library ILLIBS Checkout Error connecting to SIP Server", mS);
                return "none";
            }
            try
            {
                var result = sipclient.Login(mS.SIPUserName, mS.SIPUserPassword, "", "");
                //MessageBox.Show(LogOutput(result));
                string strUser = LogOutput(sipclient.ItemCheckOut(barcode, "", item, null, "Y", "", "", "Y", "N"));
                if (!strUser.ToUpper().Contains("CHARGED"))
                {
                    FileControl.fileWriteLog("textline intransit library ILLIBS Checkout Not CHecked Out", mS);
                    return "none";
                }
                else
                {
                    string[] strValues = strUser.Split('|');
                    foreach (string str in strValues)
                    {
                        if (str.Remove(2).Contains("AH") && str.Contains(",")) { return str.Remove(0, 2); }
                    }

                 }
            }
            catch (Exception ex1)
            {
                FileControl.fileWriteLog(ex1.Message + " textline intransit library ILLIBS Checkout", mS);
                return "none";
            }
            return "none";
        }
    }
}
