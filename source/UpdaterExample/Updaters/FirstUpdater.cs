using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterExample.Updaters
{
    public class FirstUpdater : IUpdater
    {
        public void Execute(UpdaterData data)
        {
            TaskDialog.Show("1", "First updater executed");
        }

        public string GetAdditionalInformation()
        {
            return string.Empty;
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.GridsLevelsReferencePlanes;
        }

        public UpdaterId GetUpdaterId()
        {
            return new UpdaterId(Context.Application.ActiveAddInId, new Guid("{76CAE3CD-97FE-43EB-BD53-4F99D8C54518}"));
        }

        public string GetUpdaterName()
        {
            return "FirstUpdater";
        }
    }
}
