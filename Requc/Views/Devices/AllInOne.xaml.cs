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
            var aliceDetectorAnimation = (ColorAnimation)FindResource("AliceDetectorAnimation");
            var evaDetectorAnimation = (ColorAnimation)FindResource("EvaDetectorAnimation");
            var photon1Animation = (DoubleAnimation)FindResource("MiddlePhoton1Animation");
            var photon2Animation = (DoubleAnimation)FindResource("MiddlePhoton2Animation");

            var eva1Animation = (PointAnimationUsingPath) FindResource("Eva1");
            var eva2Animation = (PointAnimationUsingPath)FindResource("Eva2");
            
            var aliceRng = (TextBlock)FindResource("AliceRng");
            var bobRng = (TextBlock)FindResource("BobRng");
            
            var storyboard = (Storyboard)FindResource("BackwardAnimation");

            alicePhaseAnimation.To = Math.Abs(e.Item.AlicePhase - e.Item.Phase0) < 1e-5 ? Colors.DeepSkyBlue : Colors.Orchid;
            bobPhaseAnimation.To = Math.Abs(e.Item.BobPhase - e.Item.Phase0) < 1e-5 ? Colors.DeepSkyBlue : Colors.Orchid;
            var destructiveInterference = Math.Abs(e.Item.AlicePhase - e.Item.BobPhase) < 1e-5;
            aliceRng.Text = e.Item.AliceValue ? "1" : "0";
            bobRng.Text = e.Item.BobValue ? "1" : "0";
           
            if (e.Item.CatchedByEva)
            {
                eva1Animation.PathGeometry = (PathGeometry) FindResource("EvaCatchedWay1");
                eva2Animation.PathGeometry = (PathGeometry)FindResource("EvaCatchedWay2");
                ((Storyboard)FindResource("AliceBackwardAnimation")).BeginTime = TimeSpan.FromSeconds(11);
                alicePhaseAnimation.BeginTime = TimeSpan.FromSeconds(2.9);
                aliceDetectorAnimation.BeginTime = TimeSpan.FromSeconds(9.9);
                aliceDetectorAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                aliceDetectorAnimation.To = Colors.GreenYellow;
                photon2Animation.To = photon1Animation.To = 1;
                evaDetectorAnimation.To = e.Item.EvaResult == MeasurementResult.Inconclusive
                                              ? Colors.Blue
                                              : e.Item.EvaResult == MeasurementResult.Phase0
                                                    ? Colors.DeepSkyBlue
                                                    : Colors.Orchid;
                if (!storyboard.Children.Contains((Timeline) FindResource("EveMirrorAnimation")))
                {
                    storyboard.Children.Insert(storyboard.Children.Count - 1,
                                               (Timeline) FindResource("EveMirrorAnimation"));
                }
                if (!storyboard.Children.Contains((Timeline) FindResource("FromEveToAliceAnimation")))
                {
                    storyboard.Children.Insert(storyboard.Children.Count - 1,
                                               (Timeline) FindResource("FromEveToAliceAnimation"));
                }
                if (!storyboard.Children.Contains((Timeline)FindResource("AliceDetectorAlarmAnimation")))
                {
                    storyboard.Children.Insert(storyboard.Children.Count - 1,
                                               (Timeline) FindResource("AliceDetectorAlarmAnimation"));
                }
            }
            else
            {
                eva1Animation.PathGeometry = (PathGeometry)FindResource("EvaNoCatchWay1");
                eva2Animation.PathGeometry = (PathGeometry)FindResource("EvaNoCatchWay2");
                ((Storyboard)FindResource("AliceBackwardAnimation")).BeginTime = TimeSpan.FromSeconds(9);
                alicePhaseAnimation.BeginTime = TimeSpan.FromSeconds(3.9);
                aliceDetectorAnimation.BeginTime = TimeSpan.FromSeconds(10.9);
                aliceDetectorAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                aliceDetectorAnimation.To = destructiveInterference ? Colors.Black : Colors.GreenYellow;
                photon2Animation.To = photon1Animation.To = destructiveInterference ? 0 : 1;
                evaDetectorAnimation.To = Colors.Black;
                storyboard.Children.Remove((Timeline)FindResource("EveMirrorAnimation"));
                storyboard.Children.Remove((Timeline)FindResource("FromEveToAliceAnimation"));
                storyboard.Children.Remove((Timeline)FindResource("AliceDetectorAlarmAnimation"));
            }

            storyboard.Begin(this, true);
        }

        private void BackwardCompleted(object sender, EventArgs e)
        {
            ((ProtocolDevice)DataContext).RequestBackwardProcessFinish();
        }
    }
}
