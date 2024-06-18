using Autodesk.Revit.Attributes;
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
            var viewModel = new SchemaViewerViewModel();
            var view = new SchemaViewerView(viewModel);
            viewModel.CloseRequest += (s, e) => view.Close();
            viewModel.HideRequest += (s, e) => view.Hide();
            viewModel.ShowRequest += (s, e) => view.ShowDialog();
            view.ShowDialog();
        }
    }
}