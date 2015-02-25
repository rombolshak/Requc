using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cascade.ViewModel;

namespace Cascade.View
{
    /// <summary>
    /// Interaction logic for ProtocolView.xaml
    /// </summary>
    public partial class ProtocolView : UserControl
    {
        public ProtocolView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
                {
                    var viewModel = (ProtocolViewModel) DataContext;
                    //viewModel.StepStarted += ViewModelOnStepStarted;
                    //viewModel.StepFinished += ViewModelOnStepFinished;
                    viewModel.StartProcessCommand.Execute(null);
                    //viewModel.NextStepCommand.Execute(null);
                };

            
        }
    }
}
