using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Cascade.Helpers;
using Cascade.ViewModel;

namespace Cascade.View
{
    /// <summary>
    /// Interaction logic for BobBlockView.xaml
    /// </summary>
    public partial class BobBlockView : UserControl
    {
        public BobBlockView()
        {
            InitializeComponent();
            Loaded += (o, a) =>
                {
                    ((BlockViewModel) DataContext).ChangeVisualStateRequested +=
                        (o2, e) => Application.Current.Dispatcher.Invoke(OnChangeVisualStateRequested);
                };
        }

        private void OnChangeVisualStateRequested()
        {   
            StateManager.AddAnimation();
            var viewModel = (BlockViewModel)DataContext;
            var newHeight = viewModel.WorkingSize*10;
            var newMargin = new Thickness(0, viewModel.StartPosition*20, 0, 2);

            var heightAnimation = (DoubleAnimation)FindResource("HeightAnimation");
            var marginAnimation = (ThicknessAnimation) FindResource("MarginAnimation");
            var storyboard = (Storyboard) FindResource("BinaryStoryboard");

            heightAnimation.To = newHeight;
            marginAnimation.To = newMargin;
            storyboard.Completed += StoryboardOnCompleted;
            storyboard.Begin(this);
        }

        private void StoryboardOnCompleted(object sender, EventArgs eventArgs)
        {
            ((Storyboard)FindResource("BinaryStoryboard")).Completed -= StoryboardOnCompleted;
            StateManager.OnAnimationCompleted();
        }
    }
}
