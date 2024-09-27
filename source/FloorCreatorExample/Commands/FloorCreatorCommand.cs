using Autodesk.Revit.Attributes;
using FloorCreatorExample.ViewModels;
using FloorCreatorExample.Views;
using Nice3point.Revit.Toolkit.External;

namespace FloorCreatorExample.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class FloorCreatorCommand : ExternalCommand
    {
        public override void Execute()
        {
            var viewModel = new FloorCreatorExampleViewModel();
            var view = new FloorCreatorExampleView(viewModel);
            viewModel.CloseRequest += (s, e) => view.Close();
            viewModel.HideRequest += (s, e) => view.Hide();
            viewModel.ShowRequest += (s, e) => view.ShowDialog();
            view.ShowDialog();
        }
    }
}