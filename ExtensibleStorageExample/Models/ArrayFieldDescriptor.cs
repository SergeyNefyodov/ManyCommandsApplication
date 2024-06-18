using System.Collections.ObjectModel;

namespace ExtensibleStorageExample.Models
{
    public partial class ArrayFieldDescriptor : FieldDescriptor
    {
        public ObservableCollection<ArrayValue> Values { get; set; } = [];


        [RelayCommand]
        private void DeleteValue(ArrayValue value)
        {
            Values.Remove(value);
        }
    }
}