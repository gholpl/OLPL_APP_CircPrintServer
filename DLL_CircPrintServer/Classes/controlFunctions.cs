using DLL_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DLL_CircPrintServer.Classes
{
    public static class controlFunctions
    {
        public static string fixVars(string input)
        {
            //MessageBox.Show(input);
            if (string.IsNullOrEmpty(input)) { input = ""; }
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
            if (input.Contains("<ProgramData>"))
            {
                data = input.Replace("<ProgramData>", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\");
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
        static public void fileWriteLog(string str, modelSettings mS)
        {
            StreamWriter log;
            mS.fileLog = controlFunctions.fixVars(mS.fileLog);
            if (!File.Exists(mS.fileLog))
            {
                log = File.CreateText(mS.fileLog);
            }
            else
            {
                log = File.AppendText(mS.fileLog);
            }
            if (mS.switchAdminMode == "0") { controlFunctions.emailError(mS, str); }
            log.WriteLine(DateTime.Now + " -- " + str);
            log.Close();
        }
        internal static string emailError(modelSettings mS, string body)
        {
            try
            {
                using (WebClient client = new WebClient { UseDefaultCredentials = true })
                {
                    string result = "";
                    byte[] response = client.UploadValues(mS.ErrorEMailServer, new NameValueCollection()
                    {
                         { "from","POSSystem@olpl.org" },
                         { "to", mS.ErrorEMailAddress },
                         { "subject", "POS System Error Report" },
                         { "body",body },
                    });
                    result = System.Text.Encoding.UTF8.GetString(response);

                    if (result.Contains("OK"))
                    {
                        return "OK";
                    }
                    else
                    {
                        return "Error";
                    }
                }
            }
            catch (Exception e1) { controlFunctions.fileWriteLog(DateTime.Now + "Send Error Email problem " + e1.ToString(), mS); return "Error"; }
        }
        static public string readTransitTo(modelSettings mS, string code, string type)
        {
            string str = "none";
            try
            {
                string[] lineArray = File.ReadAllLines(controlFunctions.fixVars(mS.fileInTransitTo));
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
        public static string proccessUserRecord(string data, string type)
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
                if (line.Contains("Basic Info") || line.Contains("USER ID:")) { typeStr = "Basic"; }
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
        public static string HTMLtoTXT(string str)
        {
            string result = "";
            string str1 = StripHTML(str);
            foreach (string str11 in str1.Split(Environment.NewLine.ToCharArray()))
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
        public static string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
        }
        public class INIFile
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
}
