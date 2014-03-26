using System;

namespace Requc.Models.Devices
{
    public abstract class Device : IDevice
    {
        protected abstract void DoProcess();

        public event EventHandler ProcessFinished;

        public event EventHandler ProcessStarted;

        public void Process()
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return "Device"; }
            set { }
        }
    }
}
