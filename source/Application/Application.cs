using Application.Commands;
using ExtensibleStorageExample.Commands;
using ExternalCommandOne.Commands;
using Nice3point.Revit.Toolkit.External;

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
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel("Commands", "Application");

            panel.AddPushButton<ExternalCommandOneCommand>("Command 1")
                .SetImage("/Application;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/Application;component/Resources/Icons/RibbonIcon32.png");

            panel.AddPushButton<ExtensibleStorageExampleCommand>("Command 2")
                .SetImage("/Application;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/Application;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}