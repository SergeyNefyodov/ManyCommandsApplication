using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterExample.Updaters
{
    public class ParametersUpdater : IUpdater
    {
        public void Execute(UpdaterData data)
        {
            var document = data.GetDocument();
            var ids = data.GetAddedElementIds().ToList();
            ids.AddRange(data.GetModifiedElementIds());

            foreach (var id in ids)
            {
                var element = document.GetElement(id);
                if (element is MEPCurve)
                {
                    var length = element.FindParameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble().ToMillimeters().Round(2);
                    element.FindParameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(length.ToString());
                }    
            }
        }

        public string GetAdditionalInformation()
        {
            return string.Empty;
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Annotations;
        }

        public UpdaterId GetUpdaterId()
        {
            return new UpdaterId(Context.Application.ActiveAddInId, new Guid("A7BFD833-1472-4FE0-B0F0-408AC43B3DA1"));
        }

        public string GetUpdaterName()
        {
            return "LastUpdater";
        }
    }
}
