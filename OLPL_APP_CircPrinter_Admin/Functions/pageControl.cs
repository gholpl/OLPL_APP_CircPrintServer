using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;
using OLPL_APP_CircPrinter_Admin.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLPL_APP_CircPrinter_Admin.Functions
{
    class pageControl
    { 
        internal static void PrintPage(object sender, PrintPageEventArgs e)
        {
            Form1 fc = new Form1();
            fc = (Form1)Application.OpenForms["Form1"];
            modelSettings mS = fc.mS;
            float x = 10;
            float y = 5;
            float width = 270.0F;
            float height = 0F;
            string data1 = "";
            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;
            foreach (modelElement l1 in fc.el1)
            {
                modelElement l2 = l1;
                data1 = controlFunctions.fixVarsElement(l2,mS);
                #region Logo
                if (l1.name == "Logo")
                {
                    try
                    {
                        Bitmap logo = controlFunctions.ResizeImage(new Bitmap(data1), l1.width, l1.height);
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawImage(logo, width - (logo.Width), y + l1.spaceTop);
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawImage(logo, 10, y + l1.spaceTop);
                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawImage(logo, e.MarginBounds.Left + (e.MarginBounds.Width / 2) - (logo.Width / 2), y + l1.spaceTop);
                        }

                        y += l1.spaceTop;
                        y += logo.Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " Logo import problem " + l2.data); }
                }
                #endregion
                #region Textline
                else if (l1.name == "Textline")
                {
                    try
                    {
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        string text = data1;
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline render problem " + l2.data); }
                }
                #endregion
                #region Checkout Block
                else if (l1.name == "Checkout Block")
                {
                    string[] arrayFileRead = new string[2];
                    arrayFileRead[0] = "Title: Stink : the incredible shrinking kid hgugu guhgugu hjguguyg^Item ID: 31186008763559^Date due: 9/14/2015,23:59^";
                    arrayFileRead[1] = "Title: Judy Moody gets famous!hihgi ghigh g gbh vbjh vjh vvh vhj v vv vvvuv v v vu^Item ID: 31186006897045^Date due: 9/14/2015,23:59^";
                    List<checkoutClass> listCheckout = populateLists.proccessCheckout(fc, arrayFileRead);
                    try
                    {
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        y += l1.spaceTop;
                        foreach (checkoutClass cC in listCheckout)
                        {
                            string text = ""; 
                            if (l1.align == "Right")
                            {
                                text = "Item ID: " + cC.itemid;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                text = "Title: " + cC.title;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                text = "Due Date: " + cC.duedate.ToShortDateString();
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                            else if (l1.align == "Left")
                            {
                                text = "Item ID: " + cC.itemid;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                text = "Title: " + cC.title;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                text = "Due Date: " + cC.duedate.ToShortDateString();
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                            else if (l1.align == "Center")
                            {
                                text = "Item ID: " + cC.itemid;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                text = "Title: " + cC.title;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                text = "Due Date: " + cC.duedate.ToShortDateString();
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                        }
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline render problem " + l2.data); }       
                }
                #endregion
                #region Payment Block
                else if (l1.name == "Payment Block")
                {
                    string[] arrayFileRead = new string[2];
                    arrayFileRead[0] = "Payment date 4/11/2015" + Environment.NewLine+ "Title The conan Doyle Notes: The secret of jack the Ri" + Environment.NewLine + "Beill Reason: PROCCESSFEE" + Environment.NewLine + "Original Bill: $500.00" + Environment.NewLine + "Amount Paid $3.00" + Environment.NewLine + "Remaining Balance: $401.90";
                    try
                    {
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        y += l1.spaceTop;
                        foreach (string cC in arrayFileRead)
                        {
                            string text = "";
                            if (l1.align == "Right")
                            {
                                text =  cC;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                            else if (l1.align == "Left")
                            {
                                text =  cC;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                            else if (l1.align == "Center")
                            {
                                text =  cC;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                        }
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " Payment block render problem " + l2.data); }
                }
                #endregion
                #region Barcode - ToLibrary
                else if (l1.name == "Barcode - ToLibrary")
                {
                    Image toBcodeImage;
                    string data = "Transit To: OLS" + Environment.NewLine + "Item ID: 31186006897045" + Environment.NewLine + "Title: Test title fshfdljhs kjhgfdksjhg kjfhg kdlskfdjh gklsjdhkfdg hlksjdhkdfsjh glk";
                    try
                    {
                        foreach(string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if(line.Contains("Transit To: "))
                            {
                                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                                b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                                b.IncludeLabel = true;
                                BarcodeLib.TYPE type = BarcodeLib.TYPE.Codabar;
                                toBcodeImage = b.Encode(type, "a" + controlFunctions.readTransitTo(mS, line.Replace("Transit to: ", "").Trim(),"barcode") + "d", Color.Black, Color.White, 250, 80);
                                if (l1.align == "Right")
                                {
                                    e.Graphics.DrawImage(toBcodeImage, width - (toBcodeImage.Width), y + l1.spaceTop);
                                }
                                else if (l1.align == "Left")
                                {
                                    e.Graphics.DrawImage(toBcodeImage, 10, y + l1.spaceTop);
                                }
                                else if (l1.align == "Center")
                                {
                                    e.Graphics.DrawImage(toBcodeImage, e.MarginBounds.Left + (e.MarginBounds.Width / 2) - (toBcodeImage.Width / 2), y + l1.spaceTop);
                                }

                                y += l1.spaceTop;
                                y += toBcodeImage.Height;
                            }
                        }
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " Barcode tolibrary proccessing problem"); }
                }
                #endregion
                #region Barcode - ItemID
                else if (l1.name == "Barcode - ItemID")
                {
                    Image toBcodeImage;
                    string data = "Transit To: OLS" + Environment.NewLine + "Item ID: 31186006897045" + Environment.NewLine + "Title: Test title fshfdljhs kjhgfdksjhg kjfhg kdlskfdjh gklsjdhkfdg hlksjdhkdfsjh glk";
                    try
                    {
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Item ID: "))
                            {
                                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                                b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                                b.IncludeLabel = true;
                                BarcodeLib.TYPE type = BarcodeLib.TYPE.Codabar;
                                toBcodeImage = b.Encode(type, "a" + line.Replace("Item ID: ", "") + "d", Color.Black, Color.White, 250, 80);
                                if (l1.align == "Right")
                                {
                                    e.Graphics.DrawImage(toBcodeImage, width - (toBcodeImage.Width), y + l1.spaceTop);
                                }
                                else if (l1.align == "Left")
                                {
                                    e.Graphics.DrawImage(toBcodeImage, 10, y + l1.spaceTop);
                                }
                                else if (l1.align == "Center")
                                {
                                    e.Graphics.DrawImage(toBcodeImage, e.MarginBounds.Left + (e.MarginBounds.Width / 2) - (toBcodeImage.Width / 2), y + l1.spaceTop);
                                }

                                y += l1.spaceTop;
                                y += toBcodeImage.Height;
                            }
                        }
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " Barcode tolibrary proccessing problem"); }
                }
                #endregion
                #region Textline - FirstName
                else if (l1.name == "Textline - FirstName")
                {
                    string data = "Item ID: 31186006897045" + Environment.NewLine + "Email1: mtruty@olpl.org" +
                        Environment.NewLine + "Phone number: 7085555555" + Environment.NewLine + "User name: Truty, Marcin" +
                        Environment.NewLine + "Pickup By: 9/20/2015";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("User name: "))
                            {
                                if (line.Contains(","))
                                {
                                    string[] strun = line.Split(',');
                                    text = strun[1].Replace("User name: ", "");
                                }
                                else
                                {
                                    text = "No Name";
                                }

                            }
                        }
                        int size = 0;
                        if(int.TryParse(l1.data,out size))
                        {
                            string newText = "";
                            int count = 0;
                            foreach(char c in text)
                            {
                                if (count < size) { newText = newText + c; }
                                count++;
                            }
                            text = newText;
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                     
                                    e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                                    y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                               
                        }

                        else if (l1.align == "Left")
                        {
                           
                                    e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                                    y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                
                        }
                        else if (l1.align == "Center")
                        {
                            
                                    e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                                    y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                
                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Textline - From Library Name
                else if (l1.name == "Textline - FromLibraryName")
                {
                    string data = "Transit Slip DGS" +Environment.NewLine + "Transit To: OLS" + Environment.NewLine + "Item ID: 31186006897045" + Environment.NewLine + "Title: Test title fshfdljhs kjhgfdksjhg kjfhg kdlskfdjh gklsjdhkfdg hlksjdhkdfsjh glk";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Transit Slip"))
                            {
                                text = data1 + " " + controlFunctions.readTransitTo(mS, line.Replace("Transit Slip ", ""), "name");
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline intransit library name render problem " + l2.data); }
                }
                #endregion
                #region Textline - HoldExpire
                else if (l1.name == "Textline - HoldExpire")
                {
                    string data = "Item ID: 31186006897045" + Environment.NewLine + "Email1: mtruty@olpl.org" +
                        Environment.NewLine + "Phone number: 7085555555" + Environment.NewLine + "User name: Truty, Marcin" +
                        Environment.NewLine + "Pickup By: 9/20/2015";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Pickup By: "))
                            {
                                text=line.Replace("Pickup By: ", "");
                            }
                        }
                        if (text.Length < 3)
                        {
                            DateTime dt2 = DateTime.Now.AddDays(7);
                            text = dt2.ToString("MM") + "/" + dt2.ToString("dd") + "/" + dt2.ToString("yyyy");
                        }
                        text = l1.data + " " + text;
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {

                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }

                        else if (l1.align == "Left")
                        {

                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {

                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Textline - Item ID
                else if (l1.name == "Textline - ItemID")
                {
                    string data = "Item ID: 31186006897045" + Environment.NewLine + "Email: mtruty@olpl.org" + 
                        Environment.NewLine + "Phone number: 7085555555" + Environment.NewLine + "User name: Truty, Marcin" +
                        Environment.NewLine + "Pickup By: 9/20/2015";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Item ID: "))
                            {
                                text = data1 + " " + line.Replace("Item ID: ", "");
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Textline - Item Title
                else if (l1.name == "Textline - ItemTitle")
                {
                    string data = "Transit To: OLS" + Environment.NewLine + "Item ID: 31186006897045" + Environment.NewLine+ "Title: Test title fshfdljhs kjhgfdksjhg kjfhg kdlskfdjh gklsjdhkfdg hlksjdhkdfsjh glk";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Title: "))
                            {
                                text = data1 + " " + line.Replace("Title: ", "");
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline intransit library name render problem " + l2.data); }
                }
                #endregion
                #region Textline - ToLibraryName
                else if (l1.name == "Textline - ToLibraryName")
                {
                    string data = "Transit To: OLS" + Environment.NewLine + "Item ID: 31186006897045" + Environment.NewLine + "Title: Test title fshfdljhs kjhgfdksjhg kjfhg kdlskfdjh gklsjdhkfdg hlksjdhkdfsjh glk";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Transit To: "))
                            {
                                text = data1 + " " + controlFunctions.readTransitTo(mS, line.Replace("Transit to: ", "").Trim(), "name");
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline intransit library name render problem " + l2.data); }
                }
                #endregion
                #region Textline - ILLLibsCeckout
                else if (l1.name == "Textline - ILLLibsCeckout")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = data1 + " " + "06/12/2016";
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline illlibs library name render problem " + l2.data); }
                }
                #endregion
                #region Textline - ToLibraryCity
                else if (l1.name == "Textline - ToLibraryCity")
                {
                    string data = "Transit To: OLS" + Environment.NewLine + "Item ID: 31186006897045" + Environment.NewLine + "Title: Test title fshfdljhs kjhgfdksjhg kjfhg kdlskfdjh gklsjdhkfdg hlksjdhkdfsjh glk";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Transit To: "))
                            {
                                text = data1 + " " + controlFunctions.readTransitTo(mS, line.Replace("Transit to: ", "").Trim(), "city");
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline in transit city name render problem " + l2.data); }
                }
                #endregion
                #region Textline - SerialTitle
                else if (l1.name == "Textline - SerialTitle")
                {
                    string data = "Title: National Geographic [2005 to ]" + Environment.NewLine + "Enumeration:V.228 NO.4" + Environment.NewLine + "Chronology:OCT 2015" + Environment.NewLine + "RouteTo: Truty, Marcin";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Title: "))
                            {
                                text = data1 + " " + line.Replace("Title: ",string.Empty);
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline in transit city name render problem " + l2.data); }
                }
                #endregion
                #region Textline - SerialEnumeration
                else if (l1.name == "Textline - SerialEnumeration")
                {
                    string data = "Title: National Geographic [2005 to ]" + Environment.NewLine + "Enumeration:V.228 NO.4" + Environment.NewLine + "Chronology:OCT 2015" + Environment.NewLine + "RouteTo: Truty, Marcin";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Enumeration:"))
                            {
                                text = data1 + " " + line.Replace("Enumeration:", string.Empty);
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline in transit city name render problem " + l2.data); }
                }
                #endregion
                #region Textline - SerialChronology
                else if (l1.name == "Textline - SerialChronology")
                {
                    string data = "Title: National Geographic [2005 to ]" + Environment.NewLine + "Enumeration:V.228 NO.4" + Environment.NewLine + "Chronology:OCT 2015" + Environment.NewLine + "RouteTo: Truty, Marcin";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Chronology:"))
                            {
                                text = data1 + " " + line.Replace("Chronology:", string.Empty);
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline in transit city name render problem " + l2.data); }
                }
                #endregion
                #region Textline - SerialRouteTO
                else if (l1.name == "Textline - SerialRouteTO")
                {
                    string data = "Title: National Geographic [2005 to ]" + Environment.NewLine + "Enumeration:V.228 NO.4" + Environment.NewLine + "Chronology:OCT 2015" + Environment.NewLine + "RouteTo: Truty, Marcin"
                        + Environment.NewLine + "RouteTo: Truty2, Marcin2";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("RouteTo: "))
                            {
                                text = text + data1 + " " + line.Replace("RouteTo: ", string.Empty)+ Environment.NewLine;
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline in transit city name render problem " + l2.data); }
                }
                #endregion
                #region Textline - UserPhoneNumber
                else if (l1.name == "Textline - UserPhoneNumber")
                {
                    bool skip = false;
                    string data = "Item ID: 31186006897045" + Environment.NewLine + "Email1: mtruty@olpl.org" +
                        Environment.NewLine + "Phone number: 7085555555" + Environment.NewLine + "User name: Truty, Marcin" +
                        Environment.NewLine + "Pickup By: 9/20/2015";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Email: "))
                            {
                                skip = true;
                            }
                            else if (line.Contains("Phone number: "))
                            {
                                text = data1 + " " + line.Replace("Phone number: ", "");
                            }
                        }
                        if (!skip)
                        {
                            y += l1.spaceTop;
                            if (l1.align == "Right")
                            {
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                            }
                            else if (l1.align == "Left")
                            {
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                            }
                            else if (l1.align == "Center")
                            {
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                            }
                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Textline - VerticalLastName
                else if (l1.name == "Textline - VerticalLastName")
                {
                    string data = "Item ID: 31186006897045" + Environment.NewLine + "Email1: mtruty@olpl.org" +
                        Environment.NewLine + "Phone number: 7085555555" + Environment.NewLine + "User name: Truty, Marcin" +
                        Environment.NewLine + "Pickup By: 9/20/2015";
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in data.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("User name: "))
                            {
                                if (line.Contains(","))
                                {
                                    string[] strun = line.Split(',');
                                    text = strun[0].Replace("User name: ", "");
                                }
                                else
                                {
                                    text = line.Replace("User name: ", "");
                                }

                            }
                        }
                        y += l1.spaceTop;
                        int count = 0;
                        if (l1.align == "Right")
                        {
                            foreach (char c in text)
                            {
                                if (count < int.Parse(l1.data))
                                {
                                    e.Graphics.DrawString(c.ToString(), font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                                    y += e.Graphics.MeasureString(c.ToString(), font, new RectangleF(x, y, width, height).Size).Height;
                                }
                                count++;
                            }
                        }
                        
                        else if (l1.align == "Left")
                        {
                            foreach (char c in text)
                            {
                                if (count < int.Parse(l1.data))
                                {
                                    e.Graphics.DrawString(c.ToString(), font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                                    y += e.Graphics.MeasureString(c.ToString(), font, new RectangleF(x, y, width, height).Size).Height;
                                }
                                count++;
                            }
                        }
                        else if (l1.align == "Center")
                        {
                            foreach (char c in text)
                            {
                                if (count < int.Parse(l1.data))
                                {
                                    e.Graphics.DrawString(c.ToString(), font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                                    y += e.Graphics.MeasureString(c.ToString(), font, new RectangleF(x, y, width, height).Size).Height;
                                }
                                count++;
                            }
                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Textline - UserRecordName
                else if (l1.name == "Textline - UserRecordName")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data);
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in controlFunctions.proccessUserRecord(data,"basic").Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("NAME:"))
                            {
                                text = data1 + " " + line.Split(new string[] { "NAME: " }, StringSplitOptions.None)[1].Split(new string[] { " PRIVILEGE " }, StringSplitOptions.None)[0];
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Textline - UserRecordID
                else if (l1.name == "Textline - UserRecordID")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data);
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in controlFunctions.proccessUserRecord(data, "basic").Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("USER ID:"))
                            {
                                text = data1 + " " + line.Split(new string[] { "USER ID: " }, StringSplitOptions.None)[1].Split(new string[] { " PRIVILEGE " }, StringSplitOptions.None)[0];
                            }
                        }
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Block - UserRecordAddress
                else if (l1.name == "Block - UserRecordAddress")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data).Replace("\r","");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(data, "address");
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Block - UserRecordDemographic
                else if (l1.name == "Block - UserRecordDemographic")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(data, "demoinfo");
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Block - UserRecordExtended
                else if (l1.name == "Block - UserRecordExtended")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(data, "extinfo");
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Block - UserRecordCirculation
                else if (l1.name == "Block - UserRecordCirculation")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(data, "circinfo");
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Block - UserRecordCheckouts
                else if (l1.name == "Block - UserRecordCheckouts")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(data, "checkouts");
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Block - UserRecordBills
                else if (l1.name == "Block - UserRecordBills")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(data, "bills");
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
                #region Block - UserRecordHolds
                else if (l1.name == "Block - UserRecordHolds")
                {
                    string data = File.ReadAllText("UserRecord.txt");
                    data = controlFunctions.HTMLtoTXTUser(data).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(data, "holds");
                        y += l1.spaceTop;
                        if (l1.align == "Right")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                        }
                        else if (l1.align == "Left")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        else if (l1.align == "Center")
                        {
                            e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                            y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;

                        }
                        // y += e.Graphics.MeasureString(text, font,width).Height;
                    }
                    catch (Exception e1) { fc.log.WriteLine(e1.Message + " textline ItemID render problem " + l2.data); }
                }
                #endregion
            }
        }
    }
}