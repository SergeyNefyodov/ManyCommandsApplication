using System.Windows;
using ExtensibleStorageExample.Models;

namespace ExtensibleStorageExample.Views
{
    public partial class MapFieldEditor : Window
    {
        public MapFieldEditor(MapFieldDescriptor descriptor)
        {
            DataContext = descriptor;
            InitializeComponent();
        }
    }
}