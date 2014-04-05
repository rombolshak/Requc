using System;
using Requc.Helpers;

namespace Requc.Models.Devices
{
    public abstract class Device : IDevice
    {
        protected abstract void DoForwardProcess();
        protected abstract void DoBackwardProcess();

        public event EventHandler ForwardProcessFinished = Actions.DoNothing;
        public event EventHandler ForwardProcessStarted = Actions.DoNothing;
        public event EventHandler BackwardProcessFinished = Actions.DoNothing;
        public event EventHandler BackwardProcessStarted = Actions.DoNothing;

        public void ProcessForward()
        {
            DoForwardProcess();
            ForwardProcessStarted(this, new EventArgs());
        }

        public void ProcessBackward()
        {
            DoBackwardProcess();
            BackwardProcessStarted(this, new EventArgs());
        }

        public void RequestForwardProcessFinish()
        {
            ForwardProcessFinished(this, new EventArgs());
        }

        public void RequestBackwardProcessFinish()
        {
            BackwardProcessFinished(this, new EventArgs());
        }
    }
}
