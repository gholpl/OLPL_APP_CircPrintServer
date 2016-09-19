using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_CircPrintServer.Models;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Diagnostics;
using DLL_CircPrintServer;
using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;

namespace APP_CircPrintServer.Functions
{
    class controlPOS
    {
        static string error = "";
        internal static void runBackupFiles(modelSettings mS)
        {
                string fileName = controlFunctions.fixVars(mS.POSDataFolder + "dataPOSTrans.data");
                if (File.Exists(fileName))
                {
                    //Debugger.Launch();
                    string[] strFile = File.ReadAllLines(fileName);
                    File.Delete(fileName);
                    foreach (string strLine in strFile)
                    {

                        modelPOSTrans mPOS = new modelPOSTrans();
                        int countEl = 1;
                        foreach (string strElement in strLine.Split('|'))
                        {
                            if (countEl == 1) { mPOS.transID = double.Parse(strElement); }
                            if (countEl == 2) { mPOS.transDate = strElement; }
                            if (countEl == 3)
                            {
                                if (strElement.Length < 4)
                                {
                                    foreach (string strElement2 in strLine.Split('|'))
                                    {
                                        if (strElement2.ToUpper().Contains("OPERATOR ID:")) { mPOS.operatorID = strElement2.Replace("Operator ID: ", string.Empty); }
                                    }
                                    if (mPOS.operatorID == null) { mPOS.operatorID = "Unknown"; }
                                }
                                else { mPOS.operatorID = strElement; }
                            }
                            if (countEl == 4) { mPOS.userID = strElement; }
                            if (countEl == 5) { mPOS.compName = strElement; }
                            if (countEl == 6) { mPOS.stationType = strElement; }
                            if (countEl == 7) { mPOS.transType = strElement; }
                            if (countEl > 7) { mPOS.strData = mPOS.strData + strElement + "|"; }
                            countEl++;
                        }
                        getTransID(mS, mPOS);
                    }

                }
               
        }
        internal static double getTransID(modelSettings mS, modelPOSTrans mPOS)
        {
            error = "";
            string strTransLine = DateTime.Now + "|" + mPOS.operatorID + "|" + mPOS.userID + "|" + mS.machineName + "|" + mPOS.stationType + "|" + mPOS.transType + "|" + mPOS.strData;
            string result = "";
            double ret = 1111111111;
            try
            {
                using (WebClient client = new WebClient { UseDefaultCredentials = true })
                {

                    byte[] response = client.UploadValues(mS.POSServerAPI + "/postranscreate", new NameValueCollection()
                {
                        {"transID",mPOS.transID.ToString() },
                    { "operatorID", mPOS.operatorID },
                    { "userID", mPOS.userID },
                    { "compName", mS.machineName },
                    { "stationType", mPOS.stationType },
                    { "strData", mPOS.strData },
                    { "transType", mPOS.transType },
                        { "transDate", mPOS.transDate }
                });

                    result = System.Text.Encoding.UTF8.GetString(response);
                    //Debugger.Launch();
                    double res = 0;
                    result = result.Replace("\"",string.Empty);
                    if (double.TryParse(result,out res))
                    {
                        if(mS.POSDebugLogging) { FileControl.writeDataLine(mS, "POSDebug.log", DateTime.Now + " - " + mPOS.compName + " Created Trans=" + strTransLine + "|" + result); }
                    }
                    else
                    {
                        
                        if (mPOS.transID < 1)
                        {
                            result = DateTime.Now.ToString("yyyyMMddHHmmss");
                        }
                        else { error = result; result = mPOS.transID.ToString(); }
                        if (!error.Contains("Duplicate"))
                        {
                            strTransLine = result + "|" + strTransLine;
                            FileControl.writeDataLine(mS, "POSDebug.log", DateTime.Now + " - " + mPOS.compName + " Duplicate/Error API Trans=" + strTransLine + "|" + result);
                            FileControl.writeDataLine(mS, "dataPOSTrans.data", strTransLine);
                        }
                        controlFunctions.fileWriteLog(DateTime.Now + " - " + mPOS.compName + " Error Creating Trans=" + strTransLine + " " + error, mS);
                    }
                    ret = double.Parse(result);
                }
            }
            catch (Exception e1)
            {
                FileControl.writeDataLine(mS, "POSDebug.log", DateTime.Now + "-" + Environment.MachineName + " - " + mPOS.compName + " Error Coding Trans=" + strTransLine + "|" + result);
                controlFunctions.fileWriteLog(DateTime.Now + "-" + Environment.MachineName + " - " + mPOS.compName + " Get transID problem " + e1.ToString() + " -- " + strTransLine, mS);
                if (mPOS.transID < 1)
                {
                    result = DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                else { error = result; result = mPOS.transID.ToString(); }
                strTransLine = result + "|" + strTransLine;
                FileControl.writeDataLine(mS, "dataPOSTrans.data", strTransLine);
                ret = long.Parse(result);
            }
           
            return ret;
        }
        internal static void postTransLine(modelSettings mS, modelPOSTransLine mPOS)
        {
            string result = "";
            string strTransLine = "";
            try
            {
                
                strTransLine = DateTime.Now + "|" + mPOS.idTrans.ToString() + "|" + mPOS.idLine.ToString() + "|" + mPOS.itemID + "|" +
                     mPOS.itemTitle + "|" + mPOS.itemCallNumber + "|" + mPOS.itemAuthor + "|" + mPOS.itemQuantity.ToString() + "|" + mPOS.itemPrice.ToString() + "|" +
                     mPOS.paymentType.ToString() + "|" + mPOS.paymentAmount.ToString() + "|" + mPOS.paymentChange.ToString() + "|" + mPOS.paymentTotal.ToString() + "|" +
                     mPOS.billReason.ToString() + "|" + mPOS.billTotal.ToString();
            
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
                    result = result.Replace("\\", "");
                    if (result != "\"0\"")
                    {
                        controlFunctions.fileWriteLog(DateTime.Now + "-" + Environment.MachineName + " Error Creating TransLine=" + strTransLine + " -- " + result, mS);
                        FileControl.writeDataLine(mS, "POSDebug.log", DateTime.Now + "-" + Environment.MachineName + " Duplicate/Error API TransLine=" + strTransLine + "|" + result);
                        FileControl.writeDataLine(mS, "dataPOSTransLine.data", strTransLine);
                    }
                    else { if (mS.POSDebugLogging) { FileControl.writeDataLine(mS, "POSDebug.log", DateTime.Now + "-" + Environment.MachineName + " Created TransLine=" + strTransLine + "|" + result); } }
                }
            }
            catch (Exception e1)
            {
                FileControl.writeDataLine(mS, "POSDebug.log", DateTime.Now + "-"+Environment.MachineName + " Error Coding TransLine=" + strTransLine + "|" + e1.ToString());
                controlFunctions.fileWriteLog(DateTime.Now + "-" + Environment.MachineName + " Create transline problem " + e1.ToString() + " -- " + strTransLine, mS);
                FileControl.writeDataLine(mS, "dataPOSTransLine.data", strTransLine);
            }
        }
        internal static void closeTrans(modelSettings mS, modelPOSTrans mPOS)
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
            catch (Exception e1) { controlFunctions.fileWriteLog(DateTime.Now + "Close trans problem " + e1.ToString(), mS); }
            //return int.Parse(result);
        }
        internal static string postCCRefund(modelSettings mS, modelPOSTrans mPOS,string cc4digit, string contactPreference,string contactInfo, string note, string recieptNumber)
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
            catch (Exception e1) { controlFunctions.fileWriteLog(DateTime.Now + "Post refund problem " + e1.ToString(), mS); return e1.ToString(); }
           
        }
        internal static string emailRefund(modelSettings mS, modelPOSTrans mPOS,string body)
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
            catch (Exception e1) { controlFunctions.fileWriteLog(DateTime.Now + "Send Refund Email problem " + e1.ToString(), mS); return "Error"; }
        }
        
    }
}
