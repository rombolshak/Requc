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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Requc.ViewModels;

namespace Requc.Views
{
    /// <summary>
    /// Interaction logic for SimpleProtocolView.xaml
    /// </summary>
    public partial class SimpleProtocolView : UserControl
    {
        public SimpleProtocolView()
        {
            InitializeComponent();
        }

        private void AnimationSpeedSetterValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AnimationsManager.SetSpeed(e.NewValue);
        }

        private void AnimationSpeedSetterTextChanged(object sender, TextChangedEventArgs e)
        {
            double newSpeed;
            Double.TryParse(((TextBox) sender).Text, out newSpeed);
            if (newSpeed > 0 && newSpeed <= 100)
            {
                AnimationsManager.SetSpeed(newSpeed);
            }
        }
    }
}
