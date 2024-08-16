using ExtensibleStorageExample.Commands;
using ExternalCommandOne.Commands;
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
        }

        private void RegisterUpdaters()
        {
            var parametersUpdater = new ParametersUpdater();
            UpdaterRegistry.RegisterUpdater(parametersUpdater);
            var updaterId = parametersUpdater.GetUpdaterId();

            var filter = new ElementClassFilter(typeof(MEPCurve));
            UpdaterRegistry.AddTrigger(updaterId, filter, Element.GetChangeTypeGeometry());
            UpdaterRegistry.AddTrigger(updaterId, filter, Element.GetChangeTypeElementAddition());

            var firstUpdater = new FirstUpdater();
            UpdaterRegistry.RegisterUpdater(firstUpdater);
            var firstUpdaterId = firstUpdater.GetUpdaterId();

            UpdaterRegistry.AddTrigger(firstUpdaterId, filter, Element.GetChangeTypeGeometry());
            UpdaterRegistry.AddTrigger(firstUpdaterId, filter, Element.GetChangeTypeElementAddition());
        }
    }
}