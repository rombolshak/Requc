using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Models;

namespace Requc.Commands
{
    class RunCommand : CommonCommand 
    {
        public RunCommand(
            SimpleProtocolAct protocolAct,
            ObservableCollection<TransmissionItem> transmissionItems,
            ModelingMode modelingMode, 
            int repeatCount)
        {
            _protocolAct = protocolAct;
            _protocolAct.Finished += ProtocolActFinished;
            _transmissionItems = transmissionItems;
            _modelingMode = modelingMode;
            _repeatCount = repeatCount;
        }

        protected override void OnExecute(object parameter)
        {
            IsExecuting = true;
            var item = _protocolAct.Process(_modelingMode);
            _transmissionItems.Add(item);
        }

        private void ProtocolActFinished(object sender, EventArgs eventArgs)
        {
            if (--_repeatCount <= 0)
            {
                IsExecuting = false;
            }
            else
            {
                Execute(null);
            }
        }

        private readonly SimpleProtocolAct _protocolAct;
        private readonly ObservableCollection<TransmissionItem> _transmissionItems;
        private readonly ModelingMode _modelingMode;
        private int _repeatCount;
    }
}
