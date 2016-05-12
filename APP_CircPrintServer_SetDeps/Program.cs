using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.IO;

namespace APP_CircPrintServer_SetDeps
{
    class Program
    {
        static private int GetOSArchitecture()
        {
            string pa =
                Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return ((String.IsNullOrEmpty(pa) ||
                     String.Compare(pa, 0, "x86", 0, 3, true) == 0) ? 32 : 64);
        }
        static void Main(string[] args)
        {
            string portName = "";
            string pathEXE = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            RegistryKey PrinterPort = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Control\\Print\\Monitors\\Redirected Port", true);
            string tempFolder = System.Environment.GetEnvironmentVariable("TEMP");
            System.IO.StreamWriter file = new System.IO.StreamWriter(tempFolder + "\\installmsi.log");
            
            file.WriteLine(pathEXE);
            if (PrinterPort == null)
            {
                int test = GetOSArchitecture();
                if (test == 32)
                {
                    Process.Start(pathEXE + "\\redmon\\setup.exe", "/S");
                }
                else
                {
                    Process.Start(pathEXE + "\\redmon\\setup64.exe", "/S");
                }
                
            }
            RegistryKey PrinterPort1 = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Control\\Print\\Monitors\\Redirected Port\\Ports", true);
            if (PrinterPort1 == null)
            {
                Registry.LocalMachine.CreateSubKey("SYSTEM\\ControlSet001\\Control\\Print\\Monitors\\Redirected Port\\Ports");
            }
            bool found = false;
            string root = "SYSTEM\\ControlSet001\\Control\\Print\\Monitors\\Redirected Port\\Ports";
            PrinterPort = Registry.LocalMachine.OpenSubKey(root, true);
            foreach (string keyname in PrinterPort.GetSubKeyNames())
            {
                //MessageBox.Show(keyname);
                foreach (string valuename in Registry.LocalMachine.OpenSubKey(root + "\\" + keyname, true).GetValueNames())
                {
                    if (valuename == "Command")
                    {
                        RegistryKey reg1 = Registry.LocalMachine.OpenSubKey(root + "\\" + keyname, true);
                        if ((string)reg1.GetValue(valuename, " ") == pathEXE + "\\APP_CircPrintServer.exe")
                        {
                            found = true;
                            portName = keyname;
                        }
                    }

                }
            }
            if (!found)
            {
                string strRoot = "SYSTEM\\ControlSet001\\Control\\Print\\Monitors\\Redirected Port\\Ports";
                RegistryKey reg1 = Registry.LocalMachine.OpenSubKey(strRoot, true);
                reg1.CreateSubKey("SirsiPrintPort");
                portName="SirsiPrintPort";
                reg1 = Registry.LocalMachine.OpenSubKey(root + "\\SirsiPrintPort", true);
                reg1.SetValue("Description", "Redirected Port", RegistryValueKind.String);
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

            RegistryKey PathKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment", true);
            string path = (string)PathKey.GetValue("path","");
            if (!path.Contains(pathEXE))
            {
                path = path + ";" + pathEXE;
                PathKey.SetValue("path", path);
            }
            found = false;
            foreach (string printername in PrinterSettings.InstalledPrinters)
            {
                if (printername == "SirsiPrinter")
                {
                    found = true;
                }
            }
            string system32dir = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string windowsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            if (found)
            {
                Process.Start(system32dir + "\\rundll32.exe", system32dir + "\\printui.dll,PrintUIEntry /dl /n SirsiPrinter");

            }
            
                
                Process.Start(system32dir + "\\rundll32.exe", system32dir + "\\printui.dll,PrintUIEntry /if /b SirsiPrinter /f  " + windowsFolder + "\\inf\\ntprint.inf /r " + portName + " /m \"Generic / Text Only\"");
            

        }
    }
}
