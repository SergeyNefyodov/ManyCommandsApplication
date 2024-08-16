namespace ExtensibleStorageExample.Models;

public partial class ArrayValue : ObservableObject
{
    [ObservableProperty] private object _activeValue;
    [ObservableProperty] private Type _type;
}