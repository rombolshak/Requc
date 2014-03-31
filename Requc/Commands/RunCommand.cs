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
        public RunCommand(SimpleProtocol protocol)
        {
            _protocol = protocol;
        }

        protected override void OnExecute(object parameter)
        {
            IsExecuting = true;
            _protocol.Finished += ProtocolFinished;
            _protocol.Process();
        }

        private void ProtocolFinished(object sender, EventArgs eventArgs)
        {
            _protocol.Finished -= ProtocolFinished;
            IsExecuting = false;
        }

        private readonly SimpleProtocol _protocol;
    }
}
