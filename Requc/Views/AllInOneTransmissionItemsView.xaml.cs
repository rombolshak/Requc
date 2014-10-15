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
    /// Interaction logic for AliceTransmissionItemsView.xaml
    /// </summary>
    public partial class AllInOneTransmissionItemsView : UserControl
    {
        public AllInOneTransmissionItemsView()
        {
            InitializeComponent();
            _scrollViewer.ScrollChanged += (sender, args) =>
                {
                    var sv = sender as ScrollViewer;
                    //if (e.ExtentHeightChange != 0 && Math.Abs(sv.VerticalOffset - sv.ScrollableHeight) < 20)
                    if (sv.ScrollableHeight - sv.VerticalOffset < 20)
                    {
                        sv.ScrollToEnd();
                    }
                };
        }
    }
}
