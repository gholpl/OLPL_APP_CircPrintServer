using APP_CircPrintServer.Models;
using DLL_CircPrintServer.Classes;
using DLL_CircPrintServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Client.Result;
using ZXing.Common;
using ZXing.Rendering;

namespace APP_CircPrintServer.Functions
{
    class controlPrints
    {
        private static Type Renderer { get; set; }
        static modelSettings mS2 = new modelSettings();
        static string datastr = "";
        static modelTemplate tc = new modelTemplate();
        public static void printStart(modelSettings mS, string data, string type, Form1 frmMain1)
        {
            //Debugger.Launch();
            mS2 = mS;
            datastr = data;
            PrintDocument docToPrint = new PrintDocument();
            docToPrint.DocumentName = "Customer Receipt";
            docToPrint.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(setupPage);
            PrinterSettings ps = new PrinterSettings();
            if (type == "checkout")
            {
                tc = controlSettings.readTemplateFile(controlFunctions.fixVars(mS.tempLocation + mS.tempCirc + ".template"), mS, "Circ");
                mS.printerCirc = controlValidation.verifyPrinter(mS.printerCirc, "printerCheckout", mS);
                ps.PrinterName = mS.printerCirc;
            }
            else if (type == "holdsP1")
            {

                tc = controlSettings.readTemplateFile(controlFunctions.fixVars(mS.tempLocation + mS.tempHoldsP1 + ".template"), mS, "HoldsPage1");
                mS.printerHolds = controlValidation.verifyPrinter(mS.printerHolds, "printerHolds", mS);
                ps.PrinterName = mS.printerHolds;
            }
            else if (type == "holdsP2")
            {
                tc = controlSettings.readTemplateFile(controlFunctions.fixVars(mS.tempLocation + mS.tempHoldsP2 + ".template"), mS, "HoldsPage2");
                mS.printerHolds = controlValidation.verifyPrinter(mS.printerHolds, "printerHolds", mS);
                ps.PrinterName = mS.printerHolds;
            }
            else if (type == "fine")
            {
                tc = controlSettings.readTemplateFile(controlFunctions.fixVars(mS.tempLocation + mS.tempFine + ".template"), mS, "Financial");
                mS.printerFine = controlValidation.verifyPrinter(mS.printerFine, "printerFine", mS);
                ps.PrinterName = mS.printerFine;
            }
            else if (type == "serialRoute")
            {
                tc = controlSettings.readTemplateFile(controlFunctions.fixVars(mS.tempLocation + mS.tempSerialRoute + ".template"), mS, "SerialRoute");
                mS.printerSerialRoute = controlValidation.verifyPrinter(mS.printerSerialRoute, "printerSerialRoute", mS);
                ps.PrinterName = mS.printerSerialRoute;
            }
            else if (type == "UserRecord")
            {
                tc = controlSettings.readTemplateFile(controlFunctions.fixVars(mS.tempLocation + mS.tempUserRecord + ".template"), mS, "UserRecord");
                mS.printerUserRecord = controlValidation.verifyPrinter(mS.printerUserRecord, "printerUserRecord", mS);
                ps.PrinterName = mS.printerUserRecord;
            }
            else if (type == "intransit")
            {
                tc = controlSettings.readTemplateFile(controlFunctions.fixVars(mS.tempLocation + mS.tempInTransit + ".template"), mS, "InTransit");
                mS.printerInTransit = controlValidation.verifyPrinter(mS.printerInTransit, "printerInTransit", mS);
                ps.PrinterName = mS.printerInTransit;
            }
            docToPrint.PrintController = new StandardPrintController();
            docToPrint.PrinterSettings = ps;
            if (mS.switchAdminMode == "0")
            {
                docToPrint.Print();
                File.Delete(controlFunctions.fixVars(mS.fileTempData));
            }
            else
            {
                PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();
                printPrvDlg.Document = docToPrint;
                printPrvDlg.Width = 1200;
                printPrvDlg.Height = 800;
                printPrvDlg.ShowDialog();
            }
        }
        public static void setupPage(object sender, PrintPageEventArgs e)
        {
            Renderer = typeof(BitmapRenderer);
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
            foreach (modelElement l1 in tc.element)
            {
                modelElement l2 = l1;
                data1 = controlFunctions.fixVarsElement(l2, mS2);
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
                    catch (Exception e1) { MessageBox.Show(e1.Message); controlFunctions.fileWriteLog(e1.Message + " Logo import problem " + l2.data, mS2); }
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline render problem " + l2.data, mS2); }
                }
                #endregion
                #region Checkout Block
                else if (l1.name == "Checkout Block")
                {
                    string[] arrayFileRead = datastr.Split(Environment.NewLine.ToCharArray());
                    List<modelCheckout> listCheckout = listControl.proccessCheckout(mS2, arrayFileRead);
                    try
                    {
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        y += l1.spaceTop;
                        foreach (modelCheckout cC in listCheckout)
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline render problem " + l2.data, mS2); }
                }
                #endregion
                #region Payment Block
                else if (l1.name == "Payment Block")
                {
                    try
                    {
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        y += l1.spaceTop;
                        foreach (string cC in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            string text = "";
                            if (l1.align == "Right")
                            {
                                text = cC;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                            else if (l1.align == "Left")
                            {
                                text = cC;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                            else if (l1.align == "Center")
                            {
                                text = cC;
                                e.Graphics.DrawString(text, font, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                                y += e.Graphics.MeasureString(text, font, new RectangleF(x, y, width, height).Size).Height;
                                y += int.Parse(l1.data);
                            }
                        }
                    }
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " Payment block render problem " + l2.data, mS2); }
                }
                #endregion
                #region Barcode - ToLibrary
                else if (l1.name == "Barcode - ToLibrary")
                {
                    Image toBcodeImage;
                    try
                    {
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.ToUpper().Contains("TRANSIT TO: "))
                            {
                                var barcodeWriter = new BarcodeWriter
                                {
                                    Format = BarcodeFormat.CODE_39,
                                    Options = new EncodingOptions
                                    {
                                        Height = 80,
                                        Margin = 0,
                                        PureBarcode = false,
                                        Width = 270
                                    },
                                    Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(Renderer)
                                };
                                toBcodeImage = barcodeWriter.Write(controlFunctions.readTransitTo(mS2, line.ToUpper().Replace("TRANSIT TO: ", "").Trim(), "barcode"));
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " Barcode tolibrary proccessing problem", mS2); }
                }
                #endregion
                #region Barcode - ItemID
                else if (l1.name == "Barcode - ItemID")
                {
                    Image toBcodeImage;
                    try
                    {
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.ToUpper().Contains("ITEM ID: "))
                            {
                                var barcodeWriter = new BarcodeWriter
                                {
                                    Format = BarcodeFormat.CODE_39,
                                    Options = new EncodingOptions
                                    {
                                        Height = 80,
                                        Margin = 0,
                                        PureBarcode = false,
                                        Width = 270
                                    },
                                    Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(Renderer)
                                };
                                toBcodeImage = barcodeWriter.Write(line.ToUpper().Replace("ITEM ID: ", ""));
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " Barcode tolibrary proccessing problem", mS2); }
                }
                #endregion
                #region Textline - FirstName
                else if (l1.name == "Textline - FirstName")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
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
                        if (int.TryParse(l1.data, out size))
                        {
                            string newText = "";
                            int count = 0;
                            foreach (char c in text)
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - From Library Name
                else if (l1.name == "Textline - FromLibraryName")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Transit Slip Start"))
                            {
                                text = data1 + " " + controlFunctions.readTransitTo(mS2, line.Replace(" Transit Slip Start", ""), "name");
                                //controlFunctions.fileWriteLog("Test1 ---- " + FileControl.readTransitTo(mS2, line.Replace(" Transit Slip Start", ""), "name"), mS2);
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline intransit library name render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - HoldExpire
                else if (l1.name == "Textline - HoldExpire")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Pickup By: "))
                            {
                                text = line.Replace("Pickup By: ", "");
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - Item ID
                else if (l1.name == "Textline - ItemID")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - Item Title
                else if (l1.name == "Textline - ItemTitle")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline intransit library name render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - ToLibraryName
                else if (l1.name == "Textline - ToLibraryName")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {

                            if (line.Contains("Transit to: "))
                            {
                                // Debugger.Launch();
                                //controlFunctions.fileWriteLog("TT" + FileControl.readTransitTo(mS2, line.Replace("Transit to: ", ""),"name"), mS2);
                                if (line.ToUpper().Contains("ILL_LIBS"))
                                {
                                    foreach (string line2 in datastr.Split(Environment.NewLine.ToCharArray()))
                                    {

                                        if (line2.ToUpper().Contains("USER NAME:")) { text = data1 + line2.Split(',')[0].Replace("User name: ", "").Trim(); }
                                    }
                                }
                                else
                                {
                                    string libName = controlFunctions.readTransitTo(mS2, line.Replace("Transit to: ", "").Trim(), "name");
                                    if (libName.ToUpper().Contains("NONE"))
                                    {
                                        text = data1 + " " + line.Replace("Transit to: ", "").Trim();
                                    }
                                    else
                                    {
                                        text = data1 + " " + libName;
                                    }
                                }
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline intransit library name render problem " + l2.data, mS2); }
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
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {

                            if (line.Contains("Transit to: "))
                            {
                                //Debugger.Launch();
                                //controlFunctions.fileWriteLog("TT" + FileControl.readTransitTo(mS2, line.Replace("Transit to: ", ""),"name"), mS2);
                                if (line.ToUpper().Contains("ILL_LIBS"))
                                {
                                    string idUser = "";
                                    string idItem = "";
                                    foreach (string line2 in datastr.Split(Environment.NewLine.ToCharArray()))
                                    {
                                        if (line2.ToUpper().Contains("USER ID")) { idUser = line2.Split(',')[0].Replace("User ID ", "").Trim(); }
                                        if (line2.ToUpper().Contains("ITEM ID")) { idItem = line2.Split(',')[0].Replace("Item ID: ", "").Trim(); }
                                    }
                                    controlSIP cSIP = new controlSIP();
                                    string dataDue = cSIP.checkoutItem(idUser, idItem, mS2);
                                    if (dataDue.ToUpper().Contains("NONE")) { MessageBox.Show("Item not Checked out.  Please check out."); }
                                    else { text = data1 + " " + dataDue; }
                                }
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline intransit library ILLIBS Checkout " + l2.data, mS2); }
                }
                #endregion
                #region Textline - ToLibraryCity
                else if (l1.name == "Textline - ToLibraryCity")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Transit to: "))
                            {
                                text = data1 + " " + controlFunctions.readTransitTo(mS2, line.Replace("Transit to: ", "").Trim(), "city");
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline in transit city name render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - SerialTitle
                else if (l1.name == "Textline - SerialTitle")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Title: "))
                            {
                                text = data1 + " " + line.Replace("Title: ", string.Empty);
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline serial title render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - SerialEnumeration
                else if (l1.name == "Textline - SerialEnumeration")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline in transit city name render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - SerialChronology
                else if (l1.name == "Textline - SerialChronology")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline in transit city name render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - SerialRouteTO
                else if (l1.name == "Textline - SerialRouteTO")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("RouteTo: "))
                            {
                                text = text + data1 + " " + line.Replace("RouteTo: ", string.Empty) + Environment.NewLine;
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline in transit city name render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - UserPhoneNumber
                else if (l1.name == "Textline - UserPhoneNumber")
                {
                    bool skip = false;
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("Email: "))
                            {
                                skip = true;
                            }
                            else if (line.Contains("Phone number: "))
                            {
                                text = data1 + " " + line.Replace("Phone number: ", "");
                                if (mS2.notificationHoldsCallSwitch == "1")
                                {
                                    string title = "";
                                    foreach (string line3 in datastr.Split(Environment.NewLine.ToCharArray()))
                                    {
                                        if (line3.Contains("Title: "))
                                        {
                                            title = line3.Replace("Title: ", "");
                                        }
                                    }
                                    //callControl.callPatron(mS2, line.Replace("Phone number: ", ""),title);
                                }
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - VerticalLastName
                else if (l1.name == "Textline - VerticalLastName")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in datastr.Split(Environment.NewLine.ToCharArray()))
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - UserRecordName
                else if (l1.name == "Textline - UserRecordName")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in controlFunctions.proccessUserRecord(datastr, "basic").Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("NAME:"))
                            {
                                text = data1 + " " + line.Split(new string[] { "NAME: " }, StringSplitOptions.None)[1].Split(new string[] { " PRIVILEGE " }, StringSplitOptions.None)[0];
                                if (text.Contains("USER ID:")) { text = text.Split(new string[] { "USER ID:" }, StringSplitOptions.None)[0].Replace("</td>", String.Empty); }
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Textline - UserRecordID
                else if (l1.name == "Textline - UserRecordID")
                {
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in controlFunctions.proccessUserRecord(datastr, "basic").Split(Environment.NewLine.ToCharArray()))
                        {
                            if (line.Contains("USER ID:"))
                            {
                                text = data1 + " " + line.Split(new string[] { "USER ID: " }, StringSplitOptions.None)[1].Split(new string[] { " PRIVILEGE " }, StringSplitOptions.None)[0].Replace("</td>", String.Empty);
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Block - UserRecordAddress
                else if (l1.name == "Block - UserRecordAddress")
                {
                    try
                    {
                        datastr = controlFunctions.HTMLtoTXTUser(datastr).Replace("\r", "");
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(datastr, "address");
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Block - UserRecordDemographic
                else if (l1.name == "Block - UserRecordDemographic")
                {
                    datastr = controlFunctions.HTMLtoTXTUser(datastr).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in controlFunctions.proccessUserRecord(datastr, "demoinfo").Split(new string[] { "   " }, StringSplitOptions.None))
                        {
                            text = text + line.TrimStart() + Environment.NewLine;
                        }
                        //text = FileControl.GetUserRecord(datastr, "demoinfo");
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Block - UserRecordExtended
                else if (l1.name == "Block - UserRecordExtended")
                {
                    datastr = controlFunctions.HTMLtoTXTUser(datastr).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(datastr, "extinfo");
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Block - UserRecordCirculation
                else if (l1.name == "Block - UserRecordCirculation")
                {
                    datastr = controlFunctions.HTMLtoTXTUser(datastr).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        text = controlFunctions.proccessUserRecord(datastr, "circinfo");
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Block - UserRecordCheckouts
                else if (l1.name == "Block - UserRecordCheckouts")
                {
                    datastr = controlFunctions.HTMLtoTXTUser(datastr).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in controlFunctions.proccessUserRecord(datastr, "checkouts").Split(Environment.NewLine.ToCharArray()))
                        {
                            string line2 = line.Replace(Environment.NewLine, "");
                            //controlFunctions.fileWriteLog(line, mS2);
                            if (line.Contains("ITEM ID: "))
                            {
                                foreach (string innerline in line2.Split(new string[] { "   " }, StringSplitOptions.None))
                                {
                                    if (!innerline.Contains("") || innerline != String.Empty)
                                    {
                                        text = text + innerline.TrimStart() + Environment.NewLine;
                                        if (innerline.Contains("STATUS")) { text = text + Environment.NewLine; }
                                    }
                                }
                            }
                            else
                            {
                                if (!line2.Contains("") || line2 != String.Empty)
                                {

                                    text = text + line2.TrimStart().Replace(Environment.NewLine, "") + Environment.NewLine;
                                }
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Block - UserRecordBills
                else if (l1.name == "Block - UserRecordBills")
                {
                    datastr = controlFunctions.HTMLtoTXTUser(datastr).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in controlFunctions.proccessUserRecord(datastr, "bills").Split(Environment.NewLine.ToCharArray()))
                        {
                            string line2 = line.Replace(Environment.NewLine, "");
                            //controlFunctions.fileWriteLog(line, mS2);
                            if (line.Contains("ITEM ID: "))
                            {
                                foreach (string innerline in line2.Split(new string[] { "   " }, StringSplitOptions.None))
                                {
                                    if (!innerline.Contains("") || innerline != String.Empty)
                                    {
                                        text = text + innerline.TrimStart() + Environment.NewLine;
                                        if (innerline.Contains("TRANSACTION DATE")) { text = text + Environment.NewLine; }
                                    }
                                }
                            }
                            else
                            {
                                if (!line2.Contains("") || line2 != String.Empty)
                                {

                                    text = text + line2.TrimStart() + Environment.NewLine;
                                }
                            }
                        }
                        //text = FileControl.GetUserRecord(datastr, "bills");
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion
                #region Block - UserRecordHolds
                else if (l1.name == "Block - UserRecordHolds")
                {
                    datastr = controlFunctions.HTMLtoTXTUser(datastr).Replace("\r", "");
                    try
                    {
                        string text = "";
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        Font font = l1.fontName;
                        foreach (string line in controlFunctions.proccessUserRecord(datastr, "holds").Split(Environment.NewLine.ToCharArray()))
                        {
                            string line2 = line.Replace(Environment.NewLine, "");
                            //controlFunctions.fileWriteLog(line, mS2);
                            if (line.Contains("ITEM ID: "))
                            {
                                foreach (string innerline in line2.Split(new string[] { "   " }, StringSplitOptions.None))
                                {
                                    if (!innerline.Contains("") || innerline != String.Empty)
                                    {
                                        text = text + innerline.TrimStart() + Environment.NewLine;
                                        if (innerline.Contains("EXPIRES")) { text = text + Environment.NewLine; }
                                    }
                                }
                            }
                            else
                            {
                                if (!line2.Contains("") || line2 != String.Empty)
                                {

                                    text = text + line2.TrimStart() + Environment.NewLine;
                                }
                            }
                        }
                        //text = FileControl.GetUserRecord(datastr, "holds");
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
                    catch (Exception e1) { controlFunctions.fileWriteLog(e1.Message + " textline ItemID render problem " + l2.data, mS2); }
                }
                #endregion

            }
        }
        
    }
}
