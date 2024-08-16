using System.Collections.ObjectModel;

namespace ExtensibleStorageExample.Models
{
    public partial class MapFieldDescriptor : FieldDescriptor
    {
        public ObservableCollection<MapValue> Values { get; set; } = [];
        public Type KeyType { get; set; }

        [RelayCommand]
        private void DeleteValue(MapValue value)
        {
            Values.Remove(value);
        }
    }
}