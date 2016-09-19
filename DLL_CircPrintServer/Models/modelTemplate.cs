using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_CircPrintServer.Models
{
    public class modelTemplate
    {
        public string name { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public string fileName { get; set; }
        public List<modelElement> element { get; set; }
    }
}
