using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Requc.Commands;
using Requc.Models;

namespace Requc.ViewModels
{
    public class SimpleProtocolViewModel : IDisposable
    {
        public SimpleProtocolViewModel()
        {
            ProtocolAct = new SimpleProtocolAct();
        }

        public SimpleProtocolAct ProtocolAct { get; private set; }

        public ICommand RunCommand 
        {
            get { return _runCommand ?? (_runCommand = new RunCommand(ProtocolAct)); }
        }

        private RunCommand _runCommand;
        
        public void Dispose()
        {
            ProtocolAct.Dispose();
        }
    }
}
