namespace ExtensibleStorageExample.Models
{
    public class SimpleFieldDescriptor : FieldDescriptor
    {
        private object _value;

        public object Value
        {
            get => _value;
            set
            {
                if (value.GetType() == FieldType)
                {
                    _value = value;
                }
            }
        }
    }
}