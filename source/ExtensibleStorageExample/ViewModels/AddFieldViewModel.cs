using Autodesk.Revit.DB.ExtensibleStorage;
using ExtensibleStorageExample.Enums;

namespace ExtensibleStorageExample.ViewModels
{
    public partial class AddFieldViewModel : ObservableObject
    {
        [ObservableProperty] private ContainerType _containerType;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _fieldName;

        [ObservableProperty] private SupportedFieldType _fieldType;
        [ObservableProperty] private ForgeTypeId _fieldUnit;
        [ObservableProperty] private SupportedKeyType _keyFieldType;

        public SupportedFieldType[] SupportedFields { get; } = Enum.GetValues(typeof(SupportedFieldType)).Cast<SupportedFieldType>().ToArray();
        public SupportedKeyType[] SupportedKeys { get; } = Enum.GetValues(typeof(SupportedKeyType)).Cast<SupportedKeyType>().ToArray();
        public ContainerType[] ContainerTypes { get; } = Enum.GetValues(typeof(ContainerType)).Cast<ContainerType>().ToArray();
        public bool IsReady { get; set; }
        public IList<ForgeTypeId> Units { get; set; } = UnitUtils.GetAllUnits();


        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save()
        {
            IsReady = true;
            RaiseCloseRequest();
        }

        private bool CanSave()
        {
            return !FieldName.IsNullOrWhiteSpace();
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}