using ExtensibleStorageExample.ViewModels;

namespace ExtensibleStorageExample.Views
{
    public sealed partial class SchemaCreatorView
    {
        public SchemaCreatorView(SchemaCreatorViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}