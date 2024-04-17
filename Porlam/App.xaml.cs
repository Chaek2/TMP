using System.Windows;
using NLog;

namespace Porlam
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LogManager.LoadConfiguration("nlog.config");
        }
    }
}
