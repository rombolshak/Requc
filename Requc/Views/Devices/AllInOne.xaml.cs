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
using Requc.Models;
using Requc.ViewModels;

namespace Requc.Views.Devices
{
    /// <summary>
    /// Interaction logic for AllInOne.xaml
    /// </summary>
    public partial class AllInOne : UserControl
    {
        public AllInOne()
        {
            InitializeComponent(); Loaded += (sender, args) =>
            {
                AnimationsManager.Add((Storyboard)FindResource("ForwardAnimation"), this);
                AnimationsManager.Add((Storyboard)FindResource("BackwardAnimation"), this);

                ((ProtocolDevice)DataContext).ForwardProcessStarted += ForwardProcessStarted;
                ((ProtocolDevice)DataContext).BackwardProcessStarted += BackwardProcessStarted;

                ((Storyboard)FindResource("ForwardAnimation")).Completed += ForwardCompleted;
                ((Storyboard)FindResource("BackwardAnimation")).Completed += BackwardCompleted;
            };
        }

        private void ForwardProcessStarted(object sender, EventArgs e)
        {
            var storyboard = (Storyboard)FindResource("ForwardAnimation");
            storyboard.Begin(this, true);
        }

        private void ForwardCompleted(object sender, EventArgs e)
        {
            ((ProtocolDevice)DataContext).RequestForwardProcessFinish();
        }

        private void BackwardProcessStarted(object sender, SimpleProtocolEventArgs e)
        {
            var alicePhaseAnimation = (ColorAnimation)FindResource("AlicePhaseShiftAnimation");
            var bobPhaseAnimation = (ColorAnimation)FindResource("BobPhaseShiftAnimation");
            var detectorAnimation = (ColorAnimation)FindResource("AliceDetectorAnimation");
            var photon1Animation = (DoubleAnimation)FindResource("MiddlePhoton1Animation");
            var photon2Animation = (DoubleAnimation)FindResource("MiddlePhoton2Animation");

            var eva1Animation = (PointAnimationUsingPath) FindResource("Eva1");
            var eva2Animation = (PointAnimationUsingPath)FindResource("Eva2");

            alicePhaseAnimation.To = Math.Abs(e.Item.AlicePhase - e.Item.Phase0) < 1e-5 ? Colors.DarkGreen : Colors.Brown;
            bobPhaseAnimation.To = Math.Abs(e.Item.BobPhase - e.Item.Phase0) < 1e-5 ? Colors.DarkGreen : Colors.Brown;
            var destructiveInterference = Math.Abs(e.Item.AlicePhase - e.Item.BobPhase) < 1e-5;
            detectorAnimation.To = destructiveInterference ? Colors.Black : Colors.GreenYellow;
            photon1Animation.To = destructiveInterference ? 0 : 1;
            photon2Animation.To = destructiveInterference ? 0 : 1;

            eva1Animation.Duration = new Duration(TimeSpan.FromSeconds(4));
            eva1Animation.PathGeometry = (PathGeometry) FindResource("EvaNoCatchWay1");
            eva2Animation.Duration = new Duration(TimeSpan.FromSeconds(4));
            eva2Animation.PathGeometry = (PathGeometry)FindResource("EvaNoCatchWay2");
            
            var storyboard = (Storyboard)FindResource("BackwardAnimation");
            storyboard.Begin(this, true);
        }

        private void BackwardCompleted(object sender, EventArgs e)
        {
            ((ProtocolDevice)DataContext).RequestBackwardProcessFinish();
        }
    }
}
