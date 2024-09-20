using Autodesk.Revit.UI.Selection;
using ImageExportExample.Models;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;

namespace ImageExportExample.ViewModels
{
    public sealed partial class ImageExportExampleViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<ElementDescriptor> _elements = [];

        [RelayCommand]
        private void AddImage()
        {
            RaiseHideRequest();
            var reference = Context.UiDocument.Selection.PickObject(ObjectType.Element);
            var element = Context.Document.GetElement(reference);

            var bitmap = ExportImage(element);
            var descriptor = new ElementDescriptor
            {
                Image = bitmap,
                Name = element.Name,
                Id = element.Id.ToString()
            };

            Elements.Add(descriptor);
            RaiseShowRequest();
        }
        public static Bitmap ExportImage(Element element)
        {
            var viewType = Context.Document.EnumerateTypes<ViewFamilyType>()
                .FirstOrDefault(view => view.ViewFamily == ViewFamily.ThreeDimensional);
            using var transaction = new Transaction(Context.Document, "New image");
            transaction.Start();
            var newView = View3D.CreateIsometric(Context.Document, viewType.Id);
            newView.IsolateElementTemporary(element.Id);
            newView.ConvertTemporaryHideIsolateToPermanent();
            newView.DisplayStyle = DisplayStyle.Realistic;

            var filePath = Path.Combine(Path.GetTempPath(), $"{element.Id}.png");

            var options = new ImageExportOptions
            {
                PixelSize = 1200,
                ExportRange = ExportRange.SetOfViews,
                FilePath = filePath,
                ShadowViewsFileType = ImageFileType.PNG
            };
            options.SetViewsAndSheets([newView.Id]);
            Context.Document.ExportImage(options);
            transaction.RollBack();

            var directoryPath = Path.GetDirectoryName(filePath);
            var files = Directory.GetFiles(directoryPath);
            foreach ( var file in files )
            {
                if (file.Contains($"{element.Id}") && file.EndsWith(".png"))
                {
                    return new Bitmap(file);
                }
            }
            return null;
        }
        public event EventHandler HideRequest;

        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ShowRequest;

        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}