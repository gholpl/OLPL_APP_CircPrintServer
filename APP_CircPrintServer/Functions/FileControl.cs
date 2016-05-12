using APP_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;

namespace APP_CircPrintServer.Functions
{
    class FileControl
    {
        static public modelSettings1 proccessSettingFile()
        {
            modelSettings1 mS = new modelSettings1();
            mS.customSettings = new List<modelSettingCustom>();
            //MessageBox.Show(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase));
            string pathEXE = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");
            if (!pathEXE.Contains(@":")) { pathEXE = @"\\" + pathEXE; }
            // MessageBox.Show(pathEXE);
            
            mS.pathEXE = pathEXE;
            mS.machineName = Environment.MachineName;
            try
            {
                List<modelSettings> set1 = ControlSettings.readSettingFile(mS.pathEXE);
               // MessageBox.Show(mS.pathEXE);
                foreach (modelSettings s1 in set1)
                {
                    
                    if (s1.name.Contains("fileNameTemplate"))
                        {
                           mS.tempLocation = fixVars(s1.value);
                        }
                        else if (s1.name.Contains("template"))
                        {
                            if (s1.name.Contains("Circ"))
                            {
                                mS.tempCirc = fixVars(s1.value);
                            }
                            if (s1.name.Contains("InTransit"))
                            {
                                mS.tempInTransit = fixVars(s1.value);
                            }
                            if (s1.name.Contains("HoldsPage1"))
                            {
                                mS.tempHoldsP1 = fixVars(s1.value);
                            }
                            if (s1.name.Contains("HoldsPage2"))
                            {
                                mS.tempHoldsP2 = s1.value;
                            }
                            if (s1.name.Contains("Fine"))
                            {
                                mS.tempFine = s1.value;
                            }
                            if (s1.name.Contains("SerialRoute"))
                            {
                                mS.tempSerialRoute = s1.value;
                            }
                            if (s1.name.Contains("UserRecord"))
                            {
                                mS.tempUserRecord = s1.value;
                            }
                        }
                        else if (s1.name.Contains("printer"))
                        {
                            if (s1.name.Contains("Checkout"))
                            {
                                    mS.printerCirc = s1.value;
                            }
                            if (s1.name.Contains("InTransit"))
                            {
                                    mS.printerInTransit = s1.value;
                            }
                            if (s1.name.Contains("Holds"))
                            {
                                  mS.printerHolds = s1.value;
                            }
                            if (s1.name.Contains("Fine"))
                            {
                                    mS.printerFine = s1.value;
                            }
                            if (s1.name.Contains("SerialRoute"))
                            {
                                mS.printerSerialRoute = s1.value;
                            }
                            if (s1.name.Contains("UserRecord"))
                            {
                                mS.printerUserRecord = s1.value;
                            }
                        }
                        else if (s1.name.Contains("fileName"))
                        {
                            if (s1.name.Contains("InTransitFrom"))
                            {
                                mS.fileInTransitFrom = fixVars(s1.value);
                            }
                            if (s1.name.Contains("InTransitTo"))
                            {
                                mS.fileInTransitTo = fixVars(s1.value);
                            }
                            if (s1.name.Contains("Data"))
                            {
                                mS.fileTempData = fixVars(s1.value);
                            }
                            if (s1.name.Contains("Log"))
                            {
                                mS.fileLog = fixVars(s1.value);
                            }
                        }
                        else if (s1.name.Contains("notification"))
                        {
                            if (s1.name.Contains("HoldsCallSwitch"))
                            {
                                mS.notificationHoldsCallSwitch = s1.value;
                            }
                            if (s1.name.Contains("HoldsAPICallSwitch"))
                            {
                                mS.notificationHoldsAPICallSwitch = s1.value;
                            }
                            if (s1.name.Contains("HoldsCallServer"))
                            {
                                mS.notificationHoldsCallServer = s1.value;
                            }
                            if (s1.name.Contains("HoldsCallVoipServer"))
                            {
                                mS.notificationHoldsCallVoipServer = s1.value;
                            }
                            if (s1.name.Contains("HoldsCallVoipPort"))
                            {
                                mS.notificationHoldsCallVoipPort = s1.value;
                            }
                            if (s1.name.Contains("HoldsCallVoipUserName"))
                            {
                               mS.notificationHoldsCallVoipUserName = s1.value;
                            }
                            if (s1.name.Contains("HoldsCallVoipPassword"))
                            {
                                mS.notificationHoldsCallVoipPassword = s1.value;
                            }
                        }
                        else if (s1.name.Contains("stats"))
                        {
                            mS.statsSwitch = s1.value;
                            mS.statsServer = s1.value;
                        }
                        else if (s1.name.Contains("twoPageHolds"))
                        {
                            mS.switchTwoPageHolds = s1.value;
                        }
                        else if (s1.name.Contains("recieptCheckoutAsk")) { mS.switchAskCheckOut = s1.value; }
                        else if (s1.name.Contains("recieptUserAsk")) { mS.switchAskUser = s1.value; }
                        else if (s1.name.Contains("recieptPaymentAsk")) { mS.switchAskPayment = s1.value; }
                        else if (s1.name.Contains("recieptHoldsAsk")) { mS.switchAskHolds = s1.value; }
                        else if (s1.name.Contains("recieptTransitAsk")) {mS.switchAskTransit = s1.value; }
                        else if (s1.name.Contains("recieptSerialAsk")) { mS.switchAskSerial = s1.value; }
                        else if (s1.name.Contains("adminDebugMode")) { mS.switchAdminMode=s1.value; }
                        else if (s1.name.Contains("POS"))
                        {
                            if (s1.name.Contains("POSReportingEnable")) { mS.POSEnable = bool.Parse(s1.value); }
                            if (s1.name.Contains("POSServerAPI")) { mS.POSServerAPI = s1.value; }
                            if (s1.name.Contains("POSEmailEnable")) { mS.POSEmailEnable = bool.Parse(s1.value); }
                            if (s1.name.Contains("POSServerEmailAPI")) { mS.POSServerEmailAPI = s1.value; }
                            if (s1.name.Contains("POSServerEmailRefund")) { mS.POSServerEmailRefund = s1.value; }

                        }
                        else if (s1.name.Contains("SIP"))
                        {
                            if (s1.name.Contains("SIPServerIP")) { mS.SIPServerIP = s1.value; }
                            if (s1.name.Contains("SIPServerPort")) { mS.SIPServerPort = s1.value; }
                            if (s1.name.Contains("SIPUserName")) { mS.SIPUserName = s1.value; }
                            if (s1.name.Contains("SIPUserPassword")) { mS.SIPUserPassword = s1.value; }

                        }
                        else
                        {
                            modelSettingCustom sc1 = new modelSettingCustom();
                            sc1.name = s1.name;
                            sc1.value = s1.value;
                            mS.customSettings.Add(sc1);
                        }

                    }
                
                return mS;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
                return mS;
            }
        }
        public static string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
        }
        public static string HTMLtoTXT(string str)
        {
            string result = "";
            string str1 = StripHTML(str);
            foreach(string str11 in str1.Split(Environment.NewLine.ToCharArray()))
            {
                if (str11 != string.Empty)
                {
                    if (str11.Contains("Copy")) { result = result + "^"; }
                    result = result + str11;
                    if (!str11.Contains(":")) { result = result + Environment.NewLine; }  
                }
            }
            return result;
        }
        static public void fileWriteLog(string str,modelSettings1 mS)
        {
            StreamWriter log;
            mS.fileLog = fixVars(mS.fileLog);
            if (!File.Exists(mS.fileLog))
            {
                log = File.CreateText(mS.fileLog);
            }
            else
            {
                log = File.AppendText(mS.fileLog);
            }
            log.WriteLine(str);
            log.Close();
        }
        static public void fileWriteTempData(string str,modelSettings1 mS)
        {
            try
            {
                string result="";
                StreamWriter data;
                if (!File.Exists(mS.fileTempData))
                {
                    data = File.CreateText(mS.fileTempData);
                }
                else
                {
                    data = File.AppendText(mS.fileTempData);
                }
                string[] str_Run1 = str.Split(Environment.NewLine.ToCharArray());
                foreach (string str4 in str_Run1)
                {
                    result = result + str4 + "^";
                }
                data.WriteLine(result);
                data.Close();
            }
           catch(Exception e)
            {
                fileWriteLog("Writing to Temp Data File Failed " + e.Message, mS);
            }

        }
        static public List<modelElement> readTemplateFile(string tempLocation,modelSettings1 mS)
        {
            try
            {
                List<modelElement> lEl = new List<modelElement>();
                XmlDocument doc = new XmlDocument();
                doc.Load(mS.pathEXE + "\\" + tempLocation + ".template");
                XmlElement Main;
                Main = doc.DocumentElement;
                XmlNodeList Element = Main.GetElementsByTagName("Element");

                foreach (XmlNode node in Element)
                {
                    modelElement ec1 = new modelElement();
                    foreach (XmlNode nodeinner in node.ChildNodes)
                    {
                        if (nodeinner.Name == "id")
                        {
                            ec1.id = int.Parse(nodeinner.InnerText);
                        }
                        if (nodeinner.Name == "name")
                        {
                            ec1.name = nodeinner.InnerText;
                        }
                        if (nodeinner.Name == "data")
                        {
                            ec1.data = nodeinner.InnerText;
                        }
                        if (nodeinner.Name == "align")
                        {
                            ec1.align = nodeinner.InnerText;
                        }
                        if (nodeinner.Name == "font")
                        {
                            if (nodeinner.InnerText.Length > 5)
                            {
                                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                                ec1.fontName = (Font)converter.ConvertFromString(nodeinner.InnerText);
                            }
                        }
                        if (nodeinner.Name == "spaceTop")
                        {
                            ec1.spaceTop = int.Parse(nodeinner.InnerText);
                        }
                        if (nodeinner.Name == "width")
                        {
                            ec1.width = int.Parse(nodeinner.InnerText);
                        }
                        if (nodeinner.Name == "height")
                        {
                            ec1.height = int.Parse(nodeinner.InnerText);
                        }
                    }
                    lEl.Add(ec1);
                }
                return lEl;

            }
            catch (Exception e)
            {
               FileControl.fileWriteLog(e.Message,mS);
                modelElement el1 = new modelElement();
                List<modelElement> lEl = new List<modelElement>();
                el1.data = "Error";
                lEl.Add(el1);
                return lEl ;
            }
        }
        static internal string readTransitTo(modelSettings1 mS, string code, string type)
        {
            string str = "none";
            try
            {
                string[] lineArray = File.ReadAllLines(fixVars(mS.fileInTransitTo));
                foreach (string line in lineArray)
                {
                    if (line.Contains(code.Replace("Transit To: ", "")))
                    {
                        foreach (string lineEl in line.Split('|'))
                        {
                            if (type == "barcode")
                            {
                                if (lineEl.Contains("Barcode:")) { str = lineEl.Replace(" Barcode:", "").Replace(" ", string.Empty); }
                            }
                            else if (type == "name")
                            {
                                if (lineEl.Contains("TO: ")) { str = lineEl.Replace(" TO: ", ""); }
                            }
                            else if (type == "city")
                            {
                                if (lineEl.Contains("CITY: ")) { str = lineEl.Replace(" CITY: ", ""); }
                            }
                        }
                    }
                }
                return str;
            }
            catch (Exception e) { return e.Message; }
        }
        static string fixVars(string input)
        {
            //MessageBox.Show(input);
            if (string.IsNullOrEmpty(input)){ input = ""; }
            string data = "";
            bool none = true;
            if (input.Contains("<<<") && input.Contains(">>>"))
            {

            }
            if (input.Contains("<exe>"))
            {
                data = input.Replace("<exe>", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\");
                none = false;
            }
            if (input.Contains("<longdate>"))
            {
                data = input.Replace("<longdate>", DateTime.Now.ToString());
                none = false;
            }
            if (input.Contains("<shortdate>"))
            {
                data = input.Replace("<shortdate>", DateTime.Now.ToShortDateString());
                none = false;
            }
            if (input.Contains("<temp>"))
            {
                data = input.Replace("<temp>", System.IO.Path.GetTempPath());
                none = false;
            }
            if (none == true) { data = input; }
            return data;
        }
        public static string GetUserRecord(string data, string type)
        {
            string typeStr = "";
            string basicinfo = "";
            string demoInfo = "";
            string circinfo = "";
            string address = "";
            string extinfo = "";
            string checkouts = "";
            string bills = "";
            string holds = "";
            foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
            {
                if (line.Contains("Basic Info")||line.Contains("USER ID:")) { typeStr = "Basic"; }
                if (line.Contains("Demographic Info")) { typeStr = "demoInfo"; }
                if (line.Contains("Circulation Info")) { typeStr = "circinfo"; }
                if (line.Contains("User Address")) { typeStr = "address"; }
                if (line.Contains("Extended Information")) { typeStr = "extinfo"; }
                if (line.ToLower().Contains("checkouts")) { typeStr = "checkouts"; }
                if (line.ToLower().Contains("bills")) { typeStr = "bills"; }
                if (line.ToLower().Contains("holds")) { typeStr = "holds"; }

                if (typeStr == "Basic") { basicinfo = basicinfo + line + Environment.NewLine; }
                if (typeStr == "demoInfo") { demoInfo = demoInfo + line + Environment.NewLine; }
                if (typeStr == "circinfo") { circinfo = circinfo + line + Environment.NewLine; }
                if (typeStr == "address") { address = address + line + Environment.NewLine; }
                if (typeStr == "extinfo") { extinfo = extinfo + line + Environment.NewLine; }
                if (typeStr == "checkouts") { checkouts = checkouts + line + Environment.NewLine; }
                if (typeStr == "bills") { bills = bills + line + Environment.NewLine; }
                if (typeStr == "holds") { holds = holds + line + Environment.NewLine; }
            }
            if (type == "basic") {return basicinfo;}
            else if (type == "demoinfo") { return demoInfo; }
            else if (type == "circinfo") { return circinfo; }
            else if (type == "address") { return address; }
            else if (type == "extinfo") { return extinfo; }
            else if (type == "checkouts") { return checkouts; }
            else if (type == "bills") { return bills; }
            else if (type == "holds") { return holds; }
            else { return "none"; }
        }
        public static string HTMLtoTXTUser(string str)
        {
            string result = "";
            string str1 = StripHTML(str).Replace("\r", "");
            foreach (string str11 in str1.Split(Environment.NewLine.ToCharArray()))
            {
                if (str11.TrimEnd().Length > 4)
                {
                    result = result + str11 + Environment.NewLine;
                }
            }
            return result;
        }
    }
}
