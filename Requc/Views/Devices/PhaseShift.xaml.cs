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

namespace Requc.Views.Devices
{
    /// <summary>
    /// Interaction logic for PhaseShift.xaml
    /// </summary>
    public partial class PhaseShift : UserControl
    {
        public PhaseShift()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                ((PhaseShiftDevice)DataContext).ProcessStarted += OnProcessStarted;
            };
        }

        private void OnProcessStarted(object sender, EventArgs eventArgs)
        {
            //Dispatcher.Invoke(() =>
            //{
                var photon = ((EllipseGeometry)FindResource("Photon")).Clone();
                var name = "ph" + Guid.NewGuid().ToString("N");
                RegisterName(name, photon);

                var path = new Path
                {
                    Stroke = Brushes.Wheat,
                    StrokeThickness = 4,
                    Data = photon
                };
                ((Grid)Content).Children.Add(path);

                var colorAnimation = (ColorAnimation)FindResource("ColorAnimation");
                colorAnimation.To = Colors.Indigo;
                var animation = ((Storyboard)FindResource("Storyboard")).Clone();
                Storyboard.SetTargetName(animation, name);
                animation.Completed += (o, args) =>
                {
                    animation.Remove();
                    UnregisterName(name);
                    ((Grid)Content).Children.Remove(path);
                    ((Device)DataContext).RequestProcessFinish();
                };
                BeginStoryboard(animation);
            //});
        }
    }
}
