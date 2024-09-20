using Autodesk.Revit.Attributes;
using ImageExportExample.ViewModels;
using ImageExportExample.Views;
using Nice3point.Revit.Toolkit.External;

namespace ImageExportExample.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ImageExportCommand : ExternalCommand
    {
        public override void Execute()
        {
            var viewModel = new ImageExportExampleViewModel();
            var view = new ImageExportExampleView(viewModel);
            viewModel.HideRequest += (s, e) => view.Hide();
            viewModel.ShowRequest += (s, e) => view.ShowDialog();
            view.ShowDialog();
        }
    }
}