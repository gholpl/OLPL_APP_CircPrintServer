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
using System.Net;
using System.Collections.Specialized;
using System.Threading;
using System.Diagnostics;

namespace APP_CircPrintServer.Functions
{
    class FileControl
    {
        internal static bool writeDataLine(modelSettings mS, string fileName, string line)
        {
            try
            {
                StreamWriter dataFile;
               string fileData = controlFunctions.fixVars(mS.POSDataFolder + fileName);
                if (!File.Exists(fileData))
                {
                    dataFile = File.CreateText(fileData);
                }
                else
                {
                    dataFile = File.AppendText(fileData);
                }
                dataFile.WriteLine(line);
                dataFile.Close();
                return true;
            }
            catch (Exception) { return false; }
        }
        static public void fileWriteTempData(string str,modelSettings mS)
        {
            try
            {
                string result="";
                StreamWriter data;
                if (!File.Exists(controlFunctions.fixVars(mS.fileTempData)))
                {
                    data = File.CreateText(controlFunctions.fixVars(mS.fileTempData));
                }
                else
                {
                    data = File.AppendText(controlFunctions.fixVars(mS.fileTempData));
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
                controlFunctions.fileWriteLog("Writing to Temp Data File Failed " + e.Message, mS);
            }

        }   
    }
}
