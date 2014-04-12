using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Models;

namespace Requc.Commands
{
    class RunCommand : CommonCommand 
    {
        public RunCommand(SimpleProtocolAct protocolAct)
        {
            _protocolAct = protocolAct;
        }

        protected override void OnExecute(object parameter)
        {
            IsExecuting = true;
            _protocolAct.Finished += ProtocolActFinished;
            _protocolAct.Process();
        }

        private void ProtocolActFinished(object sender, EventArgs eventArgs)
        {
            _protocolAct.Finished -= ProtocolActFinished;
            IsExecuting = false;
        }

        private readonly SimpleProtocolAct _protocolAct;
    }
}
