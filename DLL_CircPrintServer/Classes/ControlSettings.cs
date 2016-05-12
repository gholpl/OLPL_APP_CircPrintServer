using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DLL_CircPrintServer.Models;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLL_CircPrintServer.Classes
{
    
    public class ControlSettings
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        static public List<modelSettings> readSettingFile(string pathEXE)
        {
            List<modelSettings> set1 = new List<modelSettings>(); 
            try
            {
                
                string inputSettings = File.ReadAllText(pathEXE + "\\program.settings");
               // string inputSettings = File.ReadAllText("C:\\Program Files (x86)\\CircPrintServer\\program.settings");
               // MessageBox.Show(inputSettings);
                string[] inputSettingsArray = inputSettings.Split(Environment.NewLine.ToCharArray());
                foreach (string str in inputSettingsArray)
                {
                    modelSettings s1 = new modelSettings();
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
                modelSettings s1 = new modelSettings();
                s1.value = "error";
                s1.value = e1.Message;
                set1.Add(s1);
                return set1;
            }
        }

    }
}
