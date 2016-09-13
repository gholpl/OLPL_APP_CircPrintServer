using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP_CircPrintServer.Models
{
    class modelSettings1
    {
        internal string tempLocation { get; set; }
        internal string tempCirc { get; set; }
        internal string tempInTransit { get; set; }
        internal string tempHoldsP1 { get; set; }
        internal string tempHoldsP2 { get; set; }
        internal string tempFine { get; set; }
        internal string tempSerialRoute { get; set; }
        internal string tempUserRecord { get; set; }
        internal string printerCirc { get; set; }
        internal string printerInTransit { get; set; }
        internal string printerHolds { get; set; }
        internal string printerFine { get; set; }
        internal string printerUserRecord { get; set; }
        internal string printerSerialRoute { get; set; }
        internal string fileInTransitFrom { get; set; }
        internal string fileInTransitTo { get; set; }
        internal string fileTempData { get; set; }
        internal string fileLog { get; set; }
        internal string statsSwitch { get; set; }
        internal string statsServer { get; set; }
        internal string switchTwoPageHolds { get; set; }
        internal string switchAdminMode { get; set; }
        internal string switchAskCheckOut { get; set; }
        internal string switchAskPayment { get; set; }
        internal string switchAskUser { get; set; }
        internal string switchAskTransit{ get; set; }
        internal string switchAskHolds { get; set; }
        internal string switchAskSerial { get; set; }
        internal string pathEXE { get; set; }
        internal string machineName { get; set; }
        internal string notificationHoldsCallSwitch { get; set; }
        internal string notificationHoldsCallServer { get; set; }
        internal string notificationHoldsAPICallSwitch { get; set; }
        internal string notificationHoldsCallVoipServer { get; set; }
        internal string notificationHoldsCallVoipPort { get; set; }
        internal string notificationHoldsCallVoipUserName { get; set; }
        internal string notificationHoldsCallVoipPassword { get; set; }
        internal bool POSEnable { get; set; }
        internal string POSServerAPI { get; set; }
        internal bool POSEmailEnable { get; set; }
        internal string POSServerEmailAPI { get; set; }
        internal string POSServerEmailRefund { get; set; }
        internal List<modelSettingCustom> customSettings { get; set; }
        internal string SIPServerIP { get; set; }
        internal string SIPServerPort { get; set; }
        internal string SIPUserName { get; set; }
        internal string SIPUserPassword { get; set; }
        internal bool POSDebugLogging { get; set; }
        internal string POSDataFolder { get; set; }
        internal string ErrorEMailServer { get; set; }
        internal bool ErrorEMailEnable { get; set; }
        internal string ErrorEMailAddress { get; set; }
        internal bool backupFileRunControl { get; set; }
    }
}
