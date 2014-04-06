using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Requc.Models;
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
                    AnimationsManager.Add((Storyboard)FindResource("ForwardAnimation"));
                    AnimationsManager.Add((Storyboard)FindResource("BackwardAnimation"));

                    ((ProtocolDevice)DataContext).ForwardProcessStarted += ForwardProcessStarted;
                    ((ProtocolDevice)DataContext).BackwardProcessStarted += BackwardProcessStarted;

                    ((Storyboard)FindResource("ForwardAnimation")).Completed += ForwardCompleted;
                    ((Storyboard)FindResource("BackwardAnimation")).Completed += BackwardCompleted;
                };
        }

        void ForwardProcessStarted(object sender, EventArgs e)
        {
            var storyboard = (Storyboard)FindResource("ForwardAnimation");
            storyboard.Begin(this);
        }

        void ForwardCompleted(object sender, EventArgs e)
        {
            ((ProtocolDevice)DataContext).RequestForwardProcessFinish();
        }

        private void BackwardProcessStarted(object sender, SimpleProtocolEventArgs e)
        {
            var phaseAnimation = (ColorAnimation) FindResource("PhaseShiftAnimation");
            var detectorAnimation = (ColorAnimation)FindResource("DetectorAnimation");
            var photon1Animation = (DoubleAnimation)FindResource("MiddlePhoton1Animation");
            var photon2Animation = (DoubleAnimation)FindResource("MiddlePhoton2Animation");
            var storyboard = (Storyboard)FindResource("BackwardAnimation");

            phaseAnimation.To = Math.Abs(e.AlicePhase - e.Phase0) < 1e-5 ? Colors.DarkGreen : Colors.Brown;
            var destructiveInterference = Math.Abs(e.AlicePhase - e.BobPhase) < 1e-5;
            detectorAnimation.To = destructiveInterference ? Colors.Black : Colors.GreenYellow;
            photon1Animation.To = destructiveInterference ? 0 : 1;
            photon2Animation.To = destructiveInterference ? 0 : 1;
            storyboard.Begin(this);
        }

        private void BackwardCompleted(object sender, EventArgs e)
        {
            ((ProtocolDevice)DataContext).RequestBackwardProcessFinish();
        }
    }
}
