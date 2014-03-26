using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Helpers;
using Requc.Models.Devices;

namespace Requc.Models
{
    public class TransmissionActScheme : NotificationObject
    {
        private int _currentDevice;

        public TransmissionActScheme(IList<IDevice> devices)
        {
            Devices = devices;
        }

        public IList<IDevice> Devices { get; private set; }

        public void NextStep()
        {
            Devices[_currentDevice].Process();
            _currentDevice += 1;
            StepCompleted(this, new EventArgs());
            if (_currentDevice == Devices.Count)
            {
                _currentDevice = 0;
                ActCompleted(this, new EventArgs());
            }
        }

        public event EventHandler StepCompleted = Actions.DoNothing;
        public event EventHandler ActCompleted = Actions.DoNothing;
    }
}
