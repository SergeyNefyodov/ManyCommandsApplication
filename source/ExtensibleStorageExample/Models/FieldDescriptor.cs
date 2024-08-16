namespace ExtensibleStorageExample.Models
{
    public class FieldDescriptor : ObservableObject
    {
        public string Name { get; set; }
        public Type FieldType { get; set; }
        public ForgeTypeId Unit { get; set; }
    }
}