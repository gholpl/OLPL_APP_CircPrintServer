using APP_CircPrintServer.Forms;
using DLL_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP_CircPrintServer.Functions
{
    class controlValidation
    {
        public static string verifyPrinter(string printer, string type, modelSettings mS)
        {
            if (printer.ToUpper().Contains("NONE") || printer.Length < 1)
            {
                changePrinter cP = new changePrinter();
                cP.lblCurrentPrinter.Text = "Set printer for template " + type + " is " + printer;
                cP.mS = mS;
                cP.printer = printer;
                cP.name = type;
                cP.ShowDialog();
                printer = cP.printer;
                return printer;
            }
            else
            {
                return printer;
            }
        }
    }
}
