using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Clc.Sip;

namespace APP_CircPrintServer.SIP2
{
	public class sip2
	{
		public string hostname = "";
		public int port = 5002;
		//public string library = "";
		public string language = "001";

		//public string AA = "";
		//public string AD = "";
		public string AP = "";
		public string AO = "";
		public string VP = "";
		public string CP = "";

		string AC = "";
		string noBlock = "N";

		StringBuilder messageBuilder = new StringBuilder();
		bool _noFixed = false;

		TcpClient socket = new TcpClient();

		private const char FieldTerminator = '|';
		private const char MessageTerminator = '\r';

		int _seq = -1;

		public sip2()
		{

		}

		public sip2(string hostname, int port, string AO, string AP)
		{
			this.hostname = hostname;
			this.port = port;
			this.AO = AO;
			this.AP = AP;
		}

		public string Parse(string input, string identifier)
		{
			//var value = from string s in input.Split('|') where s.Substring(0, 2) == identifier select string.Join(string.Empty, s.Skip(2)).FirstOrDefault();
			string raw = "";

			//if (identifier == "AO")
			//{
			//    raw = input.Split('|')[0];
			//    Regex regex = new Regex("AO");
			//    var test = regex.Split(raw,2);
			//    return regex.Split(raw, 2)[1];
			//}
			raw = input.Split('|').FirstOrDefault(s => s.Substring(0, 2) == identifier) + "";
			var value = string.Join(string.Empty, raw.Skip(2));
			return value;
		}

		public void AddVariableField(string identifier, string value, bool optional = false)
		{
			if (optional && value == "") return;
			messageBuilder.Append(identifier + value + FieldTerminator);
		}

		public void AddVariableField(string identifier, int value, bool optional = false)
		{
			AddVariableField(identifier, value.ToString(), optional);
		}

		public void AddFixedField(string value, int length)
		{
			if (_noFixed) return;
			messageBuilder.Append(value.PadRight(length));
		}

		public void NewMessage(string code)
		{
			messageBuilder.Clear();
			messageBuilder.Append(code);
			_noFixed = false;
		}

		public void Connect()
		{
			socket.Connect(hostname, port);
		}

		public SipTransaction Login(string sipUsername, string sipPassword, string locationCode = "", string vendorProfile = "")
		{
			this.VP = vendorProfile;
			this.CP = locationCode;
			NewMessage("93");
			AddFixedField("0", 1);
			AddFixedField("0", 1);
			AddVariableField("CN", sipUsername);
			AddVariableField("CO", sipPassword);
			AddVariableField("CP", locationCode);
			AddVariableField("VP", VP, true);
			return SendMessage();
		}

		public SipTransaction EndSession(string patronBarcode, string patronPIN)
		{
			NewMessage("35");
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddVariableField("AO", AO);
			AddVariableField("AA", patronBarcode);
			AddVariableField("AC", AC, true);
			AddVariableField("AD", patronPIN, true);

			return SendMessage();
		}

		public SipTransaction PatronStatus(string patronBarcode, string patronPIN)
		{
			NewMessage("23");
			AddFixedField(language, 3);
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddVariableField("AO", AO);
			AddVariableField("AA", patronBarcode);
			AddVariableField("AC", AC, true);
			AddVariableField("AD", patronPIN, true);

			return SendMessage();
		}

		public SipTransaction PatronInformation(string patronBarcode, string patronPIN, string type = "none", string BP = "1", string BQ = "5")
		{
			var summary = new Dictionary<string, string>();
			summary["none"]		= "      ";
			summary["hold"]		= "Y     ";
			summary["overdue"]	= " Y    ";
			summary["charged"]	= "  Y   ";
			summary["fine"]		= "   Y  ";
			summary["recall"]	= "    Y ";
			summary["unavail"]	= "     Y";

			var test = summary[type];

			NewMessage("63");
			AddFixedField(language, 3);
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddFixedField(summary[type], 10);
			AddVariableField("AO", AO);
			AddVariableField("AA", patronBarcode);
			AddVariableField("AC", AC, true);
			AddVariableField("AD", patronPIN, true);
			AddVariableField("BP", BP, true);
			AddVariableField("BQ", BQ, true);

			return SendMessage();        
		}

		public SipTransaction ItemCheckOut(string patronBarcode, string patronPIN, string itemBarcode, DateTime? dueDate = null, string renewalOk = "Y", string itemProperties = "", string feeAcknowledged = "", string noBlock = "N", string cancel = "Y")
		{		
			NewMessage("11");
			AddFixedField(renewalOk, 1);
			AddFixedField(this.noBlock, 1);
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddFixedField(dueDate.HasValue ? dueDate.Value.ToSipString() : "", 18);
			AddVariableField("AO", AO);
			AddVariableField("AA", patronBarcode);
			AddVariableField("AB", itemBarcode);
			AddVariableField("AD", patronPIN);
			AddVariableField("AC", AC);
			AddVariableField("CH", itemProperties, true);
			AddVariableField("BO", feeAcknowledged, true);
			AddVariableField("BI", cancel, true);

			return SendMessage();			
		}

