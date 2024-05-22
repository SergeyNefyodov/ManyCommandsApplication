using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using CommonProject.Core;
using ExternalCommandOne.ViewModels;
using ExternalCommandOne.Views;
using Nice3point.Revit.Toolkit.External;

namespace ExternalCommandOne.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ExternalCommandOneCommand : ExternalCommand
    {
        public override void Execute()
        {
            RevitShell.SayHello();
            TaskDialog.Show("Info", $"{this.GetType().FullName} is executing");
            //var viewModel = new ExternalCommandOneViewModel();
            //var view = new ExternalCommandOneView(viewModel);
            //view.ShowDialog();
        }
    }
}