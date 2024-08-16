using System.Windows;
using ExtensibleStorageExample.ViewModels;

namespace ExtensibleStorageExample.Views
{
    public partial class AddFieldView : Window
    {
        public AddFieldView(AddFieldViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}