using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using CommonProject.Core;
using ExtensibleStorageExample.ViewModels;
using ExtensibleStorageExample.Views;
using Nice3point.Revit.Toolkit.External;

namespace ExtensibleStorageExample.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ExtensibleStorageExampleCommand : ExternalCommand
    {
        public override void Execute()
        {
            RevitShell.SayHello();
            TaskDialog.Show("Info", $"{this.GetType().FullName} is executing");
            //var viewModel = new ExtensibleStorageExampleViewModel();
            //var view = new ExtensibleStorageExampleView(viewModel);
            //view.ShowDialog();
        }
    }
}