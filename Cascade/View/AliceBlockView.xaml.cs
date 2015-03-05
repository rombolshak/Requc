using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Cascade.Helpers;
using Cascade.ViewModel;

namespace Cascade.View
{
    /// <summary>
    /// Interaction logic for AliceBlockView.xaml
    /// </summary>
    public partial class AliceBlockView : UserControl
    {
        public AliceBlockView()
        {
            InitializeComponent();
            Loaded += (o, a) =>
            {
                ((BlockViewModel)DataContext).ChangeVisualStateRequested +=
                    (o2, e) => Application.Current.Dispatcher.Invoke(OnChangeVisualStateRequested);
            };
        }

        private void OnChangeVisualStateRequested()
        {
            StateManager.AddAnimation();
            var viewModel = (BlockViewModel)DataContext;
            var newHeight = viewModel.WorkingSize * 10;
            var newMargin = new Thickness(0, viewModel.StartPosition * 20, 0, 2);

            var heightAnimation = (DoubleAnimation)FindResource("HeightAnimation");
            var marginAnimation = (ThicknessAnimation)FindResource("MarginAnimation");
            var storyboard = (Storyboard)FindResource("BinaryStoryboard");

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
