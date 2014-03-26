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
        #region Ctor
        public MainWindowViewModel()
        {
            TransmissionAct = new TransmissionActViewModel()
                {
                    TransmissionActScheme = new TransmissionActScheme(new IDevice[]
                        {
                            new LaserDevice(),
                            new PhaseShiftDevice(),
                        })
                };
        }
        #endregion

        #region Properties
        public TransmissionActViewModel TransmissionAct { get; private set; }
        #endregion

        #region Commands
        
        #endregion
        
        #region Command Handlers

        #endregion
    }
}