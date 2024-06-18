using System.Collections;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace ExtensibleStorageExample.Models
{
    public partial class EntityDescriptor : ObservableObject
    {
        [ObservableProperty] bool _isExisted;
        [ObservableProperty] FieldDescriptor _selectedField;

        public EntityDescriptor(Entity entity)
        {
            EntityName = entity.Schema.SchemaName;
            Guid = entity.SchemaGUID;
            var fields = entity.Schema.ListFields();
            foreach (var field in fields)
            {
                var valueType = field.ValueType;

                if (field.ContainerType == ContainerType.Simple)
                {
                    var info = typeof(Entity).GetMethod("Get", [typeof(string)]);
                    var getMethod = info!.MakeGenericMethod(valueType);
                    var value = getMethod.Invoke(entity, new object[] { field.FieldName });
                    Fields.Add(new SimpleFieldDescriptor()
                    {
                        Name = field.FieldName,
                        FieldType = valueType,
                        Value = value
                    });
                }

                if (field.ContainerType == ContainerType.Array)
                {
                    var info = typeof(Entity).GetMethod("Get", [typeof(string)]);
                    var genericListType = typeof(IList<>).MakeGenericType(valueType);
                    var getMethod = info!.MakeGenericMethod(genericListType);
                    var value = (ICollection)getMethod.Invoke(entity, new object[] { field.FieldName });
                    var values = new ObservableCollection<ArrayValue>();
                    foreach (var obj in value)
                    {
                        values.Add(new ArrayValue()
                        {
                            Type = valueType,
                            ActiveValue = obj
                        });
                    }

                    Fields.Add(new ArrayFieldDescriptor()
                    {
                        Name = field.FieldName,
                        FieldType = valueType,
                        Values = values
                    });
                }
                else if (field.ContainerType == ContainerType.Map)
                {
                    var info = typeof(Entity).GetMethod("Get", [typeof(string)]);
                    var genericDictType = typeof(IDictionary<,>).MakeGenericType(field.KeyType, valueType);
                    var getMethod = info!.MakeGenericMethod(genericDictType);
                    var value = (IDictionary)getMethod.Invoke(entity, new object[] { field.FieldName });
                    var values = new ObservableCollection<MapValue>();
                    foreach (var obj in value.Keys)
                    {
                        values.Add(new MapValue()
                        {
                            Type = valueType,
                            ActiveValue = value[obj],
                            ActiveKey = obj
                        });
                    }

                    Fields.Add(new MapFieldDescriptor()
                    {
                        Name = field.FieldName,
                        FieldType = valueType,
                        Values = values
                    });
                }
            }

            IsExisted = true;
        }

        public EntityDescriptor(Schema schema)
        {
            EntityName = schema.SchemaName;
            Guid = schema.GUID;
            var fields = schema.ListFields();
            foreach (var field in fields)
            {
                var valueType = field.ValueType;

                if (field.ContainerType == ContainerType.Simple)
                {
                    Fields.Add(new SimpleFieldDescriptor()
                    {
                        Name = field.FieldName,
                        FieldType = valueType,
                    });
                }
                else if (field.ContainerType == ContainerType.Array)
                {
                    Fields.Add(new ArrayFieldDescriptor()
                    {
                        Name = field.FieldName,
                        FieldType = valueType,
                        Values = []
                    });
                }
                else if (field.ContainerType == ContainerType.Map)
                {
                    Fields.Add(new MapFieldDescriptor()
                    {
                        Name = field.FieldName,
                        FieldType = valueType,
                        KeyType = field.KeyType,
                        Values = []
                    });
                }
            }
        }

        public string EntityName { get; set; }
        public Guid Guid { get; set; }

        public List<FieldDescriptor> Fields { get; set; } = [];
    }
}