using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using Requc.Commands;
using Requc.Models;

namespace Requc.ViewModels
{
    public class SimpleProtocolViewModel : IDisposable
    {
        public SimpleProtocolViewModel()
        {
            ProtocolAct = new SimpleProtocolAct();
            TransmissionItems = new ObservableCollection<TransmissionItem>();
            ModelingMode = ModelingMode.Random;
            RepeatCount = 1;
        }

        public SimpleProtocolAct ProtocolAct { get; private set; }

        public ObservableCollection<TransmissionItem> TransmissionItems { get; private set; }

        public ModelingMode ModelingMode { get; set; }

        public int RepeatCount { get; set; }

        public ICommand RunCommand 
        {
            get
            {
                return _runCommand ??
                       (_runCommand =
                        new RelayCommand(
                            parameter =>
                            (_internalRunCommand = new RunCommand(ProtocolAct, TransmissionItems, ModelingMode, RepeatCount)).Execute(parameter),
                            o => _internalRunCommand == null || _internalRunCommand.CanExecute(o)));
            }
        }

        private RelayCommand _runCommand;
        private RunCommand _internalRunCommand;
        
        public void Dispose()
        {
            ProtocolAct.Dispose();
        }
    }
}
