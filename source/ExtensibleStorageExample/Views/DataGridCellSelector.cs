using System.Windows;
using System.Windows.Controls;
using ExtensibleStorageExample.Models;

namespace ExtensibleStorageExample.Views
{
    public class DataGridCellSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is null) return null;
            var presenter = (FrameworkElement)container;

            var templateName = item switch
            {
                SimpleFieldDescriptor => "SimpleCellTemplate",
                ArrayFieldDescriptor => "ArrayCellTemplate",
                MapFieldDescriptor => "MapCellTemplate",
                _ => "SimpleCellTemplate"
            };

            return (DataTemplate)presenter.FindResource(templateName);
        }
    }
}