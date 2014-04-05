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
                    AnimationsManager.Add((Storyboard)FindResource("ForwardAnimation"));
                    AnimationsManager.Add((Storyboard)FindResource("BackwardAnimation"));

                    ((Device)DataContext).ForwardProcessStarted += ForwardProcessStarted;
                    ((Device)DataContext).BackwardProcessStarted += BackwardProcessStarted;

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
            ((Device)DataContext).RequestForwardProcessFinish();
        }

        private void BackwardProcessStarted(object sender, EventArgs e)
        {
            var storyboard = (Storyboard)FindResource("BackwardAnimation");
            storyboard.Begin(this);
        }

        private void BackwardCompleted(object sender, EventArgs e)
        {
            ((Device)DataContext).RequestBackwardProcessFinish();
        }
    }
}
