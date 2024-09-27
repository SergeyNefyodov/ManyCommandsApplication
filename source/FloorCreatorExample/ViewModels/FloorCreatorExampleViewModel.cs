using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using FloorCreatorExample.Core;
using System.Collections.ObjectModel;

namespace FloorCreatorExample.ViewModels
{
    public sealed partial class FloorCreatorExampleViewModel : ObservableObject
    {
        [ObservableProperty] private ElementType _selectedFloorType;
        [ObservableProperty] private ElementType[] _floorTypes;
        [ObservableProperty] private Room[] _rooms = [];

        public FloorCreatorExampleViewModel()
        {
            FloorTypes = Context.ActiveDocument
                .EnumerateTypes<FloorType>()
                .ToArray();
        }

        [RelayCommand]
        private void SelectRooms()
        {
            RaiseHideRequest();

            var references = Context.ActiveUiDocument.Selection.PickObjects(ObjectType.Element,
                new RoomSelectionFilter(),
                "Select rooms");

            Rooms = references
                .Select(reference => (Room)Context.ActiveDocument.GetElement(reference))
                .ToArray();

            RaiseShowRequest();
        }

        [RelayCommand]
        private void CreateFloors()
        {
            try
            {
                FloorCreator.CreateFloors(SelectedFloorType, Rooms);
            }
            finally
            {
                RaiseCloseRequest();
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