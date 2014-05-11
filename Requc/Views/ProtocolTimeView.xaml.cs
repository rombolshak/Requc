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

namespace Requc.Views
{
    /// <summary>
    /// Interaction logic for ProtocolTimeView.xaml
    /// </summary>
    public partial class ProtocolTimeView : UserControl
    {
        public ProtocolTimeView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
                {
                    var catchedAnimation = (Storyboard) FindResource("CatchedAnimation");
                    var notCatchedAnimation = (Storyboard)FindResource("NotCatchedAnimation");
                    var showPaths = (Storyboard)FindResource("ShowPaths");

                    AnimationsManager.Add(catchedAnimation, this);
                    AnimationsManager.Add(notCatchedAnimation, this);

                    ((SimpleProtocolAct) DataContext).Started += (o, eventArgs) =>
                        {
                            if (eventArgs.Item.CatchedByEva)
                            {
                                if (eventArgs.Item.IsBlocked)
                                {
                                    ((Storyboard) FindResource("HidePhotons")).BeginTime = TimeSpan.FromSeconds(29);
                                    showPaths.BeginTime = TimeSpan.FromSeconds(28.5);
                                    Storyboard.SetTargetName(showPaths.Children[0], "Path1Blocked");
                                    Storyboard.SetTargetName(showPaths.Children[1], "Path2Blocked");
                                }
                                else
                                {
                                    ((Storyboard)FindResource("HidePhotons")).BeginTime = TimeSpan.FromSeconds(42);
                                    showPaths.BeginTime = TimeSpan.FromSeconds(41.5);
                                    Storyboard.SetTargetName(showPaths.Children[0], "Path1Catched");
                                    Storyboard.SetTargetName(showPaths.Children[1], "Path2Catched");
                                }

                                catchedAnimation.Begin(this);
                            }
                            else
                            {
                                var destructiveInterference = Math.Abs(eventArgs.Item.AlicePhase - eventArgs.Item.BobPhase) < 1e-5;
                                if (destructiveInterference)
                                {
                                    ((Storyboard)FindResource("HidePhotons")).BeginTime = TimeSpan.FromSeconds(37);
                                    showPaths.BeginTime = TimeSpan.FromSeconds(36.5);
                                    Storyboard.SetTargetName(showPaths.Children[0], "Path1Destructed");
                                    Storyboard.SetTargetName(showPaths.Children[1], "Path2Destructed");
                                }
                                else
                                {
                                    ((Storyboard)FindResource("HidePhotons")).BeginTime = TimeSpan.FromSeconds(40);
                                    showPaths.BeginTime = TimeSpan.FromSeconds(39.5);
                                    Storyboard.SetTargetName(showPaths.Children[0], "Path1");
                                    Storyboard.SetTargetName(showPaths.Children[1], "Path2");
                                }
                                
                                notCatchedAnimation.Begin(this);
                            }
                        };
                };
        }
    }
}
