using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProject.Core
{
    public class RevitShell
    {
        public static void SayHello ()
        {
            TaskDialog.Show("Hello world!", $"Hello world in {Context.Document.Title}");
        }
    }
}
