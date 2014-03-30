using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Requc.Commands;
using Requc.Helpers;
using Requc.Models;

namespace Requc.ViewModels
{
    public class TransmissionActViewModel : BaseViewModel
    {
        public TransmissionActScheme TransmissionActScheme { get; set; }

        public ICommand RunNewActCommand
        {
            get
            {
                return _newActCommand ??
                       (_newActCommand = new RunNewActCommand());
            }
        }
        
        private RunNewActCommand _newActCommand;
    }
}