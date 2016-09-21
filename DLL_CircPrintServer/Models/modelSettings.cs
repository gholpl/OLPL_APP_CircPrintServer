using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_CircPrintServer.Models
{
    public class modelSettings
    {
        public string tempLocation { get; set; }
        public string tempCirc { get; set; }
        public string tempInTransit { get; set; }
        public string tempHoldsP1 { get; set; }
        public string tempHoldsP2 { get; set; }
        public string tempFine { get; set; }
        public string tempSerialRoute { get; set; }
        public string tempUserRecord { get; set; }
        public string printerCirc { get; set; }
        public string printerInTransit { get; set; }
        public string printerHolds { get; set; }
        public string printerFine { get; set; }
        public string printerUserRecord { get; set; }
        public string printerSerialRoute { get; set; }
        public string fileInTransitFrom { get; set; }
        public string fileInTransitTo { get; set; }
        public string fileTempData { get; set; }
        public string fileLog { get; set; }
        public string statsSwitch { get; set; }
        public string statsServer { get; set; }
        public string switchTwoPageHolds { get; set; }
        public string switchAdminMode { get; set; }
        public string switchAskCheckOut { get; set; }
        public string switchAskPayment { get; set; }
        public string switchAskUser { get; set; }
        public string switchAskTransit{ get; set; }
        public string switchAskHolds { get; set; }
        public string switchAskSerial { get; set; }
        public string pathEXE { get; set; }
        public string machineName { get; set; }
        public string notificationHoldsCallSwitch { get; set; }
        public string notificationHoldsCallServer { get; set; }
        public string notificationHoldsAPICallSwitch { get; set; }
        public string notificationHoldsCallVoipServer { get; set; }
        public string notificationHoldsCallVoipPort { get; set; }
        public string notificationHoldsCallVoipUserName { get; set; }
        public string notificationHoldsCallVoipPassword { get; set; }
        public bool POSEnable { get; set; }
        public string POSServerAPI { get; set; }
        public bool POSEmailEnable { get; set; }
        public string POSServerEmailAPI { get; set; }
        public string POSServerEmailRefund { get; set; }
        public List<modelSettingsCustom> customSettings { get; set; }
        public string SIPServerIP { get; set; }
        public string SIPServerPort { get; set; }
        public string SIPUserName { get; set; }
        public string SIPUserPassword { get; set; }
        public bool POSDebugLogging { get; set; }
        public string POSDataFolder { get; set; }
        public string ErrorEMailServer { get; set; }
        public bool ErrorEMailEnable { get; set; }
        public string ErrorEMailAddress { get; set; }
        public bool backupFileRunControl { get; set; }
        public string viewAdvanced { get; set; }
        public bool generateBackup { get; set; }
    }
}
