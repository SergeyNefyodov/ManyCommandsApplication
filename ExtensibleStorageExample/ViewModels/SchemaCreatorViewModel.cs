using System.Collections.ObjectModel;
using Autodesk.Revit.DB.ExtensibleStorage;
using ExtensibleStorageExample.Enums;
using ExtensibleStorageExample.Models;
using ExtensibleStorageExample.Views;

namespace ExtensibleStorageExample.ViewModels
{
    public partial class SchemaCreatorViewModel : ObservableObject
    {
        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveSchemaCommand))]
        private string _documentation;

        [ObservableProperty] private ObservableCollection<FieldDescriptor> _fields = [];
        private SchemaBuilder _schemaBuilder;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveSchemaCommand))]
        private string _schemaName;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveSchemaCommand))]
        private string _vendorId;

        public SchemaCreatorViewModel()
        {
            _schemaBuilder = new SchemaBuilder(Guid.NewGuid());
            _schemaBuilder = _schemaBuilder.SetWriteAccessLevel(AccessLevel.Public).SetReadAccessLevel(AccessLevel.Public);
        }

        private bool CanSaveSchema()
        {
            return !SchemaName.IsNullOrEmpty() && !VendorId.IsNullOrEmpty() && !Documentation.IsNullOrEmpty();
        }

        [RelayCommand(CanExecute = nameof(CanSaveSchema))]
        private void SaveSchema()
        {
            _schemaBuilder = _schemaBuilder.SetSchemaName(SchemaName).SetDocumentation(Documentation).SetVendorId(VendorId);

            foreach (FieldDescriptor field in Fields)
            {
                if (field is SimpleFieldDescriptor)
                {
                    var builder = _schemaBuilder.AddSimpleField(field.Name, field.FieldType);
                    if (builder.NeedsUnits())
                    {
                        builder.SetSpec(field.Unit);
                    }
                }
                else if (field is ArrayFieldDescriptor)
                {
                    var builder = _schemaBuilder.AddArrayField(field.Name, field.FieldType);
                    if (builder.NeedsUnits())
                    {
                        builder.SetSpec(field.Unit);
                    }
                }
                else if (field is MapFieldDescriptor mapFieldDescriptor)
                {
                    var builder = _schemaBuilder.AddMapField(mapFieldDescriptor.Name, mapFieldDescriptor.KeyType, mapFieldDescriptor.FieldType);
                    if (builder.NeedsUnits())
                    {
                        builder.SetSpec(field.Unit);
                    }
                }
            }

            _schemaBuilder.Finish();
            RaiseCloseRequest();
        }

        [RelayCommand]
        private void AddField()
        {
            var creator = new AddFieldViewModel();
            var view = new AddFieldView(creator);
            creator.CloseRequest += (s, e) => view.Close();
            view.ShowDialog();
            if (!creator.IsReady) return;

            var type = creator.FieldType switch
            {
                SupportedFieldType.Boolean => typeof(bool),
                SupportedFieldType.Byte => typeof(Byte),
                SupportedFieldType.Int16 => typeof(Int16),
                SupportedFieldType.Int32 => typeof(Int32),
                SupportedFieldType.Float => typeof(float),
                SupportedFieldType.Double => typeof(double),
                SupportedFieldType.ElementId => typeof(ElementId),
                SupportedFieldType.GUID => typeof(Guid),
                SupportedFieldType.String => typeof(string),
                SupportedFieldType.XYZ => typeof(XYZ),
                SupportedFieldType.UV => typeof(UV),
                SupportedFieldType.Entity => typeof(Entity),
                _ => throw new ArgumentException(nameof(creator.FieldType))
            };

            var keyType = creator.KeyFieldType switch
            {
                SupportedKeyType.Boolean => typeof(bool),
                SupportedKeyType.Byte => typeof(Byte),
                SupportedKeyType.Int16 => typeof(Int16),
                SupportedKeyType.Int32 => typeof(Int32),
                SupportedKeyType.ElementId => typeof(ElementId),
                SupportedKeyType.GUID => typeof(Guid),
                SupportedKeyType.String => typeof(string),
                _ => throw new ArgumentException(nameof(creator.KeyFieldType))
            };

            switch (creator.ContainerType)
            {
                case ContainerType.Simple:
                    Fields.Add(new SimpleFieldDescriptor()
                    {
                        Name = creator.FieldName,
                        FieldType = type,
                        Unit = creator.FieldUnit
                    });
                    break;
                case ContainerType.Array:
                    Fields.Add(new ArrayFieldDescriptor()
                    {
                        Name = creator.FieldName,
                        FieldType = type,
                        Unit = creator.FieldUnit
                    });
                    break;
                case ContainerType.Map:
                    Fields.Add(new MapFieldDescriptor()
                    {
                        Name = creator.FieldName,
                        FieldType = type,
                        KeyType = keyType,
                        Unit = creator.FieldUnit,
                    });
                    break;
            }
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler HideRequest;

        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ShowRequest;

        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}