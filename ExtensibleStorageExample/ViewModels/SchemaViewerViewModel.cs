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
            var ids = Context.UiDocument.Selection.GetElementIds();
            if (ids.Count == 1)
            {
                _element = ids.ElementAt(0).ToElement(Context.Document);
                CollectValues();
            }
        }

        [RelayCommand]
        private void SelectElement()
        {
            RaiseHideRequest();
            Entities.Clear();
            var reference = Context.UiDocument.Selection.PickObject(ObjectType.Element);
            _element = Context.Document.GetElement(reference);
            CollectValues();
            RaiseShowRequest();
        }

        [RelayCommand]
        private void CreateNewSchema()
        {
            RaiseHideRequest();
            var createViewModel = new SchemaCreatorViewModel();
            var view = new SchemaCreatorView(createViewModel);
            createViewModel.CloseRequest += (s, e) => view.Close();
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

                if (field is SimpleFieldDescriptor simpleField)
                {
                    if (simpleField.Value is null) continue;

                    var value = simpleField.Value.ToString();
                    entity.Set<string>(simpleField.Name, value);
                    _element.SetEntity(entity);
                }
                else if (field is ArrayFieldDescriptor arrayField)
                {
                    var list = entity.Get<IList<string>>(arrayField.Name);
                    list.Clear();
                    foreach (var value in arrayField.Values)
                    {
                        list.Add(value.ActiveValue.ToString());
                    }

                    entity.Set(arrayField.Name, list);
                    _element.SetEntity(entity);
                }
                else if (field is MapFieldDescriptor mapFieldDescriptor)
                {
                    var dictionary = entity.Get<IDictionary<string, string>>(mapFieldDescriptor.Name);
                    dictionary.Clear();
                    foreach (var value in mapFieldDescriptor.Values)
                    {
                        dictionary.Add(value.ActiveKey.ToString(), value.ActiveValue.ToString());
                    }

                    entity.Set(mapFieldDescriptor.Name, dictionary);
                    _element.SetEntity(entity);
                }
            }

            transaction.Commit();
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