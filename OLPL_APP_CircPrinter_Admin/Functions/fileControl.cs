using OLPL_APP_CircPrinter_Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Web;
using DLL_CircPrintServer;
using DLL_CircPrintServer.Models;
using DLL_CircPrintServer.Classes;

namespace OLPL_APP_CircPrinter_Admin.Functions
{
    public class fileControl
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
           string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        static public List<templatesClass> getTemplates(Form1 fc)
        {
                string fileloc = "";
                List<templatesClass> tc = new List<templatesClass>();
                if (fc.tb_file_temp.Text.Contains("<exe>")) { fileloc = fc.tb_file_temp.Text.Replace("<exe>", fc.pathEXE) + "\\"; }
                else { fileloc = fc.tb_file_temp.Text + "\\"; }
                string[] array1 = Directory.GetFiles(fileloc, "*.template");
                templatesClass tc1 = new templatesClass();
                tc1.name = "Add New Template";
                tc.Add(tc1);
                foreach (string str in array1)
                {
                    tc.Add(readTemplateFile(str, fc.log));
                }
            return tc;
        }
        static public templatesClass readTemplateFile(string location, StreamWriter log)
        {
            try
            {
                templatesClass tc = new templatesClass();
                tc.element = new List<elementClass>();
                XmlDocument doc = new XmlDocument();
                doc.Load(location);
                XmlElement Main;
                Main = doc.DocumentElement;
                tc.name = Main.GetAttributeNode("Name").InnerText;
                tc.type = Main.GetAttributeNode("Type").InnerText;
                tc.fileName = location;
                //MessageBox.Show(Main.GetAttributeNode("Type").InnerText);
                XmlNodeList Element = Main.GetElementsByTagName("Element");

                foreach (XmlNode node in Element)
                {
                    elementClass ec1 = new elementClass();
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
                    tc.element.Add(ec1);
                }
                return tc;

            }
            catch (Exception e)
            {
                log.WriteLine(e.Message);
                templatesClass tc = new templatesClass();
                tc.name = "Error";
                return tc;
            }
        }
        static public void proccessSettingFile(Form1 fc)
        {
            try
            {
                List<modelSettings> set1 =  ControlSettings.readSettingFile(fc.pathEXE);

                foreach(modelSettings s1 in set1)
                {
                  // MessageBox.Show(s1.name + " " + s1.value);
                    if (s1.name.Contains("fileNameTemplate"))
                    {
                        fc.tb_file_temp.Text = s1.value;
                    }
                    else if (s1.name.Contains("template"))
                    {
                        if (s1.name.Contains("Circ"))
                        {
                            fc.tempCirc = s1.value;
                        }
                        if (s1.name.Contains("InTransit"))
                        {
                            fc.tempInTransit = s1.value;
                        }
                        if (s1.name.Contains("HoldsPage1"))
                        {
                            fc.tempHoldsP1 = s1.value;
                        }
                        if (s1.name.Contains("HoldsPage2"))
                        {
                            fc.tempHoldsP2 = s1.value;
                        }
                        if (s1.name.Contains("Fine"))
                        {
                            fc.tempFine = s1.value;
                        }
                        if (s1.name.Contains("SerialRoute"))
                        {
                            fc.tempSerialRoute = s1.value;
                        }
                        if (s1.name.Contains("UserRecord"))
                        {
                            fc.tempUserRecord = s1.value;
                        }
                    }
                     else if (s1.name.Contains("printer"))
                        {
                            if (s1.name.Contains("Checkout"))
                            {
                                if (fc.cbCircPrinter.Items.Contains(s1.value))
                                {
                                    fc.cbCircPrinter.SelectedItem = s1.value;
                                }
                                else
                                {
                                    fc.cbCircPrinter.Items.Add(s1.value + " (Not Found)");
                                    fc.cbCircPrinter.SelectedText = s1.value + " (Not Found)";
                                }
                            }
                            if (s1.name.Contains("InTransit"))
                            {
                                if (fc.cbInTransitPrinter.Items.Contains(s1.value))
                                {
                                    fc.cbInTransitPrinter.SelectedItem = s1.value;
                                }
                                else
                                {
                                    fc.cbInTransitPrinter.Items.Add(s1.value + " (Not Found)");
                                    fc.cbInTransitPrinter.SelectedText = s1.value + " (Not Found)";
                                }
                            }
                            if (s1.name.Contains("Holds"))
                            {
                                if (fc.cbHoldsPrinter.Items.Contains(s1.value))
                                {
                                    fc.cbHoldsPrinter.SelectedItem = s1.value;
                                }
                                else
                                {
                                    fc.cbHoldsPrinter.Items.Add(s1.value + " (Not Found)");
                                    fc.cbHoldsPrinter.SelectedText = s1.value + " (Not Found)";
                                }
                            }
                            if (s1.name.Contains("Fine"))
                            {
                                if (fc.cbFinePrinter.Items.Contains(s1.value))
                                {
                                    fc.cbFinePrinter.SelectedItem = s1.value;
                                }
                                else
                                {
                                    fc.cbFinePrinter.Items.Add(s1.value + " (Not Found)");
                                    fc.cbFinePrinter.SelectedText = s1.value + " (Not Found)";
                                }
                            }
                            if (s1.name.Contains("SerialRoute"))
                            {
                                if (fc.cbSerialRoutePrinter.Items.Contains(s1.value))
                                {
                                    fc.cbSerialRoutePrinter.SelectedItem = s1.value;
                                }
                                else
                                {
                                    fc.cbSerialRoutePrinter.Items.Add(s1.value + " (Not Found)");
                                    fc.cbSerialRoutePrinter.SelectedText = s1.value + " (Not Found)";
                                }
                            }
                            if (s1.name.Contains("UserRecord"))
                            {
                                if (fc.cbUserRecordPrinter.Items.Contains(s1.value))
                                {
                                    fc.cbUserRecordPrinter.SelectedItem = s1.value;
                                }
                                else
                                {
                                    fc.cbUserRecordPrinter.Items.Add(s1.value + " (Not Found)");
                                    fc.cbUserRecordPrinter.SelectedText = s1.value + " (Not Found)";
                                }
                            }
                        }
                    else if (s1.name.Contains("fileName"))
                    {
                        if (s1.name.Contains("InTransitFrom"))
                        {
                            fc.tbInTransitFromLoc.Text = s1.value;
                        }
                        if (s1.name.Contains("InTransitTo"))
                        {
                            fc.tbInTransitToLoc.Text = s1.value;
                        }
                        if (s1.name.Contains("Data"))
                        {
                            fc.tbTempDataLoc.Text = s1.value;
                        }
                        if (s1.name.Contains("Log"))
                        {
                            fc.tbLogLoc.Text = s1.value;
                        }
                    }
                    else if (s1.name.Contains("stats"))
                    {
                        if (s1.name.Contains("ON")) { fc.tbStatsOnOff.Value = int.Parse(s1.value); }
                        if (s1.name.Contains("Server")) { fc.tbStatsServer.Text = s1.value; }
                    }
                    else if (s1.name.Contains("notification"))
                    {
                        if (s1.name.Contains("HoldsCallSwitch"))
                        {
                            fc.tbarNotificationHoldsCall.Value = int.Parse(s1.value);
                        }
                        if (s1.name.Contains("HoldsAPICallSwitch"))
                        {
                            fc.tbarNotificationHoldsCallAPI.Value = int.Parse(s1.value);
                        }
                        if (s1.name.Contains("HoldsCallServer"))
                        {
                            fc.tboxNotificationHoldsCallServer.Text = s1.value;
                        }
                        if (s1.name.Contains("HoldsCallVoipServer"))
                        {
                            fc.tboxVoipServer.Text = s1.value;
                        }
                        if (s1.name.Contains("HoldsCallVoipPort"))
                        {
                            fc.tBoxVoipServerPort.Text = s1.value;
                        }
                        if (s1.name.Contains("HoldsCallVoipUserName"))
                        {
                            fc.tBoxVoipUserName.Text = s1.value;
                        }
                        if (s1.name.Contains("HoldsCallVoipPassword"))
                        {
                            fc.tBoxVoipPassword.Text = s1.value;
                        }
                    }
                    else if (s1.name.Contains("twoPageHolds"))
                    {
                        fc.tbTwoPageHolds.Value = int.Parse(s1.value);
                    }
                    else if (s1.name.Contains("recieptCheckoutAsk")) { fc.tbAskCheckout.Value = int.Parse(s1.value); }
                    else if (s1.name.Contains("recieptUserAsk")) { fc.tbAskUser.Value = int.Parse(s1.value); }
                    else if (s1.name.Contains("recieptPaymentAsk")) { fc.tbAskPayment.Value = int.Parse(s1.value); }
                    else if (s1.name.Contains("recieptHoldsAsk")) { fc.tbAskHolds.Value = int.Parse(s1.value); }
                    else if (s1.name.Contains("recieptTransitAsk")) { fc.tbAskTransit.Value = int.Parse(s1.value); }
                    else if (s1.name.Contains("recieptSerialAsk")) { fc.tbAskSerial.Value = int.Parse(s1.value); }
                    else if (s1.name.Contains("adminDebugMode")) { fc.tbAdminMode.Value = int.Parse(s1.value); }
                    else if (s1.name.Contains("POS"))
                    {
                        if (s1.name.Contains("POSReportingEnable")){fc.cbPOSEnable.Checked = bool.Parse(s1.value);}
                        if (s1.name.Contains("POSServerAPI")) { fc.tbPOSServerAPI.Text = s1.value; }
                        if (s1.name.Contains("POSEmailEnable")) { fc.cbPOSEmailEnable.Checked = bool.Parse(s1.value); }
                        if (s1.name.Contains("POSServerEmailAPI")) { fc.tbPOSServerEmailAPI.Text = s1.value; }
                        if (s1.name.Contains("POSServerEmailRefund")) { fc.tbPOSServerEmailRefund.Text = s1.value; }

                    }
                    else if (s1.name.Contains("SIP"))
                    {
                        if (s1.name.Contains("SIPServerIP")) { fc.tbSIPServerName.Text = s1.value; }
                        if (s1.name.Contains("SIPServerPort")) { fc.tbSIPServerPassword.Text = s1.value; }
                        if (s1.name.Contains("SIPUserName")) { fc.tbSIPUserName.Text = s1.value; }
                        if (s1.name.Contains("SIPUserPassword")) { fc.tbSIPUserPassword.Text = s1.value; }
                    }
                    else
                    {
                        if (s1.name.Length>6)
                        {
                            settingsClass sc1 = new settingsClass();
                            sc1.name = s1.name.Split('=')[0];
                            sc1.value = s1.value;
                            fc.sList.Add(sc1);
                        }
                        
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }
        static public void createLog(Form1 fc)
        {
            string logFile = "";
            if (fc.tbLogLoc.Text.Contains("<temp>"))
            {
                logFile = fc.tbLogLoc.Text.Replace("<temp>", System.IO.Path.GetTempPath());
                //System.IO.Path.GetTempPath()
            }
            else { logFile = fc.tbLogLoc.Text; }
            if (!File.Exists(logFile))
            {
                try
                {
                    fc.log = File.CreateText(logFile);
                }
                catch (Exception) { fc.log = File.CreateText(System.IO.Path.GetTempPath() + "\\printProgramAdmin.log"); }
            }
            else
            {
                fc.log = File.AppendText(logFile);
            }
        }
        static internal void writeTemplateFile(Form1 fc)
        {
            string fileloc = "";
            if (fc.tb_file_temp.Text.Contains("<exe>")) { fileloc = fc.tb_file_temp.Text.Replace("<exe>", fc.pathEXE) + "\\"; }
            else { fileloc = fc.tb_file_temp.Text + "\\"; }
            //File.Delete(fileloc);
            using (XmlWriter writer = XmlWriter.Create(fileloc + fc.templateName + ".template"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Main");
                writer.WriteAttributeString("Name", fc.templateName);
                writer.WriteAttributeString("Type", fc.tempateType);
                writer.WriteStartElement("Elements");
                foreach (elementClass el in fc.el1)
                {
                    string fontString = "";
                    if (el.fontName != null)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                        // Saving Font object as a string
                        fontString = converter.ConvertToString(el.fontName);
                    }

                    writer.WriteStartElement("Element");
                    writer.WriteElementString("id", el.id.ToString());
                    writer.WriteElementString("name", el.name);
                    writer.WriteElementString("data", el.data);
                    writer.WriteElementString("dataType", el.dataType);
                    writer.WriteElementString("align", el.align);
                    writer.WriteElementString("font", fontString);
                    writer.WriteElementString("spaceTop", el.spaceTop.ToString());
                    writer.WriteElementString("width", el.width.ToString());
                    writer.WriteElementString("height", el.height.ToString());

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

        }
        static internal void writeSettingsFile(Form1 fc)
        {
            try
            {
                File.Delete(fc.pathEXE + "\\program.settings");
            }
            catch (Exception e)
            {
                MessageBox.Show("User Does Not have rights to the folder" + Environment.NewLine + e.Message);
            }
            INIFile setting = new INIFile(fc.pathEXE + "\\program.settings");
            setting.Write("General", "adminDebugMode", fc.tbAdminMode.Value.ToString());
            setting.Write("General", "recieptCheckoutAsk", fc.tbAskCheckout.Value.ToString());
            setting.Write("General", "recieptUserAsk", fc.tbAskUser.Value.ToString());
            setting.Write("General", "recieptPaymentAsk", fc.tbAskPayment.Value.ToString());
            setting.Write("General", "recieptHoldsAsk", fc.tbAskHolds.Value.ToString());
            setting.Write("General", "recieptTransitAsk", fc.tbAskTransit.Value.ToString());
            setting.Write("General", "recieptSerialAsk", fc.tbAskSerial.Value.ToString());
            setting.Write("General", "twoPageHolds", fc.tbTwoPageHolds.Value.ToString());
            setting.Write("Stats", "statsON", fc.tbStatsOnOff.Value.ToString());
            setting.Write("Stats", "statsServer", fc.tbStatsServer.Text);
            setting.Write("Printing", "printerCheckout", fc.cbCircPrinter.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Printing", "printerInTransit", fc.cbInTransitPrinter.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Printing", "printerHolds", fc.cbHoldsPrinter.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Printing", "printerFine", fc.cbFinePrinter.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Printing", "printerSerialRoute", fc.cbSerialRoutePrinter.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Printing", "printerUserRecord", fc.cbUserRecordPrinter.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Files", "fileNameInTransitFrom", fc.tbInTransitFromLoc.Text);
            setting.Write("Files", "fileNameInTransitTo", fc.tbInTransitToLoc.Text);
            setting.Write("Files", "fileNameData", fc.tbTempDataLoc.Text);
            setting.Write("Files", "fileNameLog", fc.tbLogLoc.Text);
            setting.Write("Files", "fileNameTemplate", fc.tb_file_temp.Text);
            setting.Write("Templates", "templateCirc", fc.cb_temp_circ.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Templates", "templateInTransit", fc.cb_temp_transit.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Templates", "templateHoldsPage1", fc.cb_temp_holds_p1.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Templates", "templateHoldsPage2", fc.cb_temp_holds_p2.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Templates", "templateFine", fc.cb_temp_fine.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Templates", "templateSerialRoute", fc.cb_temp_serialRoute.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Templates", "templateUserRecord", fc.cb_temp_UserRecord.Text.Replace(" (Not Found)", string.Empty));
            setting.Write("Notifications", "notificationHoldsCallSwitch", fc.tbarNotificationHoldsCall.Value.ToString());
            setting.Write("Notifications", "notificationHoldsAPICallSwitch", fc.tbarNotificationHoldsCallAPI.Value.ToString());
            setting.Write("Notifications", "notificationHoldsCallServer", fc.tboxNotificationHoldsCallServer.Text);
            setting.Write("Notifications", "notificationHoldsCallVoipServer", fc.tboxVoipServer.Text);
            setting.Write("Notifications", "notificationHoldsCallVoipPort", fc.tBoxVoipServerPort.Text);
            setting.Write("Notifications", "notificationHoldsCallVoipUserName", fc.tBoxVoipUserName.Text);
            setting.Write("Notifications", "notificationHoldsCallVoipPassword", fc.tBoxVoipPassword.Text);
            setting.Write("POS", "POSReportingEnable", fc.cbPOSEnable.Checked.ToString());
            setting.Write("POS", "POSServerAPI", fc.tbPOSServerAPI.Text);
            setting.Write("POS", "POSEmailEnable", fc.cbPOSEmailEnable.Checked.ToString());
            setting.Write("POS", "POSServerEmailAPI", fc.tbPOSServerEmailAPI.Text);
            setting.Write("POS", "POSServerEmailRefund", fc.tbPOSServerEmailRefund.Text);
            setting.Write("SIPSettings", "SIPServerIP", fc.tbSIPServerName.Text);
            setting.Write("SIPSettings", "SIPServerPort", fc.tbSIPServerPassword.Text);
            setting.Write("SIPSettings", "SIPUserName", fc.tbSIPUserName.Text);
            setting.Write("SIPSettings", "SIPUserPassword", fc.tbSIPUserPassword.Text);
            if (fc.sList.Count > 0)
            {
                foreach (settingsClass cs1 in fc.sList)
                {
                    setting.Write("Custom", cs1.name, cs1.value);
                }
            }
        }
        static string fixVars(string input)
        {
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
            if (none == true) { data = input; }
            return data;
        }
        static internal string readTransitTo(Form1 fc, string code, string type)
        {
            string str = "none";
            try
            {
                string[] lineArray = File.ReadAllLines(fixVars(fc.tbInTransitToLoc.Text));
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
        static internal void populateTempates(Form1 fc)
        {

        }
        public static string HTMLtoTXTUser(string str)
        {
            string result = "";
            string str1 = StripHTML(str).Replace("\r","");
            foreach (string str11 in str1.Split(Environment.NewLine.ToCharArray()))
            {
                if (str11.TrimEnd().Length>4)
                {
                    result = result + str11 + Environment.NewLine;
                }
            }
            return result;
        }
        public static string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
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
                if (line.Contains("Basic Info")) { typeStr = "Basic"; }
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
            if (type == "basic") { return basicinfo; }
            else if (type == "demoinfo") { return demoInfo; }
            else if (type == "circinfo") { return circinfo; }
            else if (type == "address") { return address; }
            else if (type == "extinfo") { return extinfo; }
            else if (type == "checkouts") { return checkouts; }
            else if (type == "bills") { return bills; }
            else if (type == "holds") { return holds; }
            else { return "none"; }
        }

    }
        class INIFile
        {
            private string filePath;

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section,
            string key,
            string val,
            string filePath);

            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section,
            string key,
            string def,
            StringBuilder retVal,
            int size,
            string filePath);

            public INIFile(string filePath)
            {
                this.filePath = filePath;
            }

            public void Write(string section, string key, string value)
            {
                WritePrivateProfileString(section, key, value, this.filePath);
            }

            public string Read(string section, string key)
            {
                StringBuilder SB = new StringBuilder(255);
                int i = GetPrivateProfileString(section, key, "", SB, 255, this.filePath);
                return SB.ToString();
            }

            public string FilePath
            {
                get { return this.filePath; }
                set { this.filePath = value; }
            }
        }
}
