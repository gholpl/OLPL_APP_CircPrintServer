using Microsoft.Win32;
using OLPL_APP_CircPrinter_Admin.Functions;
using OLPL_APP_CircPrinter_Admin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace OLPL_APP_CircPrinter_Admin
{
    public partial class Form1 : Form
    {
        internal string portName;
        public StreamWriter log;
        internal List<elementClass> el1;
        internal List<templatesClass> tl1;
        internal bool notsaved;
        public string templateName = "";
        public string tempateType = "";
        internal List<settingsClass> sList;
        internal string pathEXE = "";
        internal string customValueFunction;
        internal string tempCirc = "";
        internal string tempInTransit = "";
        internal string tempFine = "";
        internal string tempHoldsP1 = "";
        internal string tempHoldsP2 = "";
        internal string tempSerialRoute = "";
        internal string tempUserRecord = "";
        public Form1()
        {
            InitializeComponent();
            el1 = new List<elementClass>();
            tl1 = new List<templatesClass>();
            sList = new List<settingsClass>();

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            templeteName tp1 = new templeteName();
            tp1.ShowDialog();
        }

        private void btnAddElement_Click(object sender, EventArgs e)
        {
            elementAdd eadd = new elementAdd();
            eadd.fc = this;
            eadd.elementNew = true;
            eadd.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            stat_PortRedirInstall.Enabled = true;
            customValueFunction = "";
            //notsaved = false;
            pathEXE = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            populateLists.populatePrinters(this);
            fileControl.proccessSettingFile(this);
            populateLists.populateCustomSettings(this);
            tl1 = fileControl.getTemplates(this);
            fileControl.createLog(this);
            populateLists.populateTemplateListView(this);
            validationControl.testSettingsWrite(this);
            validationControl.testPrinter(this);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listElements.SelectedItems.Count > 0)
            {
                bool done = false;
                foreach (elementClass l1 in el1)
                {
                    if (done==false)
                    {
                        if (l1.id == int.Parse(listElements.SelectedItems[0].Text))
                        {
                            elementAdd eadd = new elementAdd();
                            eadd.fc = this;
                            eadd.elementNew = false;
                            eadd.l1 = l1;
                            done = true;
                            eadd.ShowDialog();
                        }

                    }

                }
            }
            
        } 
        private void listSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            notsaved = true;
            foreach (ListViewItem ls1 in listSettings.SelectedItems)
            {
                tbCustomSettingName.Text = ls1.Text;
                tbCustomSettingValue.Text = ls1.SubItems[1].Text;
            }
        }

        private void tbTempDataLoc_TextChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void tbAdminMode_Scroll(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void tbAskCheckout_Scroll(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void tbStatsOnOff_Scroll(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void tbStatsServer_TextChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void cbCircPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void cbInTransitPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void cbHoldsPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void cbFinePrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void tbLogLoc_TextChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void tbInTransitFromLoc_TextChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void tbInTransitToLoc_TextChanged(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void listTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listTemplates.SelectedItems.Count > 0)
            {
                if (listTemplates.SelectedItems[0].Text == "Add New Template")
                {
                    btnLoad.Text = "Add Template";

                }
                else
                {
                    btnLoad.Text = "Load Template";
                }
            }
           
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (listTemplates.SelectedItems.Count > 0)
            {
                if (listTemplates.SelectedItems[0].Text == "Add New Template")
                {
                    el1 = new List<elementClass>();
                    populateLists.populateList(this);
                    PrintControl.CreatePrintPreviewControl(this);
                    templeteName tp1 = new templeteName();
                    tp1.ShowDialog();
                }
                else
                {
                    foreach(templatesClass tl in tl1)
                    {
                        if (tl.name == listTemplates.SelectedItems[0].Text)
                        {
                            label2.Text = tl.type + " templete named " + tl.name;
                            tempateType = tl.type;
                            templateName = tl.name;
                            el1 = new List<elementClass>();
                            foreach (elementClass el in tl.element)
                            {
                                
                                el1.Add(el);
                            }
                            populateLists.populateList(this);
                            PrintControl.CreatePrintPreviewControl(this);
                            btnAddElement.Visible = true;
                        }
                    }
                   // templatesClass tl = tl1[tl1.IndexOf()];
                }
            }
        }

        private void listElements_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listElements.SelectedItems.Count > 0)
            {
                btnModifyElement.Visible = true;
            }
            else { btnModifyElement.Visible = false; }
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            if (templateName.Length > 3)
            {
                fileControl.writeTemplateFile(this);
                tl1 = fileControl.getTemplates(this);
                populateLists.populateTemplateListView(this);
                el1 = new List<elementClass>();
                populateLists.populateList(this);
                label2.Text = "";
                tempateType = "";
                templateName = "";
                el1 = new List<elementClass>();
            }
            else
            {
                MessageBox.Show("Cannot save template -- no template name provided");
            }

        }
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            fileControl.writeSettingsFile(this);
        }

        private void btnAddCustomValue_Click(object sender, EventArgs e)
        {
            customValueFunction = "Add";
            btnAddCustomValue.Enabled = false;
            btnChangeValue.Enabled = false;
            btnDeleteValue.Enabled = false;
            btnSettingCancel.Enabled = true;
            btnSaveValue.Enabled = true;
            tbCustomSettingName.Text = "";
            tbCustomSettingName.Enabled = true;
            tbCustomSettingValue.Text = "";
            tbCustomSettingValue.Enabled = true;
        }

        private void btnSettingCancel_Click(object sender, EventArgs e)
        {
            btnAddCustomValue.Enabled = true;
            btnChangeValue.Enabled = true;
            btnDeleteValue.Enabled = true;
            btnSettingCancel.Enabled = false;
            btnSaveValue.Enabled = false;
            tbCustomSettingName.Text = "";
            tbCustomSettingName.Enabled = false;
            tbCustomSettingValue.Text = "";
            tbCustomSettingValue.Enabled = false;
            customValueFunction = "";
        }

        private void btnDeleteValue_Click(object sender, EventArgs e)
        {
            if (listSettings.SelectedItems.Count > 0)
            {
                settingsClass sc2 = new settingsClass();
                foreach(settingsClass sc1 in sList)
                {
                    if (sc1.name ==listSettings.SelectedItems[0].Text)
                    {
                        sc2 = sc1;
                    }
                }
                sList.Remove(sc2);
                populateLists.populateCustomSettings(this);
            }
            else { MessageBox.Show("Please select item to delete"); }
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {
            if (listSettings.SelectedItems.Count > 0)
            {
                customValueFunction = "Change";
                btnAddCustomValue.Enabled = false;
                btnChangeValue.Enabled = false;
                btnDeleteValue.Enabled = false;
                btnSettingCancel.Enabled = true;
                btnSaveValue.Enabled = true;
                tbCustomSettingName.Enabled = true;
                tbCustomSettingValue.Enabled = true;
            }
            else { MessageBox.Show("Please select value to change"); }
            
        }

        private void btnSaveValue_Click(object sender, EventArgs e)
        {
                settingsClass sc1 = new settingsClass();
                sc1.name = tbCustomSettingName.Text;
                sc1.value = tbCustomSettingValue.Text;
                populateLists.addCustomSetting(this, sc1);
                btnAddCustomValue.Enabled = true;
                btnChangeValue.Enabled = true;
                btnDeleteValue.Enabled = true;
                btnSettingCancel.Enabled = false;
                btnSaveValue.Enabled = false;
                tbCustomSettingName.Text = "";
                tbCustomSettingName.Enabled = false;
                tbCustomSettingValue.Text = "";
                tbCustomSettingValue.Enabled = false;
                customValueFunction = "";
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (notsaved)
            {
                DialogResult dialogResult = MessageBox.Show("Settings not saved!  Do you want to save?", "Save Settings", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    fileControl.writeSettingsFile(this);
                }
            }
        }

        private void btnDeleteElement_Click(object sender, EventArgs e)
        {
            populateLists.deleteElement(this);
            populateLists.populateList(this);
            PrintControl.CreatePrintPreviewControl(this);
        }

        private void btnMoveUpElelement_Click(object sender, EventArgs e)
        {
            populateLists.moveElement(this, "up");
            populateLists.populateList(this);
            PrintControl.CreatePrintPreviewControl(this);
        }

        private void btnMoveDOwnElement_Click(object sender, EventArgs e)
        {
            populateLists.moveElement(this, "down");
            populateLists.populateList(this);
            PrintControl.CreatePrintPreviewControl(this);
        }

        private void cb_temp_holds_p1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_temp_circ_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbTwoPageHolds_Scroll(object sender, EventArgs e)
        {
            notsaved = true;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void stat_PortRedirInstall_Tick(object sender, EventArgs e)
        {
            RegistryKey PrinterPort = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Control\\Print\\Monitors\\Redirected Port", true);
            lbl_portRedirInsall.Text = "Checking";
            lbl_portRedirInsall.ForeColor = Color.Black;
            if (PrinterPort == null)
            {
                lbl_portRedirInsall.Text = "Failed";
                lbl_portRedirInsall.ForeColor = Color.Red;
                btnInstallRedmon.Visible = true;
            }
            else
            {
                lbl_portRedirInsall.Text = "OK";
                lbl_portRedirInsall.ForeColor = Color.Green;
                btnInstallRedmon.Visible = false;
                stat_PortRedirInstall.Enabled = false;
                stats_PortRedirCreate.Enabled = true;
            }
        }

        private void btnInstallRedmon_Click(object sender, EventArgs e)
        {
            Process.Start(pathEXE + "\\redmon\\setup64.exe", "/S");
        }

        private void stats_PortRedirCreate_Tick(object sender, EventArgs e)
        {
            bool found = false;
            string root = "SYSTEM\\ControlSet001\\Control\\Print\\Monitors\\Redirected Port\\Ports";
            RegistryKey PrinterPort = Registry.LocalMachine.OpenSubKey(root, true);
            lbl_redirPortCreated.Text = "Checking";
            lbl_redirPortCreated.ForeColor = Color.Black;
            foreach(string keyname in PrinterPort.GetSubKeyNames())
            {
                //MessageBox.Show(keyname);
                foreach (string valuename in Registry.LocalMachine.OpenSubKey(root+"\\"+ keyname, true).GetValueNames())
                {
                    if (valuename == "Command")
                    {
                        RegistryKey reg1 = Registry.LocalMachine.OpenSubKey(root + "\\" + keyname, true);
                        if((string)reg1.GetValue(valuename," ") == pathEXE + "\\APP_CircPrintServer.exe")
                        {
                            found = true;
                            portName = valuename;
                        }
                    }
                    
                }
            }
            if (found)
            {

                lbl_redirPortCreated.Text = "OK";
                lbl_redirPortCreated.ForeColor = Color.Green;
                stats_PortRedirCreate.Enabled = false;
                btn_CreatePort.Visible = false;
                stats_PrinterCreated.Enabled = true;
            }
            else
            {
                lbl_redirPortCreated.Text = "Failed";
                lbl_redirPortCreated.ForeColor = Color.Red;
                btn_CreatePort.Visible = true;
            }
        }

        private void btn_CreatePort_Click(object sender, EventArgs e)
        {
            string root = "SYSTEM\\ControlSet001\\Control\\Print\\Monitors\\Redirected Port\\Ports";
            RegistryKey reg1 = Registry.LocalMachine.OpenSubKey(root, true);
            reg1.CreateSubKey("SirsiPrintPort");
            reg1 = Registry.LocalMachine.OpenSubKey(root + "\\SirsiPrintPort", true);
            reg1.SetValue("Description", "Redirected Port",RegistryValueKind.String);
            reg1.SetValue("Command", pathEXE + "\\APP_CircPrintServer.exe", RegistryValueKind.String);
            reg1.SetValue("Arguments", "", RegistryValueKind.String);
            reg1.SetValue("Printer", "SirsiPrinter", RegistryValueKind.String);
            reg1.SetValue("Output", "00000000", RegistryValueKind.DWord);
            reg1.SetValue("ShowWindow", "00000002", RegistryValueKind.DWord);
            reg1.SetValue("RunUser", "00000001", RegistryValueKind.DWord);
            reg1.SetValue("Delay", "00000300", RegistryValueKind.DWord);
            reg1.SetValue("LogFileUse", "00000000", RegistryValueKind.DWord);
            reg1.SetValue("LogFileName", "", RegistryValueKind.String);
            reg1.SetValue("LogFileDebug", "00000000", RegistryValueKind.DWord);
            reg1.SetValue("PrintError", "00000000", RegistryValueKind.DWord);
        }

        private void stats_PrinterCreated_Tick(object sender, EventArgs e)
        {
            bool found = false;
            lbl_PrinterCreated.Text = "Checking";
            lbl_PrinterCreated.ForeColor = Color.Black;
            foreach(string printername in PrinterSettings.InstalledPrinters)
            {
                if (printername == "SirsiPrinter")
                {
                    found = true;
                }
            }
            if (found)
            {
                lbl_PrinterCreated.Text = "OK";
                lbl_PrinterCreated.ForeColor = Color.Green;
                btn_CreatePrinter.Visible = false;
                stats_PrinterCreated.Enabled = false;
                stat_SirsiSettings.Enabled = true;
            }
            else
            {
                lbl_PrinterCreated.Text = "Failed";
                lbl_PrinterCreated.ForeColor = Color.Red;
                btn_CreatePrinter.Visible = true;
            }
        }

        private void btn_CreatePrinter_Click(object sender, EventArgs e)
        {
            string system32dir = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string windowsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            Process.Start(system32dir + "\\rundll32.exe", system32dir + "\\printui.dll,PrintUIEntry /if /b SirsiPrinter /f  " + windowsFolder + "\\inf\\ntprint.inf /r SirsiPrintPort /m \"Generic / Text Only\" /z");
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
    }
}
