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
                    DescribeSimpleField(entity, valueType, field);
                }
                else if (field.ContainerType == ContainerType.Array)
                {
                    DescribeArrayField(entity, valueType, field);
                }
                else if (field.ContainerType == ContainerType.Map)
                {
                    DescribeMapField(entity, field, valueType);
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
                    CreateEmptySimpleField(field, valueType);
                }
                else if (field.ContainerType == ContainerType.Array)
                {
                    CreateEmptyArrayField(field, valueType);
                }
                else if (field.ContainerType == ContainerType.Map)
                {
                    CreateEmptyMapField(field, valueType);
                }
            }
        }

        public string EntityName { get; set; }
        public Guid Guid { get; set; }

        public List<FieldDescriptor> Fields { get; set; } = [];

        private void DescribeMapField(Entity entity, Field field, Type valueType)
        {
            var info = typeof(Entity).GetMethod("Get", [typeof(string)]);
            var genericDictType = typeof(IDictionary<,>).MakeGenericType(field.KeyType, valueType);
            var getMethod = info!.MakeGenericMethod(genericDictType);
            var value = (IDictionary)getMethod.Invoke(entity, [field.FieldName]);

            var values = new ObservableCollection<MapValue>();
            foreach (var key in value!.Keys)
            {
                values.Add(new MapValue
                {
                    Type = valueType,
                    ActiveValue = value[key]!,
                    ActiveKey = key
                });
            }

            Fields.Add(new MapFieldDescriptor
            {
                Name = field.FieldName,
                FieldType = valueType,
                Values = values
            });
        }

        private void DescribeArrayField(Entity entity, Type valueType, Field field)
        {
            var info = typeof(Entity).GetMethod("Get", [typeof(string)]);
            var genericListType = typeof(IList<>).MakeGenericType(valueType);
            var getMethod = info!.MakeGenericMethod(genericListType);
            var value = (ICollection)getMethod.Invoke(entity, [field.FieldName]);
            var values = new ObservableCollection<ArrayValue>();
            foreach (var obj in value!)
            {
                values.Add(new ArrayValue
                {
                    Type = valueType,
                    ActiveValue = obj
                });
            }

            Fields.Add(new ArrayFieldDescriptor
            {
                Name = field.FieldName,
                FieldType = valueType,
                Values = values
            });
        }

        private void DescribeSimpleField(Entity entity, Type valueType, Field field)
        {
            var info = typeof(Entity).GetMethod("Get", [typeof(string)]);
            var getMethod = info!.MakeGenericMethod(valueType);
            var value = getMethod.Invoke(entity, [field.FieldName]);
            Fields.Add(new SimpleFieldDescriptor
            {
                Name = field.FieldName,
                FieldType = valueType,
                Value = value
            });
        }

        private void CreateEmptyMapField(Field field, Type valueType)
        {
            Fields.Add(new MapFieldDescriptor
            {
                Name = field.FieldName,
                FieldType = valueType,
                KeyType = field.KeyType,
                Values = []
            });
        }

        private void CreateEmptyArrayField(Field field, Type valueType)
        {
            Fields.Add(new ArrayFieldDescriptor
            {
                Name = field.FieldName,
                FieldType = valueType,
                Values = []
            });
        }

        private void CreateEmptySimpleField(Field field, Type valueType)
        {
            Fields.Add(new SimpleFieldDescriptor
            {
                Name = field.FieldName,
                FieldType = valueType,
            });
        }
    }
}