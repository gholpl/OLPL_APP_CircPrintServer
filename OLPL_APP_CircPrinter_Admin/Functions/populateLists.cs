using DLL_CircPrintServer.Models;
using OLPL_APP_CircPrinter_Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLPL_APP_CircPrinter_Admin.Functions
{
    class populateLists
    {
        static internal void populateTemplateListView(Form1 fc)
        {
            if (fc.listTemplates.SelectedItems.Count > 0)
            {
                string name = fc.listTemplates.SelectedItems[0].Text;
            }
            
            if (fc.tl1.Count > 1)
            {
                if (fc.cb_temp_circ.SelectedText.Length > 3) { fc.tempCirc = fc.cb_temp_circ.SelectedText; }
                if (fc.cb_temp_fine.SelectedText.Length > 3) {  fc.tempFine = fc.cb_temp_fine.SelectedText;}
                if (fc.cb_temp_holds_p1.SelectedText.Length > 3) { fc.tempHoldsP1 = fc.cb_temp_holds_p1.SelectedText; }
                if (fc.cb_temp_holds_p2.SelectedText.Length > 3) { fc.tempHoldsP2 = fc.cb_temp_holds_p2.SelectedText; }
                if (fc.cb_temp_transit.SelectedText.Length > 3) { fc.tempInTransit = fc.cb_temp_transit.SelectedText; }
                if (fc.cb_temp_serialRoute.SelectedText.Length > 3) { fc.tempSerialRoute = fc.cb_temp_serialRoute.SelectedText; }
                if (fc.cb_temp_UserRecord.SelectedText.Length > 3) { fc.tempUserRecord = fc.cb_temp_UserRecord.SelectedText; }
                fc.cb_temp_circ.Items.Clear();
                fc.cb_temp_fine.Items.Clear();
                fc.listTemplates.Items.Clear();
                fc.cb_temp_holds_p1.Items.Clear();
                fc.cb_temp_holds_p2.Items.Clear();
                fc.cb_temp_transit.Items.Clear();
                fc.cb_temp_serialRoute.Items.Clear();
                fc.cb_temp_UserRecord.Items.Clear();
                foreach (modelTemplate cl1 in fc.tl1)
                {
                    ListViewItem item = new ListViewItem(new[] { cl1.name, cl1.type });
                    fc.listTemplates.Items.Add(item);
                    if (cl1.type == "Financial"){fc.cb_temp_fine.Items.Add(cl1.name);}
                    else if(cl1.type == "Circulation") { fc.cb_temp_circ.Items.Add(cl1.name); }
                    else if(cl1.type == "InTransit"){ fc.cb_temp_transit.Items.Add(cl1.name); }
                    else if (cl1.type == "Holds")
                    {
                        fc.cb_temp_holds_p1.Items.Add(cl1.name);
                        fc.cb_temp_holds_p2.Items.Add(cl1.name);
                    }
                    else if (cl1.type == "SerialRoute") { fc.cb_temp_serialRoute.Items.Add(cl1.name); }
                    else if (cl1.type == "UserRecord") { fc.cb_temp_UserRecord.Items.Add(cl1.name); }
                }
                populateTempDropDowns(fc);
            }
        }
        static void populateTempDropDowns(Form1 fc)
        {
            bool found = false;
            foreach(string str in fc.cb_temp_circ.Items)
            {
                if (str == fc.tempCirc)
                {
                    found = true;
                    fc.cb_temp_circ.SelectedItem = fc.tempCirc;
                }
            }
            if (!found)
            {
                fc.cb_temp_circ.Items.Add(fc.tempCirc + " (Not Found)");
                fc.cb_temp_circ.SelectedItem = fc.tempCirc + " (Not Found)";
            }
            found = false;
            foreach (string str in fc.cb_temp_fine.Items)
            {
                if (str == fc.tempFine)
                {
                    found = true;
                    fc.cb_temp_fine.SelectedItem = fc.tempFine;
                }
            }
            if (!found)
            {
                fc.cb_temp_fine.Items.Add(fc.tempFine + " (Not Found)");
                fc.cb_temp_fine.SelectedItem = fc.tempFine + " (Not Found)";
            }
            found = false;
            foreach (string str in fc.cb_temp_transit.Items)
            {
                if (str == fc.tempInTransit)
                {
                    found = true;
                    fc.cb_temp_transit.SelectedItem = fc.tempInTransit;
                }
            }
            if (!found)
            {
                fc.cb_temp_transit.Items.Add(fc.tempInTransit + " (Not Found)");
                fc.cb_temp_transit.SelectedItem = fc.tempInTransit + " (Not Found)";
            }
            found = false;
            foreach (string str in fc.cb_temp_holds_p1.Items)
            {
                if (str == fc.tempHoldsP1)
                {
                    found = true;
                    fc.cb_temp_holds_p1.SelectedItem = fc.tempHoldsP1;
                }
            }
            if (!found)
            {
                fc.cb_temp_holds_p1.Items.Add(fc.tempHoldsP1 + " (Not Found)");
                fc.cb_temp_holds_p1.SelectedItem = fc.tempHoldsP1 + " (Not Found)";
            }
            found = false;
            foreach (string str in fc.cb_temp_holds_p2.Items)
            {
                if (str == fc.tempHoldsP2)
                {
                    found = true;
                    fc.cb_temp_holds_p2.SelectedItem = fc.tempHoldsP2;
                }
            }
            if (!found)
            {
                fc.cb_temp_holds_p2.Items.Add(fc.tempHoldsP2 + " (Not Found)");
                fc.cb_temp_holds_p2.SelectedItem = fc.tempHoldsP2 + " (Not Found)";
            }
            found = false;
            foreach (string str in fc.cb_temp_serialRoute.Items)
            {
                if (str == fc.tempSerialRoute)
                {
                    found = true;
                    fc.cb_temp_serialRoute.SelectedItem = fc.tempSerialRoute;
                }
            }
            if (!found)
            {
                fc.cb_temp_serialRoute.Items.Add(fc.tempSerialRoute + " (Not Found)");
                fc.cb_temp_serialRoute.SelectedItem = fc.tempSerialRoute + " (Not Found)";
            }
            found = false;
            foreach (string str in fc.cb_temp_UserRecord.Items)
            {
                if (str == fc.tempUserRecord)
                {
                    found = true;
                    fc.cb_temp_UserRecord.SelectedItem = fc.tempUserRecord;
                }
            }
            if (!found)
            {
                fc.cb_temp_UserRecord.Items.Add(fc.tempUserRecord + " (Not Found)");
                fc.cb_temp_UserRecord.SelectedItem = fc.tempUserRecord + " (Not Found)";
            }
        }
        static internal void populateList(Form1 fc)
        {
            ListViewItem lvi = new ListViewItem();
            fc.listElements.Items.Clear();
            fc.btnModifyElement.Visible = true;
            fc.btnSaveTemplate.Visible = true;
            fc.btnDeleteElement.Visible = true;
            fc.btnMoveDOwnElement.Visible = true;
            fc.btnMoveUpElelement.Visible = true;
            foreach (modelElement ec1 in fc.el1)
            {
                ListViewItem item = new ListViewItem(new[] { ec1.id.ToString(), ec1.name, ec1.data });
                fc.listElements.Items.Add(item);

            }
        }
        static internal void populateCustomSettings(Form1 fc)
        {
            if (fc.sList.Count > 0)
            {
                fc.listSettings.Items.Clear();
                foreach(settingsClass sc1 in fc.sList)
                {
                    ListViewItem ls1 = new ListViewItem(new[] { sc1.name, sc1.value });
                    fc.listSettings.Items.Add(ls1);
                }
            }
        }
        static internal void populatePrinters(Form1 fc)
        {
            using (ImpersonationUtils.ImpersonateCurrentUser())
            {
                foreach (string str in PrinterSettings.InstalledPrinters)
                {
                    fc.cbCircPrinter.Items.Add(str);
                    fc.cbInTransitPrinter.Items.Add(str);
                    fc.cbHoldsPrinter.Items.Add(str);
                    fc.cbFinePrinter.Items.Add(str);
                    fc.cbSerialRoutePrinter.Items.Add(str);
                    fc.cbUserRecordPrinter.Items.Add(str);
                }
            }
            
        }
        static internal void addCustomSetting(Form1 fc,settingsClass sc1)
        {
            settingsClass sc3 = new settingsClass();
            foreach(settingsClass sc2 in fc.sList)
            {
                if(sc1.name == sc2.name)
                {
                    sc3 = sc2;
                }
            }
            fc.sList.Remove(sc3);
            fc.sList.Add(sc1);
            populateLists.populateCustomSettings(fc);
            fc.notsaved = false;
        }
        static internal List<checkoutClass> proccessCheckout(Form1 fc, string[] checkouts)
        {
            List<checkoutClass> listCheckout = new List<checkoutClass>();
            foreach(string stringLine in checkouts)
            {
                checkoutClass cC = new checkoutClass();
                foreach (string stringItem in stringLine.Split('^'))
                {
                    if (stringItem.Contains("Item ID:"))
                    {
                        cC.itemid = stringItem.Replace("Item ID: ", string.Empty);
                    }
                    else if (stringItem.Contains("Title"))
                    {
                        cC.title = stringItem.Replace("Title: ", string.Empty);
                    }
                    else if (stringItem.Contains("Date due:"))
                    {
                        DateTime dt = new DateTime();
                        if(DateTime.TryParse(stringItem.Replace("Date due: ", string.Empty).Replace(",", " "),out dt))
                        {
                            cC.duedate = dt;
                        }
                        else
                        {
                            dt = DateTime.Today;
                            cC.duedate = dt.AddDays(21);
                        }
                    }
                }
                listCheckout.Add(cC);
            }
            return listCheckout;
        }
        static internal void deleteElement(Form1 fc)
        {
            int index = 1;
            modelElement ec1 = new modelElement();
            foreach(modelElement ec in fc.el1)
            {
                if(fc.listElements.SelectedItems[0].SubItems[0].Text == ec.id.ToString())
                {
                    ec1 = ec;
                }
            }
            fc.el1.Remove(ec1);
            foreach (modelElement ec in fc.el1)
            {
                ec.id = index;
                index++;
            }
        }
        static internal void moveElement(Form1 fc,string updown)
        {
            if (fc.listElements.SelectedItems.Count > 0)
            {
                int index = 1;
                int indexSelected = int.Parse(fc.listElements.SelectedItems[0].SubItems[0].Text);
                int indexUP = indexSelected - 1;
                int indexDown = indexSelected;
                modelElement ecSelected = new modelElement();
                modelElement ecUp = new modelElement();
                modelElement ecDown = new modelElement();
                foreach (modelElement ec in fc.el1)
                {

                    if (indexUP.ToString() == ec.id.ToString())
                    {
                        ecUp = ec;
                    }
                    if (indexDown.ToString() == ec.id.ToString())
                    {
                        ecDown = ec;
                    }
                    if (indexSelected.ToString() == ec.id.ToString())
                    {
                        ecSelected = ec;
                    }
                }
                fc.el1.Remove(ecSelected);
                if (updown == "up")
                {
                    fc.el1.Insert(indexUP-1, ecSelected);
                }
                if (updown == "down")
                {
                    fc.el1.Insert(indexDown, ecSelected);
                }
                foreach (modelElement ec in fc.el1)
                {
                    ec.id = index;
                    index++;
                }
            }
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
