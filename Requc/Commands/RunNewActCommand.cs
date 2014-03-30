using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Models;

namespace Requc.Commands
{
    class RunNewActCommand : CommonCommand 
    {
        protected override void OnExecute(object parameter)
        {
            _act = new TransmissionAct((TransmissionActScheme) parameter);
            _act.Completed += ActOnCompleted;
            _act.Run();
        }

        private void ActOnCompleted(object sender, EventArgs eventArgs)
        {
            _act.Completed -= ActOnCompleted;
            _act.Dispose();
        }

        private TransmissionAct _act;
    }
}
