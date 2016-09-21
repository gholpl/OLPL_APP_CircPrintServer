using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLPL_APP_CircPrinter_Admin.Functions
{
    class PrintControl
    {
        static internal void CreatePrintPreviewControl(Form1 fc)
        {
            PrintDocument docToPrint = new PrintDocument();
            fc.ppc1.Document = docToPrint;
            // ppc1.Zoom = 0.25;
            fc.ppc1.Document.DocumentName = "Reciept";
            fc.ppc1.Document.DefaultPageSettings.PaperSize = new PaperSize("Custom", 300, 700);
            fc.ppc1.UseAntiAlias = true;
            docToPrint.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pageControl.PrintPage);
        }
    }
}
