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
        public TransmissionActScheme(params DeviceColumn[] columns)
        {
            Columns = columns;
        }

        public IList<DeviceColumn> Columns { get; private set; }
        /*
        public void NextStep()
        {
            Devices[_currentDevice].Process();
        }

        private void DeviceOnProcessFinished(object sender, EventArgs eventArgs)
        {
            _currentDevice += 1;
            if (_currentDevice == Devices.Count)
            {
                _currentDevice = 0;
                ActCompleted(this, new EventArgs());
            }
            StepCompleted(this, new EventArgs());
        }

        public event EventHandler StepCompleted = Actions.DoNothing;
        public event EventHandler ActCompleted = Actions.DoNothing;
        */
    }
}
