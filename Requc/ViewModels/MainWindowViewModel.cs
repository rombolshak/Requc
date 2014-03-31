using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Requc.Helpers;
using Requc.Models;
using Requc.Models.Devices;

namespace Requc.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            SimpleProtocol = new SimpleProtocolViewModel();
        }

        public SimpleProtocolViewModel SimpleProtocol { get; private set; }

        public override void Dispose()
        {
            SimpleProtocol.Dispose();
        }
    }
}