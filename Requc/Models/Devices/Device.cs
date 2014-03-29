using System;
using Requc.Helpers;

namespace Requc.Models.Devices
{
    public abstract class Device : IDevice
    {
        protected abstract void DoProcess();

        public event EventHandler ProcessFinished = Actions.DoNothing;

        public event EventHandler ProcessStarted = Actions.DoNothing;

        public void Process()
        {
            DoProcess();
            ProcessStarted(this, new EventArgs());
        }

        public void RequestProcessFinish()
        {
            ProcessFinished(this, new EventArgs());
        }
    }
}
