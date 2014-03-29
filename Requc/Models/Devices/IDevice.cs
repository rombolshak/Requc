using System;

namespace Requc.Models.Devices
{
    public interface IDevice
    {
        event EventHandler ProcessFinished;

        event EventHandler ProcessStarted;

        void Process();
    }
}
