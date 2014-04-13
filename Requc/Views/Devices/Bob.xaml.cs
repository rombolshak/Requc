using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Requc.Models;
using Requc.ViewModels;

namespace Requc.Views.Devices
{
    /// <summary>
    /// Interaction logic for Bob.xaml
    /// </summary>
    public partial class Bob : UserControl
    {
        public Bob()
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

        void ForwardProcessStarted(object sender, SimpleProtocolEventArgs e)
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
            var animation = (ColorAnimation) FindResource("PhaseShiftAnimation");
            animation.To = Math.Abs(e.Item.BobPhase - e.Item.Phase0) < 1e-5 ? Colors.DarkGreen : Colors.Brown;
            var storyboard = (Storyboard)FindResource("BackwardAnimation");
            storyboard.Begin(this);
        }

        private void BackwardCompleted(object sender, EventArgs e)
        {
            ((ProtocolDevice)DataContext).RequestBackwardProcessFinish();
        }
    }
}
