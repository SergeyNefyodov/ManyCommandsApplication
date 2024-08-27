using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageExportExample.Models
{
    public class ElementDescriptor
    {
        public Bitmap Image { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
