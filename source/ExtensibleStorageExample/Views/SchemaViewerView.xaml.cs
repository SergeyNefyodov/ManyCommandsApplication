using ExtensibleStorageExample.ViewModels;

namespace ExtensibleStorageExample.Views
{
    public sealed partial class SchemaViewerView
    {
        public SchemaViewerView(SchemaViewerViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}