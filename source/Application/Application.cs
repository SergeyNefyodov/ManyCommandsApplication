using ExtensibleStorageExample.Commands;
using ExternalCommandOne.Commands;
using FloorCreatorExample.Commands;
using ImageExportExample.Commands;
using Nice3point.Revit.Toolkit.External;
using UpdaterExample.Updaters;

namespace Application
{
    /// <summary>
    ///     Application entry point
    /// </summary>
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();
            RegisterUpdaters();
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel("Commands", "Application");

            panel.AddPushButton<ExternalCommandOneCommand>("Command 1")
                .SetImage("/Application;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/Application;component/Resources/Icons/RibbonIcon32.png");

            panel.AddPushButton<ExtensibleStorageExampleCommand>("Schema Manager")
                .SetImage("/Application;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/Application;component/Resources/Icons/RibbonIcon32.png");

            panel.AddPushButton<ImageExportCommand>("Image example")
                .SetImage("/Application;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/Application;component/Resources/Icons/icons8-building-32-96.png");

            panel.AddPushButton<FloorCreatorCommand>("Create floors")
                .SetImage("/Application;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/Application;component/Resources/Icons/icons8-building-32-96.png");
        }

        private void RegisterUpdaters()
        {
            var parametersUpdater = new ParametersUpdater();
            UpdaterRegistry.RegisterUpdater(parametersUpdater,true);
            var updaterId = parametersUpdater.GetUpdaterId();

            var filter = new ElementClassFilter(typeof(MEPCurve));
            UpdaterRegistry.AddTrigger(updaterId, filter, Element.GetChangeTypeGeometry());
            UpdaterRegistry.AddTrigger(updaterId, filter, Element.GetChangeTypeElementAddition());
        }
    }
}