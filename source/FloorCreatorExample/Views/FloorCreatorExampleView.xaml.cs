using FloorCreatorExample.ViewModels;

namespace FloorCreatorExample.Views
{
    public sealed partial class FloorCreatorExampleView
    {
        public FloorCreatorExampleView(FloorCreatorExampleViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}