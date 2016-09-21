using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;

namespace OLPL_APP_CircPrinter_Admin.Functions
{
    class validationControl
    {
        internal static void testSettingsWrite(Form1 fc)
        {
            try
            {
                fileControl.writeSettingsFile(fc);
            }
            catch(Exception)
            {
                MessageBox.Show("Cant write to settings file.  Start as Administrator");  
            }
        }
        internal static void testPrinter(Form1 fc)
        {
            try
            {
                PrinterSettings ps = new PrinterSettings();
                ps.PrinterName = "SirsiPrinter"; // Load the appropriate printer's setting
               // ps.
                string printerName = "SirsiPrinter";
                string query = string.Format("SELECT * from Win32_Printer WHERE Name LIKE '%{0}'", printerName);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection coll = searcher.Get();

                foreach (ManagementObject printer in coll)
                {
                    foreach (PropertyData property in printer.Properties)
                    {
                        //fc.log.WriteLine(string.Format("{0}: {1}", property.Name, property.Value));
                        //MessageBox.Show(string.Format("{0}: {1}", property.Name, property.Value));
                    }
                }
            }
            catch(Exception)
            {
                //controlFunctions.fileWriteLog(e.Message,mS);
            }
        }
        internal static void setupViews(Form1 fc, modelSettings mS)
        {
            if(mS.viewAdvanced == "0")
            {
                fc.tabControl1.TabPages.RemoveByKey("tabPage4");
                fc.tabControl1.TabPages.RemoveByKey("tabPOS");
                fc.tabControl1.TabPages.RemoveByKey("tabPage7");
            }
            else
            {
                
            }
        }
    }
}
