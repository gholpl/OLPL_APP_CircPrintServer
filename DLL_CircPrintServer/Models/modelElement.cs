﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;

namespace DLL_CircPrintServer.Models
{
    public class modelElement
    {
        public int id { get; set; }
        public string name { get; set; }
        public string data { get; set; }
        public string dataType { get; set; }
        public Font fontName { get; set; }
        public string align { get; set; }
        public int spaceTop { get; set; }
        public int spaceBottom { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
