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
using System.Diagnostics;
using System.ComponentModel;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
        public static Image byteArrayToImage(byte[] bytesArr)
        {
            MemoryStream memstr = new MemoryStream(bytesArr);
            Image img = Image.FromStream(memstr);
            return img;
        }
        public static string fixVarsElement(modelElement l1, modelSettings mS)
        {
            string data = "";
            bool none = true;
            if (string.IsNullOrEmpty(l1.data)) { l1.data = ""; }
            if (l1.data.Contains("<<<") && l1.data.Contains(">>>"))
            {

            }
            else if (l1.data.Contains("<<") && l1.data.Contains(">>"))
            {
                data = l1.data;
                if (l1.data.Contains("<<") && l1.data.Contains(">>"))
                {
                    foreach (modelSettingsCustom sc1 in mS.customSettings)
                    {
                        if (data.Contains("<<" + sc1.name + ">>"))
                        {
                            data = data.Replace("<<" + sc1.name + ">>", sc1.value);
                            none = false;
                        }
                    }
                }
            }
            if (l1.data.Contains("<exe>"))
            {
                //MessageBox.Show(data);
                data = data + l1.data.Replace("<exe>", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\");
                none = false;
            }
            if (l1.data.Contains("<ProgramData>"))
            {
                data = data + l1.data.Replace("<ProgramData>", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\");
                none = false;
            }
            if (l1.data.Contains("<longdate>"))
            {
                data = data + l1.data.Replace("<longdate>", DateTime.Now.ToString());
                none = false;
            }
            if (l1.data.Contains("<shortdate>"))
            {
                data = data + l1.data.Replace("<shortdate>", DateTime.Now.ToShortDateString());
                none = false;
            }
            if (none == true) { data = l1.data; }
            return data;
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        static public void fileWriteLog(string str, modelSettings mS)
        {
            StreamWriter log;
            mS.fileLog = controlFunctions.fixVars(mS.fileLog);
            if (!File.Exists(mS.fileLog))
            {
                log = File.CreateText(mS.fileLog);
                log.WriteLine(DateTime.Now + " -- " + str);
                AddFileSecurity(mS.fileLog, "Users", FileSystemRights.FullControl, AccessControlType.Allow);
                log.Close();
            }
            else
            {
                try
                {
                    log = File.AppendText(mS.fileLog);
                    log.WriteLine(DateTime.Now + " -- " + str);
                    log.Close();
                }
                catch (Exception) { }
            }
            if (mS.switchAdminMode == "0") { controlFunctions.emailError(mS, str); }
        }
        public static void AddFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {

            try
            {
                // Get a FileSecurity object that represents the
                // current security settings.
                FileSecurity fSecurity = File.GetAccessControl(fileName);

                // Add the FileSystemAccessRule to the security settings.
                fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                    rights, controlType));

                // Set the new access settings.
                File.SetAccessControl(fileName, fSecurity);
            }
            catch(Exception) { }

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
        public static class ImpersonationUtils
        {
            private const int SW_SHOW = 5;
            private const int TOKEN_QUERY = 0x0008;
            private const int TOKEN_DUPLICATE = 0x0002;
            private const int TOKEN_ASSIGN_PRIMARY = 0x0001;
            private const int STARTF_USESHOWWINDOW = 0x00000001;
            private const int STARTF_FORCEONFEEDBACK = 0x00000040;
            private const int CREATE_UNICODE_ENVIRONMENT = 0x00000400;
            private const int TOKEN_IMPERSONATE = 0x0004;
            private const int TOKEN_QUERY_SOURCE = 0x0010;
            private const int TOKEN_ADJUST_PRIVILEGES = 0x0020;
            private const int TOKEN_ADJUST_GROUPS = 0x0040;
            private const int TOKEN_ADJUST_DEFAULT = 0x0080;
            private const int TOKEN_ADJUST_SESSIONID = 0x0100;
            private const int STANDARD_RIGHTS_REQUIRED = 0x000F0000;
            private const int TOKEN_ALL_ACCESS =
                STANDARD_RIGHTS_REQUIRED |
                TOKEN_ASSIGN_PRIMARY |
                TOKEN_DUPLICATE |
                TOKEN_IMPERSONATE |
                TOKEN_QUERY |
                TOKEN_QUERY_SOURCE |
                TOKEN_ADJUST_PRIVILEGES |
                TOKEN_ADJUST_GROUPS |
                TOKEN_ADJUST_DEFAULT |
                TOKEN_ADJUST_SESSIONID;

            [StructLayout(LayoutKind.Sequential)]
            private struct PROCESS_INFORMATION
            {
                public IntPtr hProcess;
                public IntPtr hThread;
                public int dwProcessId;
                public int dwThreadId;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct SECURITY_ATTRIBUTES
            {
                public int nLength;
                public IntPtr lpSecurityDescriptor;
                public bool bInheritHandle;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct STARTUPINFO
            {
                public int cb;
                public string lpReserved;
                public string lpDesktop;
                public string lpTitle;
                public int dwX;
                public int dwY;
                public int dwXSize;
                public int dwYSize;
                public int dwXCountChars;
                public int dwYCountChars;
                public int dwFillAttribute;
                public int dwFlags;
                public short wShowWindow;
                public short cbReserved2;
                public IntPtr lpReserved2;
                public IntPtr hStdInput;
                public IntPtr hStdOutput;
                public IntPtr hStdError;
            }

            private enum SECURITY_IMPERSONATION_LEVEL
            {
                SecurityAnonymous,
                SecurityIdentification,
                SecurityImpersonation,
                SecurityDelegation
            }

            private enum TOKEN_TYPE
            {
                TokenPrimary = 1,
                TokenImpersonation
            }

            [DllImport("advapi32.dll", SetLastError = true)]
            private static extern bool CreateProcessAsUser(
                IntPtr hToken,
                string lpApplicationName,
                string lpCommandLine,
                ref SECURITY_ATTRIBUTES lpProcessAttributes,
                ref SECURITY_ATTRIBUTES lpThreadAttributes,
                bool bInheritHandles,
                int dwCreationFlags,
                IntPtr lpEnvironment,
                string lpCurrentDirectory,
                ref STARTUPINFO lpStartupInfo,
                out PROCESS_INFORMATION lpProcessInformation);

            [DllImport("advapi32.dll", SetLastError = true)]
            private static extern bool DuplicateTokenEx(
                IntPtr hExistingToken,
                int dwDesiredAccess,
                ref SECURITY_ATTRIBUTES lpThreadAttributes,
                int ImpersonationLevel,
                int dwTokenType,
                ref IntPtr phNewToken);

            [DllImport("advapi32.dll", SetLastError = true)]
            private static extern bool OpenProcessToken(
                IntPtr ProcessHandle,
                int DesiredAccess,
                ref IntPtr TokenHandle);

            [DllImport("userenv.dll", SetLastError = true)]
            private static extern bool CreateEnvironmentBlock(
                    ref IntPtr lpEnvironment,
                    IntPtr hToken,
                    bool bInherit);

            [DllImport("userenv.dll", SetLastError = true)]
            private static extern bool DestroyEnvironmentBlock(
                    IntPtr lpEnvironment);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool CloseHandle(
                IntPtr hObject);

            private static void LaunchProcessAsUser(string cmdLine, IntPtr token, IntPtr envBlock, int sessionId)
            {
                var pi = new PROCESS_INFORMATION();
                var saProcess = new SECURITY_ATTRIBUTES();
                var saThread = new SECURITY_ATTRIBUTES();
                saProcess.nLength = Marshal.SizeOf(saProcess);
                saThread.nLength = Marshal.SizeOf(saThread);

                var si = new STARTUPINFO();
                si.cb = Marshal.SizeOf(si);
                si.lpDesktop = @"WinSta0\Default";
                si.dwFlags = STARTF_USESHOWWINDOW | STARTF_FORCEONFEEDBACK;
                si.wShowWindow = SW_SHOW;

                if (!CreateProcessAsUser(
                    token,
                    null,
                    cmdLine,
                    ref saProcess,
                    ref saThread,
                    false,
                    CREATE_UNICODE_ENVIRONMENT,
                    envBlock,
                    null,
                    ref si,
                    out pi))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateProcessAsUser failed");
                }
            }

            private static IDisposable Impersonate(IntPtr token)
            {
                var identity = new WindowsIdentity(token);
                return identity.Impersonate();
            }

            private static IntPtr GetPrimaryToken(Process process)
            {
                var token = IntPtr.Zero;
                var primaryToken = IntPtr.Zero;

                if (OpenProcessToken(process.Handle, TOKEN_DUPLICATE, ref token))
                {
                    var sa = new SECURITY_ATTRIBUTES();
                    sa.nLength = Marshal.SizeOf(sa);

                    if (!DuplicateTokenEx(
                        token,
                        TOKEN_ALL_ACCESS,
                        ref sa,
                        (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation,
                        (int)TOKEN_TYPE.TokenPrimary,
                        ref primaryToken))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error(), "DuplicateTokenEx failed");
                    }

                    CloseHandle(token);
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "OpenProcessToken failed");
                }

                return primaryToken;
            }

            private static IntPtr GetEnvironmentBlock(IntPtr token)
            {
                var envBlock = IntPtr.Zero;
                if (!CreateEnvironmentBlock(ref envBlock, token, false))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateEnvironmentBlock failed");
                }
                return envBlock;
            }

            public static void LaunchAsCurrentUser(string cmdLine)
            {
                var process = Process.GetProcessesByName("explorer").FirstOrDefault();
                if (process != null)
                {
                    var token = GetPrimaryToken(process);
                    if (token != IntPtr.Zero)
                    {
                        var envBlock = GetEnvironmentBlock(token);
                        if (envBlock != IntPtr.Zero)
                        {
                            LaunchProcessAsUser(cmdLine, token, envBlock, process.SessionId);
                            if (!DestroyEnvironmentBlock(envBlock))
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error(), "DestroyEnvironmentBlock failed");
                            }
                        }

                        CloseHandle(token);
                    }
                }
            }

            public static IDisposable ImpersonateCurrentUser()
            {
                var process = Process.GetProcessesByName("explorer").FirstOrDefault();
                if (process != null)
                {
                    var token = GetPrimaryToken(process);
                    if (token != IntPtr.Zero)
                    {
                        return Impersonate(token);
                    }
                }

                throw new Exception("Could not find explorer.exe");
            }
        }
    }
}
