using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DLL_CircPrintServer.Models;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml;
using System.Drawing;
using static DLL_CircPrintServer.Classes.controlFunctions;
using System.Security.AccessControl;

namespace DLL_CircPrintServer.Classes
{
    
    public class controlSettings
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        static public List<modelSettingsCustom> readSettingFile(string pathEXE)
        {
            List<modelSettingsCustom> set1 = new List<modelSettingsCustom>(); 
            try
            {
                
                string inputSettings = File.ReadAllText(pathEXE + "\\program.settings");
                string[] inputSettingsArray = inputSettings.Split(Environment.NewLine.ToCharArray());
                foreach (string str in inputSettingsArray)
                {
                    modelSettingsCustom s1 = new modelSettingsCustom();
                    if (str.Contains("="))
                    {
                        s1.name= str.Split('=')[0];
                        s1.value= str.Split('=')[1];
                        set1.Add(s1);
                    }
                }
                return set1;
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error during setting file read " + e1.ToString());
                modelSettingsCustom s1 = new modelSettingsCustom();
                s1.value = "error";
                s1.value = e1.Message;
                set1.Add(s1);
                return set1;
            }
        }
        public static modelSettings proccessSettingFile()
        {
            modelSettings mS = new modelSettings();
            mS.customSettings = new List<modelSettingsCustom>();
            mS.machineName = Environment.MachineName;
            try
            {
                if (!Directory.Exists(controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware"))) { Directory.CreateDirectory(controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware")); }
                if (!File.Exists(controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware\\program.Settings")))
                {
                    File.Copy(controlFunctions.fixVars("<exe>\\defaultProgram.settings"), controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware\\program.Settings"));
                    AddFileSecurity(controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware\\program.Settings"), "Users", FileSystemRights.FullControl, AccessControlType.Allow);
                }
                List<modelSettingsCustom> set1 = controlSettings.readSettingFile(controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware"));
                foreach (modelSettingsCustom s1 in set1)
                {

                    if (s1.name.Contains("fileNameTemplate"))
                    {
                        mS.tempLocation = (s1.value);
                    }
                    else if (s1.name.Contains("template"))
                    {
                        if (s1.name.Contains("Circ"))
                        {
                            mS.tempCirc = (s1.value);
                        }
                        if (s1.name.Contains("InTransit"))
                        {
                            mS.tempInTransit = (s1.value);
                        }
                        if (s1.name.Contains("HoldsPage1"))
                        {
                            mS.tempHoldsP1 = (s1.value);
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
                            mS.fileInTransitFrom = (s1.value);
                        }
                        if (s1.name.Contains("InTransitTo"))
                        {
                            mS.fileInTransitTo = (s1.value);
                        }
                        if (s1.name.Contains("Data"))
                        {
                            mS.fileTempData = (s1.value);
                        }
                        if (s1.name.Contains("Log"))
                        {
                            mS.fileLog = (s1.value);
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
                        if (s1.name.Contains("statsON")) { mS.statsSwitch = s1.value; }
                        if (s1.name.Contains("statsServer")) { mS.statsServer = s1.value; }
                    }
                    else if (s1.name.Contains("twoPageHolds"))
                    {
                        mS.switchTwoPageHolds = s1.value;
                    }
                    else if (s1.name.Contains("recieptCheckoutAsk")) { mS.switchAskCheckOut = s1.value; }
                    else if (s1.name.Contains("recieptUserAsk")) { mS.switchAskUser = s1.value; }
                    else if (s1.name.Contains("recieptPaymentAsk")) { mS.switchAskPayment = s1.value; }
                    else if (s1.name.Contains("recieptHoldsAsk")) { mS.switchAskHolds = s1.value; }
                    else if (s1.name.Contains("recieptTransitAsk")) { mS.switchAskTransit = s1.value; }
                    else if (s1.name.Contains("recieptSerialAsk")) { mS.switchAskSerial = s1.value; }
                    else if (s1.name.Contains("adminDebugMode")) { mS.switchAdminMode = s1.value; }
                    else if (s1.name.Contains("viewAdvanced")) { mS.viewAdvanced = s1.value; }
                    else if (s1.name.Contains("POS"))
                    {
                        if (s1.name.Contains("POSReportingEnable")) { mS.POSEnable = bool.Parse(s1.value); }
                        if (s1.name.Contains("POSServerAPI")) { mS.POSServerAPI = s1.value; }
                        if (s1.name.Contains("POSEmailEnable")) { mS.POSEmailEnable = bool.Parse(s1.value); }
                        if (s1.name.Contains("POSServerEmailAPI")) { mS.POSServerEmailAPI = s1.value; }
                        if (s1.name.Contains("POSServerEmailRefund")) { mS.POSServerEmailRefund = s1.value; }
                        if (s1.name.Contains("POSDataFolder")) { mS.POSDataFolder = s1.value; }
                        if (s1.name.Contains("POSDebugLogging")) { mS.POSDebugLogging = bool.Parse(s1.value); }
                    }
                    else if (s1.name.Contains("SIP"))
                    {
                        if (s1.name.Contains("SIPServerIP")) { mS.SIPServerIP = s1.value; }
                        if (s1.name.Contains("SIPServerPort")) { mS.SIPServerPort = s1.value; }
                        if (s1.name.Contains("SIPUserName")) { mS.SIPUserName = s1.value; }
                        if (s1.name.Contains("SIPUserPassword")) { mS.SIPUserPassword = s1.value; }

                    }
                    else if (s1.name.Contains("Error"))
                    {
                        if (s1.name.Contains("ErrorEMailEnable")) { mS.ErrorEMailEnable = bool.Parse(s1.value); }
                        if (s1.name.Contains("ErrorEMailServer")) { mS.ErrorEMailServer = s1.value; }
                        if (s1.name.Contains("ErrorEMailAddress")) { mS.ErrorEMailAddress = s1.value; }
                    }
                    else
                    {
                        modelSettingsCustom sc1 = new modelSettingsCustom();
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
        static public modelTemplate readTemplateFile(string location, modelSettings mS,string type)
        {
            try
            {
                modelTemplate tc = new modelTemplate();
                tc.element = new List<modelElement>();
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
                    tc.element.Add(ec1);
                }
                return tc;

            }
            catch (Exception)
            {
                if (!type.Contains("none"))
                {
                    return readTemplateFile(controlFunctions.fixVars("<exe>\\default" + type + ".template"), mS, "none");
                }
                else
                {
                    //log.WriteLine(e.Message);
                    modelTemplate tc = new modelTemplate();
                    tc.name = "Error";
                    return tc;
                }
            }
        }
        static public List<modelTemplate> getTemplates(modelSettings mS)
        {
            string fileloc = "";
            List<modelTemplate> tc = new List<modelTemplate>();

            fileloc = controlFunctions.fixVars(mS.tempLocation);
            string[] array1 = Directory.GetFiles(fileloc, "*.template");
            modelTemplate tc1 = new modelTemplate();
            tc1.name = "Add New Template";
            tc.Add(tc1);
            foreach (string str in array1)
            {
                tc.Add(readTemplateFile(str, mS,"All"));
            }
            return tc;
        }
        static public void writeSetting(string category, string name, string value)
        {
            try
            {
                INIFile setting = new INIFile(controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware") + "\\program.settings");
                setting.Write(category, name, value);
            }
            catch (Exception e)
            {
                MessageBox.Show("User Does Not have rights to the folder" + Environment.NewLine + e.Message);
            }
            
        }
    }
}
