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
using Requc.ViewModels;

namespace Requc.Views
{
    /// <summary>
    /// Interaction logic for AliceTransmissionItemView.xaml
    /// </summary>
    public partial class AliceTransmissionItemView : UserControl
    {
        public AliceTransmissionItemView()
        {
            InitializeComponent();
            Loaded += (sender, args) => AnimationsManager.Add((Storyboard)FindResource("TransmissionItemAnimation"), this);
        }
    }
}