		public SipTransaction ItemInformation(string itemBarcode)
		{
			NewMessage("17");
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddVariableField("AO", AO);
			AddVariableField("AB", itemBarcode);
			AddVariableField("AC", AC);

			return SendMessage();
		}

		public SipTransaction Renew(string patronBarcode, string patronPIN, DateTime dueDate, string itemBarcode, string title = "", string itemProperties = "", string thirdPartyAllowed = "Y", string noBlock = "Y", string feeAcknowledged = "")
		{
			NewMessage("29");
			AddFixedField(thirdPartyAllowed, 1);
			AddFixedField(this.noBlock, 1);
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddFixedField(dueDate.ToSipString(), 18);
			AddVariableField("AO", AO);
			AddVariableField("AA", patronBarcode);
			AddVariableField("AD", patronPIN, true);
			AddVariableField("AB", itemBarcode, true);
			AddVariableField("AJ", title, true);
			AddVariableField("AC", AC, true);
			AddVariableField("CH", itemProperties, true);
			AddVariableField("BO", feeAcknowledged,true);

			return SendMessage();
		}

		public SipTransaction RenewAll(string patronBarcode, string patronPIN, string feeAcknowledged = "")
		{
			NewMessage("65");
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddVariableField("AO", AO);
			AddVariableField("AA", patronBarcode);
			AddVariableField("AD", patronPIN);
			AddVariableField("AC", AC, true);
			AddVariableField("BO", feeAcknowledged, true);

			return SendMessage();
		}

		public SipTransaction ItemCheckIn(string itemBarcode, DateTime? returnDate = null, string returnLocation = "", string itemProperties = "", string noBlock = "N", string cancel = "")
		{
			NewMessage("09");
			AddFixedField(this.noBlock, 1);
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddFixedField(returnDate.HasValue ? returnDate.Value.ToSipString() : "", 18);
			AddVariableField("AP", returnLocation == "" ? AP : returnLocation);
			AddVariableField("AO", AO);
			AddVariableField("AB", itemBarcode);
			AddVariableField("AC", AC);
			AddVariableField("CH", itemProperties, true);
			AddVariableField("BI", cancel, true);

			return SendMessage();
		}

		public SipTransaction FeePaid(int feeType, int paymentType, double feeAmount, string patronBarcode, string patronPIN, string currencyType = "USD", string feeIdentifier = "", string transactionId = "")
		{
			NewMessage("37");
			AddFixedField(DateTime.Now.ToSipString(), 18);
			AddFixedField(feeType.ToString("00"), 2);
			AddFixedField(paymentType.ToString("00"), 2);
			AddFixedField(currencyType, 3);
			AddVariableField("BV", feeAmount.ToString());
			AddVariableField("AO", AO);
			AddVariableField("AA", patronBarcode);
			AddVariableField("AC", AC, true);
			AddVariableField("AD", patronPIN, true);
			AddVariableField("CG", feeIdentifier, true);
			AddVariableField("BK", transactionId, true);

			return SendMessage();
		}

		public SipTransaction SendMessage()
		{
			var msg = BuildMessage();
			socket.WriteToStream(msg);
            socket.NoDelay=true;
            string test = socket.ReadFromStream();
            test = test + socket.ReadFromStream();
			return new SipTransaction { Message = msg, Response = test };
			//return socket.ReadFromStream();

		}

		public string BuildMessage()
		{
			messageBuilder.Append("AY" + GetSeqNumber());
			messageBuilder.Append("AZ");
			messageBuilder.Append(CalculateChecksum(messageBuilder.ToString()));
			messageBuilder.Append(MessageTerminator);
			var check = messageBuilder.ToString();
			var test = messageBuilder.ToString();
			return messageBuilder.ToString();
		}

		static string CalculateChecksum(string msg)
		{
			int checksum = 0;

			foreach (char c in msg)
			{
				checksum += (int)c;
			}

			//checksum = checksum & 0xffff;			
			checksum *= -1;
			var hex = String.Format("{0:X2}", Convert.ToUInt64(Convert.ToString(checksum, 2), 2));
			return hex.Substring(hex.Length - 4);
		}

		public int GetSeqNumber()
		{
			_seq = _seq == 9 ? 0 : ++_seq;
			return _seq;
		}
	}
}
