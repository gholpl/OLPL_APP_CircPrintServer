using APP_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLL_CircPrintServer;
using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;
using System.Diagnostics;

namespace APP_CircPrintServer.Functions
{
    class statsControl
    {
        public static void tickStats(modelSettings mS, string Type)
        {
            if (mS.statsSwitch == "1")
            {
                try
                {
                    string result = "";
                    using (WebClient client = new WebClient { UseDefaultCredentials = true })
                    {

                        byte[] response = client.UploadValues(mS.statsServer, new NameValueCollection()
                {
                    { "strCPU", mS.machineName },
                    { "stringLoc", mS.machineName },
                    { "stringType", Type },
                    { "stringComment", "" }
                });

                        result = System.Text.Encoding.UTF8.GetString(response);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Stats are not working.  Please contact Admin");
                    controlFunctions.fileWriteLog("stats are not working " + e.Message, mS);
                }
            }
            
        }
    }
}
