using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Models;

namespace Requc.Commands
{
    class RunNewActCommand : AsyncCommand 
    {
        public override string Text
        {
            get { throw new NotImplementedException(); }
        }

        protected override void OnExecute(object parameter)
        {
            var act = new TransmissionAct((TransmissionActScheme)parameter);
            act.Run();
        }
    }
}
