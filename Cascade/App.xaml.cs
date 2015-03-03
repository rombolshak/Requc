using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Cascade.Model;
using Cascade.ViewModel;
using FirstFloor.ModernUI.Presentation;

namespace Cascade
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            AppearanceManager.Current.AccentColor = Color.FromRgb(0x76, 0x60, 0x8a);  // mauve
            var environment = new CascadeProtocolRuntimeEnvironment();
            var viewModel = new ProtocolViewModel(new ProtocolRunner(environment), environment);
            new MainWindow{ DataContext = viewModel }.Show();
        }
    }
}
