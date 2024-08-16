using System.Windows;
using ExtensibleStorageExample.Models;

namespace ExtensibleStorageExample.Views
{
    public partial class ArrayFieldEditor : Window
    {
        public ArrayFieldEditor(ArrayFieldDescriptor descriptor)
        {
            DataContext = descriptor;
            InitializeComponent();
        }
    }
}