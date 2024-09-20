using ImageExportExample.ViewModels;

namespace ImageExportExample.Views
{
    public sealed partial class ImageExportExampleView
    {
        public ImageExportExampleView(ImageExportExampleViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}