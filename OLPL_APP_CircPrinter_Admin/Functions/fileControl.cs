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
using static DLL_CircPrintServer.Classes.controlFunctions;

namespace OLPL_APP_CircPrinter_Admin.Functions
{
    public class fileControl
    { 
        static public void proccessSettingFile(Form1 fc, modelSettings mS)
        {
            int value = 0;
            fc.tb_file_temp.Text = mS.tempLocation;
                            fc.tempCirc =mS.tempCirc;
                            fc.tempInTransit = mS.tempInTransit;
                            fc.tempHoldsP1 = mS.tempHoldsP1;
                            fc.tempHoldsP2 = mS.tempHoldsP2;
                            fc.tempFine = mS.tempFine;
                            fc.tempSerialRoute = mS.tempSerialRoute;
                            fc.tempUserRecord = mS.tempUserRecord;
                                if (fc.cbCircPrinter.Items.Contains(mS.printerCirc))
                                {
                                    fc.cbCircPrinter.SelectedItem = mS.printerCirc;
                                }
                                else
                                {
                                    fc.cbCircPrinter.Items.Add(mS.printerCirc + " (Not Found)");
                                    fc.cbCircPrinter.SelectedText = mS.printerCirc + " (Not Found)";
                                }
                                if (fc.cbInTransitPrinter.Items.Contains(mS.printerInTransit))
                                {
                                    fc.cbInTransitPrinter.SelectedItem = mS.printerInTransit;
                                }
                                else
                                {
                                    fc.cbInTransitPrinter.Items.Add(mS.printerInTransit + " (Not Found)");
                                    fc.cbInTransitPrinter.SelectedText = mS.printerInTransit + " (Not Found)";
                                }
                                if (fc.cbHoldsPrinter.Items.Contains(mS.printerHolds))
                                {
                                    fc.cbHoldsPrinter.SelectedItem = mS.printerHolds;
                                }
                                else
                                {
                                    fc.cbHoldsPrinter.Items.Add(mS.printerHolds + " (Not Found)");
                                    fc.cbHoldsPrinter.SelectedText = mS.printerHolds + " (Not Found)";
                                }
                                if (fc.cbFinePrinter.Items.Contains(mS.printerFine))
                                {
                                    fc.cbFinePrinter.SelectedItem = mS.printerFine;
                                }
                                else
                                {
                                    fc.cbFinePrinter.Items.Add(mS.printerFine + " (Not Found)");
                                    fc.cbFinePrinter.SelectedText = mS.printerFine + " (Not Found)";
                                }
                                if (fc.cbSerialRoutePrinter.Items.Contains(mS.printerSerialRoute))
                                {
                                    fc.cbSerialRoutePrinter.SelectedItem = mS.printerSerialRoute;
                                }
                                else
                                {
                                    fc.cbSerialRoutePrinter.Items.Add(mS.printerSerialRoute + " (Not Found)");
                                    fc.cbSerialRoutePrinter.SelectedText = mS.printerSerialRoute + " (Not Found)";
                                }
                                if (fc.cbUserRecordPrinter.Items.Contains(mS.printerUserRecord))
                                {
                                    fc.cbUserRecordPrinter.SelectedItem = mS.printerUserRecord;
                                }
                                else
                                {
                                    fc.cbUserRecordPrinter.Items.Add(mS.printerUserRecord + " (Not Found)");
                                    fc.cbUserRecordPrinter.SelectedText = mS.printerUserRecord + " (Not Found)";
                                }
                            fc.tbInTransitFromLoc.Text = mS.fileInTransitFrom;
                fc.tbInTransitToLoc.Text = mS.fileInTransitTo;
                fc.tbTempDataLoc.Text = mS.fileTempData;
                fc.tbLogLoc.Text = mS.fileLog;
            if(int.TryParse(mS.statsSwitch,out value)){ fc.tbStatsOnOff.Value = value; } else { fc.tbStatsOnOff.Value = 0; }
                fc.tbStatsServer.Text = mS.statsServer;
            if (int.TryParse(mS.notificationHoldsCallSwitch, out value)) { fc.tbarNotificationHoldsCall.Value = value; } else { fc.tbarNotificationHoldsCall.Value = 0; }
            if (int.TryParse(mS.notificationHoldsAPICallSwitch, out value)) { fc.tbarNotificationHoldsCallAPI.Value = value; } else { fc.tbarNotificationHoldsCallAPI.Value = 0; }
                fc.tboxNotificationHoldsCallServer.Text = mS.notificationHoldsCallServer;
                fc.tboxVoipServer.Text = mS.notificationHoldsCallVoipServer;
                fc.tBoxVoipServerPort.Text = mS.notificationHoldsCallVoipPort;
                fc.tBoxVoipUserName.Text = mS.notificationHoldsCallVoipUserName;
                fc.tBoxVoipPassword.Text = mS.notificationHoldsCallVoipPassword;
            if (int.TryParse(mS.switchTwoPageHolds, out value)) { fc.tbTwoPageHolds.Value = value; } else { fc.tbTwoPageHolds.Value = 0; }
            if (int.TryParse(mS.switchAskCheckOut, out value)) { fc.tbAskCheckout.Value = value; } else { fc.tbAskCheckout.Value = 0; }
            if (int.TryParse(mS.switchAskUser, out value)) { fc.tbAskUser.Value = value; } else { fc.tbAskUser.Value = 0; }
            if (int.TryParse(mS.switchAskPayment, out value)) { fc.tbAskPayment.Value = value; } else { fc.tbAskPayment.Value = 0; }
            if (int.TryParse(mS.switchAskHolds, out value)) { fc.tbAskHolds.Value = value; } else { fc.tbAskHolds.Value = 0; }
            if (int.TryParse(mS.switchAskTransit, out value)) { fc.tbAskTransit.Value = value; } else { fc.tbAskTransit.Value = 0; }
            if (int.TryParse(mS.switchAskSerial, out value)) { fc.tbAskSerial.Value = value; } else { fc.tbAskSerial.Value = 0; }
            if (int.TryParse(mS.switchAdminMode, out value)) { fc.tbAdminMode.Value = value; } else { fc.tbAdminMode.Value = 0; }
                fc.cbPOSEnable.Checked = mS.POSEnable;
                fc.tbPOSServerAPI.Text = mS.POSServerAPI;
                fc.cbPOSEmailEnable.Checked = mS.POSEmailEnable;
                fc.tbPOSServerEmailAPI.Text = mS.POSServerEmailAPI;
                fc.tbPOSServerEmailRefund.Text = mS.POSServerEmailRefund;
                fc.tbPOSDataFolder.Text = mS.POSDataFolder;
                fc.cbPOSDebugLogging.Checked = mS.POSDebugLogging;
                fc.tbSIPServerName.Text = mS.SIPServerIP;
                fc.tbSIPServerPassword.Text = mS.SIPServerPort;
                fc.tbSIPUserName.Text = mS.SIPUserName;
                fc.tbSIPUserPassword.Text = mS.SIPUserPassword;
                fc.cbErrorEMailEnable.Checked = mS.ErrorEMailEnable;
                fc.tbErrorEmailServer.Text = mS.ErrorEMailServer;
                fc.tbErrorEmalAddress.Text = mS.ErrorEMailAddress;
                foreach(modelSettingsCustom mSc in mS.customSettings)
                {
                    settingsClass sc1 = new settingsClass();
                    sc1.name = mSc.name;
                    sc1.value = mSc.value;
                    fc.sList.Add(sc1);
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
            //if (fc.tb_file_temp.Text.Contains("<exe>")) { fileloc = fc.tb_file_temp.Text.Replace("<exe>", fc.pathEXE) + "\\"; }
            //else { fileloc = fc.tb_file_temp.Text + "\\"; }
            fileloc = controlFunctions.fixVars(fc.tb_file_temp.Text);
            //File.Delete(fileloc);
            using (XmlWriter writer = XmlWriter.Create(fileloc + fc.templateName + ".template"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Main");
                writer.WriteAttributeString("Name", fc.templateName);
                writer.WriteAttributeString("Type", fc.tempateType);
                writer.WriteStartElement("Elements");
                foreach (modelElement el in fc.el1)
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
                File.Delete(controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware") + "\\program.settings");
            }
            catch (Exception e)
            {
                MessageBox.Show("User Does Not have rights to the folder" + Environment.NewLine + e.Message);
            }
            INIFile setting = new INIFile(controlFunctions.fixVars("<ProgramData>\\CircPrintSoftware") + "\\program.settings");
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
            setting.Write("POS", "POSDataFolder", fc.tbPOSDataFolder.Text);
            setting.Write("POS", "POSDebugLogging", fc.cbPOSDebugLogging.Checked.ToString());
            setting.Write("SIPSettings", "SIPServerIP", fc.tbSIPServerName.Text);
            setting.Write("SIPSettings", "SIPServerPort", fc.tbSIPServerPassword.Text);
            setting.Write("SIPSettings", "SIPUserName", fc.tbSIPUserName.Text);
            setting.Write("SIPSettings", "SIPUserPassword", fc.tbSIPUserPassword.Text);
            setting.Write("ErrorReporting", "ErrorEMailEnable", fc.cbErrorEMailEnable.Checked.ToString());
            setting.Write("ErrorReporting", "ErrorEMailServer", fc.tbErrorEmailServer.Text);
            setting.Write("ErrorReporting", "ErrorEMailAddress", fc.tbErrorEmalAddress.Text);
            if (fc.sList.Count > 0)
            {
                foreach (settingsClass cs1 in fc.sList)
                {
                    setting.Write("Custom", cs1.name, cs1.value);
                }
            }
        }

    }
        
}
