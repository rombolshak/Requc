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
                    AnimationsManager.Add((Storyboard) FindResource("Animation"), this);
                    ((SimpleProtocolAct)DataContext).Started += (o, eventArgs) => ((Storyboard)FindResource("Animation")).Begin(this);
                };
        }
    }
}
