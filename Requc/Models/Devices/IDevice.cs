using System;

namespace Requc.Models.Devices
{
    public interface IDevice
    {
        event EventHandler ForwardProcessFinished;
        event EventHandler ForwardProcessStarted;
        event EventHandler BackwardProcessFinished;
        event EventHandler BackwardProcessStarted;

        void ProcessForward();
        void ProcessBackward();
    }
}
