using ExtensibleStorageExample.ViewModels;

namespace ExtensibleStorageExample.Views
{
    public sealed partial class ExtensibleStorageExampleView
    {
        public ExtensibleStorageExampleView(ExtensibleStorageExampleViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}