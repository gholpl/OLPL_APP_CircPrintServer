using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP_CircPrintServer.Models
{
    class modelPOS
    {
        internal string operatorID { get; set; }
        internal string userID { get; set; }
        internal string title { get; set; }
        internal string itemID { get; set; }
        internal DateTime paymentDate { get; set; }
        internal Decimal totalBill { get; set; }
        internal string author { get; set; }
        internal string callnumber { get; set; }
        internal string status { get; set; }
        internal string type { get; set; }
        internal Decimal amtCol { get; set; }
        internal string reason { get; set; }
        internal Decimal tax { get; set; }
        internal Decimal change { get; set; }
        internal Decimal amtDrawer { get; set; }
        internal string transType { get; set; }
        internal int quantity { get; set; }
        internal int transID { get; set; }
        internal string stationType { get; set; }
        internal string strData { get; set; }
    }
    class modelPOSTrans
    {
        public double transID { get; set; }
        public string operatorID { get; set; }
        public string userID { get; set; }
        public string compName { get; set; }
        public string stationType { get; set; }
        public string strData { get; set; }
        public string transType { get; set; }
        public string transDate { get; set; }
    }
    class modelPOSTransLine
    {
        public double idTrans { get; set; }
        public int idLine { get; set; }
        public string itemID { get; set; }
        public string itemTitle { get; set; }
        public string itemCallNumber { get; set; }
        public string itemAuthor { get; set; }
        public int itemQuantity { get; set; }
        public decimal itemPrice { get; set; }
        public string paymentType { get; set; }
        public decimal paymentAmount { get; set; }
        public decimal paymentChange { get; set; }
        public decimal paymentTotal { get; set; }
        public string billReason { get; set; }
        public decimal billTotal { get; set; }
    }
}
