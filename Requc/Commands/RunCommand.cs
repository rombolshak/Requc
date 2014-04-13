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
        public RunCommand(SimpleProtocolAct protocolAct, ObservableCollection<TransmissionItem> transmissionItems)
        {
            _protocolAct = protocolAct;
            _protocolAct.Finished += ProtocolActFinished;
            _transmissionItems = transmissionItems;
        }

        protected override void OnExecute(object parameter)
        {
            IsExecuting = true;
            var item = _protocolAct.Process();
            _transmissionItems.Add(item);
        }

        private void ProtocolActFinished(object sender, EventArgs eventArgs)
        {
            IsExecuting = false;
        }

        private readonly SimpleProtocolAct _protocolAct;
        private readonly ObservableCollection<TransmissionItem> _transmissionItems;
    }
}
