using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;

namespace Clc.Sip
{
	public static class Extensions
	{
		public static void WriteToStream(this TcpClient clientSocket, string message)
		{
			NetworkStream serverStream = clientSocket.GetStream();
			byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message);
			serverStream.Write(outStream, 0, outStream.Length);
			serverStream.Flush();
		}

		public static string ReadFromStream(this TcpClient clientSocket)
		{
			NetworkStream serverStream = clientSocket.GetStream();
			var data = new Byte[clientSocket.ReceiveBufferSize];
			// String to store the response ASCII representation.
			String responseData = String.Empty;

			// Read the first batch of the TcpServer response bytes.
			Int32 bytes = serverStream.Read(data, 0, data.Length);
			responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
			return responseData;
		}

		public static string ToSipString(this DateTime dt)
		{
			return DateTime.Now.ToString("yyyyMMdd    HHmmss");
		}

		public static void AppendText(this RichTextBox box, string text, Color color)
		{
			Color _color;
			string txt;

			foreach (var c in text)
			{
				if (c == '|')
				{
					_color = Color.Black;
					txt = string.Format("{0}", c.ToString());
				}
				else
				{
					txt = c.ToString();
					_color = color;

				}
				box.SelectionStart = box.TextLength;
				box.SelectionLength = 0;

				box.SelectionColor = _color;
				box.AppendText(txt);
				box.SelectionColor = box.ForeColor;
			}
		}

		public static string ToY_N(this bool b)
		{
			return b ? "Y" : "N";
		}
	}
}
