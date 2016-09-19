using APP_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL_CircPrintServer;
using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;

namespace APP_CircPrintServer.Functions
{
    class listControl
    {
        static internal List<modelCheckout> proccessCheckout(modelSettings mS, string[] checkouts)
        {
            List<modelCheckout> listCheckout = new List<modelCheckout>();
            foreach (string stringLine in checkouts)
            {
                bool add = false;
                modelCheckout cC = new modelCheckout();
                foreach (string stringItem in stringLine.Split('^'))
                {
                    if (stringItem.Contains("Item ID:"))
                    {
                        cC.itemid = stringItem.Replace("Item ID: ", string.Empty);
                        add = true;
                    }
                    else if (stringItem.Contains("Title"))
                    {
                        cC.title = stringItem.Replace("Title: ", string.Empty);
                    }
                    else if (stringItem.Contains("Date due:"))
                    {
                        DateTime dt = new DateTime();
                        if (DateTime.TryParse(stringItem.Replace("Date due: ", string.Empty).Replace(",", " "), out dt))
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
                if (add)
                {
                    listCheckout.Add(cC);
                }
            }
            return listCheckout;
        }
    }
}
