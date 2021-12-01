using System.Windows;
using Prism.Ioc;
using MSOptimization.Views;

namespace MSOptimization
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
