using System.Collections.ObjectModel;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI.Selection;
using ExtensibleStorageExample.Models;
using ExtensibleStorageExample.Views;

namespace ExtensibleStorageExample.ViewModels
{
    public sealed partial class SchemaViewerViewModel : ObservableObject
    {
        private Element _element;
        [ObservableProperty] private ObservableCollection<EntityDescriptor> _entities = [];
        [ObservableProperty] private EntityDescriptor _selectedEntity;

        public SchemaViewerViewModel()
        {
            var ids = Context.UiDocument!.Selection.GetElementIds();
            if (ids.Count != 1) return;

            _element = ids.ElementAt(0).ToElement(Context.Document);
            CollectValues();
        }

        [RelayCommand]
        private void SelectElement()
        {
            RaiseHideRequest();
            Entities.Clear();
            var reference = Context.UiDocument!.Selection.PickObject(ObjectType.Element);
            _element = Context.Document.GetElement(reference);
            CollectValues();
            RaiseShowRequest();

            var color = System.Windows.Media.Colors.Green;
        }

        [RelayCommand]
        private void CreateNewSchema()
        {
            RaiseHideRequest();
            var createViewModel = new SchemaCreatorViewModel();
            var view = new SchemaCreatorView(createViewModel);
            createViewModel.CloseRequest += (_, _) => view.Close();
            view.ShowDialog();
            RaiseShowRequest();
        }

        [RelayCommand]
        private void SaveData()
        {
            var schema = Schema.Lookup(SelectedEntity.Guid);

            using var transaction = new Transaction(Context.Document, "Save extensible storage data");
            transaction.Start();
            foreach (var field in SelectedEntity.Fields)
            {
                if (field.FieldType != typeof(string)) continue;

                var entity = _element.GetEntity(schema);
                if (entity.Schema is null)
                {
                    entity = new Entity(schema);
                }

                if (field is SimpleFieldDescriptor simpleFieldDescriptor)
                {
                    SaveSimpleValue(simpleFieldDescriptor, entity);
                }
                else if (field is ArrayFieldDescriptor arrayFieldDescriptor)
                {
                    SaveArrayValue(entity, arrayFieldDescriptor);
                }
                else if (field is MapFieldDescriptor mapFieldDescriptor)
                {
                    SaveMapValue(entity, mapFieldDescriptor);
                }
            }

            transaction.Commit();
        }

        private void SaveMapValue(Entity entity, MapFieldDescriptor mapFieldDescriptor)
        {
            var dictionary = entity.Get<IDictionary<string, string>>(mapFieldDescriptor.Name);
            dictionary.Clear();
            foreach (var value in mapFieldDescriptor.Values)
            {
                dictionary.Add(value.ActiveKey.ToString()!, value.ActiveValue.ToString());
            }

            entity.Set(mapFieldDescriptor.Name, dictionary);
            _element.SetEntity(entity);
        }

        private void SaveArrayValue(Entity entity, ArrayFieldDescriptor arrayFieldDescriptor)
        {
            var list = entity.Get<IList<string>>(arrayFieldDescriptor.Name);
            list.Clear();
            foreach (var value in arrayFieldDescriptor.Values)
            {
                list.Add(value.ActiveValue.ToString());
            }

            entity.Set(arrayFieldDescriptor.Name, list);
            _element.SetEntity(entity);
        }

        private void SaveSimpleValue(SimpleFieldDescriptor simpleField, Entity entity)
        {
            if (simpleField.Value is null) return;

            var value = simpleField.Value.ToString();
            entity.Set(simpleField.Name, value);
            _element.SetEntity(entity);
        }


        [RelayCommand]
        private void EditArrayField(ArrayFieldDescriptor descriptor)
        {
            var view = new ArrayFieldEditor(descriptor);
            view.ShowDialog();
        }

        [RelayCommand]
        private void EditMapField(MapFieldDescriptor descriptor)
        {
            var view = new MapFieldEditor(descriptor);
            view.ShowDialog();
        }

        private void CollectValues()
        {
            var schemas = Schema.ListSchemas();

            foreach (var schema in schemas)
            {
                if (schema.ReadAccessGranted())
                {
                    var entity = _element.GetEntity(schema);
                    if (entity is not null && entity.Schema is not null)
                    {
                        Entities.Add(new EntityDescriptor(entity));
                    }
                    else
                    {
                        Entities.Add(new EntityDescriptor(schema));
                    }
                }
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