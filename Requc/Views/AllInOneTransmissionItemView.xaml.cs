using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Requc.Models;
using Requc.ViewModels;

namespace Requc.Views
{
    /// <summary>
    /// Interaction logic for AliceTransmissionItemView.xaml
    /// </summary>
    public partial class AllInOneTransmissionItemView : UserControl
    {
        public AllInOneTransmissionItemView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
                {
                    var item = (TransmissionItem) DataContext;
                    var aliceCellColorAnimation = (ColorAnimation) FindResource("AliceCellColorAnimation");
                    var bobCellColorAnimation = (ColorAnimation)FindResource("BobCellColorAnimation");
                    aliceCellColorAnimation.To = bobCellColorAnimation.To = item.IsGood
                                                                                ? Colors.YellowGreen
                                                                                : item.IsEvaDetected
                                                                                      ? Colors.Red
                                                                                      : Colors.Transparent;
                    var animation = (Storyboard) FindResource("TransmissionItemAnimation");
                    AnimationsManager.Add(animation, this);
                    animation.Completed +=
                        (o, eventArgs) => AnimationsManager.Remove(animation, this);
                };
        }
    }
}
