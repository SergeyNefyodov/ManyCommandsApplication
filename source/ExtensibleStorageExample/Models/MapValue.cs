namespace ExtensibleStorageExample.Models;

public partial class MapValue : ObservableObject
{
    [ObservableProperty] private object _activeKey;
    [ObservableProperty] private object _activeValue;
    [ObservableProperty] private Type _type;
}