using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_CircPrintServer.Models;
using System.Collections.Specialized;
using System.Net;

namespace APP_CircPrintServer.Functions
{
    class controlPOS
    {
        internal static int getTransID(modelSettings1 mS, modelPOSTrans mPOS)
        {
            string result = "";
            try
            {
                
                using (WebClient client = new WebClient { UseDefaultCredentials = true })
                {

                    byte[] response = client.UploadValues(mS.POSServerAPI+ "/postranscreate", new NameValueCollection()
                {
                    { "operatorID", mPOS.operatorID },
                    { "userID", mPOS.userID },
                    { "compName", mS.machineName },
                    { "stationType", mPOS.stationType },
                    { "strData", mPOS.strData },
                    { "transType", mPOS.transType }
                });

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }
            catch (Exception e1) { FileControl.fileWriteLog(e1.ToString(), mS); return 0; }
            return int.Parse(result);
        }
        internal static void postTransLine(modelSettings1 mS, modelPOSTransLine mPOS)
        {
            string result = "";
            try
            {

                using (WebClient client = new WebClient { UseDefaultCredentials = true })
                {

                    byte[] response = client.UploadValues(mS.POSServerAPI + "/POSTransLineCreate", new NameValueCollection()
                {
                    { "idTrans", mPOS.idTrans.ToString() },
                    { "idLine", mPOS.idLine.ToString() },
                    { "itemID", mPOS.itemID },
                    { "itemTitle", mPOS.itemTitle },
                    { "itemCallNumber", mPOS.itemCallNumber },
                    { "itemAuthor", mPOS.itemAuthor },
                    { "itemQuantity", mPOS.itemQuantity.ToString() },
                    { "itemPrice", mPOS.itemPrice.ToString() },
                    { "paymentType", mPOS.paymentType.ToString() },
                    { "paymentAmount", mPOS.paymentAmount.ToString() },
                    { "paymentChange", mPOS.paymentChange.ToString() },
                    { "paymentTotal", mPOS.paymentTotal.ToString() },
                    { "billReason", mPOS.billReason.ToString() },
                    { "billTotal", mPOS.billTotal.ToString() }
                });

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }
            catch (Exception e1) { FileControl.fileWriteLog(e1.ToString(), mS); }
            //return int.Parse(result);
        }
        internal static void closeTrans(modelSettings1 mS, modelPOSTrans mPOS)
        {
            string result = "";
            try
            {

                using (WebClient client = new WebClient { UseDefaultCredentials = true })
                {

                    byte[] response = client.UploadValues(mS.POSServerAPI + "/postranscomplete", new NameValueCollection()
                {
                    { "transID", mPOS.transID.ToString() }
                });

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }
            catch (Exception e1) { FileControl.fileWriteLog(e1.ToString(), mS); }
            //return int.Parse(result);
        }
        internal static string postCCRefund(modelSettings1 mS, modelPOS mPOS,string cc4digit, string contactPreference,string contactInfo, string note, string recieptNumber)
        {
            string result = "";
            try
            {

                using (WebClient client = new WebClient { UseDefaultCredentials = true })
                {

                    byte[] response = client.UploadValues(mS.POSServerAPI + "/POSCCRefundCreate", new NameValueCollection()
                {
                    { "transID", mPOS.transID.ToString() },
                    { "cc4digits", cc4digit },
                    { "contactPreference", contactPreference },
                    { "contactInfo", contactInfo },
                    { "note", note },
                    { "recieptNumber", recieptNumber },
                    { "stationType", mPOS.stationType }

                });

                    result = System.Text.Encoding.UTF8.GetString(response);
                }
                return "DOne";
            }
            catch (Exception e1) { FileControl.fileWriteLog(e1.ToString(), mS); return e1.ToString(); }
           
        }
        internal static string emailRefund(modelSettings1 mS, modelPOS mPOS,string body)
        {
            try {
                using (WebClient client = new WebClient { UseDefaultCredentials = true })
                {
                    string result = "";
                    byte[] response = client.UploadValues("http://api.olpl.org/api/sendemail", new NameValueCollection()
                    {
                         { "from","POSSystem@olpl.org" },
                         { "to", mS.POSServerEmailRefund },
                         { "subject", "POS System CC refund notification" },
                         { "body",body },
                    });
                    result = System.Text.Encoding.UTF8.GetString(response);

                    if (result.Contains("OK"))
                    {
                        return "OK";
                    }
                    else
                    {
                        return "Error";
                    }
                }
            }
            catch (Exception e1) { FileControl.fileWriteLog(e1.ToString(), mS); return "Error"; }
        }
    }
}
