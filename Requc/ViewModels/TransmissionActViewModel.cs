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

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}