using ExternalCommandOne.ViewModels;

namespace ExternalCommandOne.Views
{
    public sealed partial class ExternalCommandOneView
    {
        public ExternalCommandOneView(ExternalCommandOneViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}