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
using static DLL_CircPrintServer.Classes.controlFunctions;

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
}
