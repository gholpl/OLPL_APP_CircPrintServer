using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLPL_APP_CircPrinter_Admin
{
    public class templatesClass
    {
        public string name { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public string fileName { get; set; }
        public List<elementClass> element { get; set; }
    }
}
