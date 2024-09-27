using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorCreatorExample.Core
{
    public static class FloorCreator
    {
        public static void CreateFloors(ElementType floorType, Room[] rooms)
        {
            var options = new SpatialElementBoundaryOptions()
            {
                SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish,
                StoreFreeBoundaryFaces = true
            };
            var ids = new List<ElementId>();
            using var transaction = new Transaction(floorType.Document);
            transaction.Start("Create floors");
            foreach (var room in rooms)
            {
                var segments = room.GetBoundarySegments(options).First();
                var loop = new CurveLoop();
                foreach (var segment in segments)
                {
                    loop.Append(segment.GetCurve());
                }
                var list = new List<CurveLoop>() { loop };
                var floor = Floor.Create(floorType.Document, list, floorType.Id, room.LevelId);
                ids.Add(floor.Id);
            }
            transaction.Commit();
            Context.ActiveUiDocument.Selection.SetElementIds(ids);
        }
    }
}
