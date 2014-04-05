using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Requc.Models.Devices;
using Requc.ViewModels;

namespace Requc.Views.Devices
{
    /// <summary>
    /// Interaction logic for Alice.xaml
    /// </summary>
    public partial class Alice : UserControl
    {
        public Alice()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
                {
                    AnimationsManager.Add((Storyboard)FindResource("Animations"));
                    ((AliceDevice) DataContext).ProcessStarted += Alice_ProcessStarted;
                    ((Storyboard)FindResource("Animations")).Completed += Alice_Completed;
                };
        }

        void Alice_Completed(object sender, EventArgs e)
        {
            ((AliceDevice) DataContext).RequestProcessFinish();
        }

        void Alice_ProcessStarted(object sender, EventArgs e)
        {
            var storyboard = (Storyboard) FindResource("Animations");
            storyboard.Begin(this);
        }
    }
}
