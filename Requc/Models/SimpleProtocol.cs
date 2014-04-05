using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Helpers;
using Requc.Models.Devices;

namespace Requc.Models
{
    public class SimpleProtocol : IDisposable
    {
        public SimpleProtocol()
        {
            AliceDevice = new AliceDevice();
            BobDevice = new BobDevice();

            AliceDevice.ForwardProcessFinished += AliceDeviceForwardProcessFinished;
            BobDevice.ForwardProcessFinished += BobDeviceForwardProcessFinished;
            BobDevice.BackwardProcessFinished += BobDevice_BackwardProcessFinished;
            AliceDevice.BackwardProcessFinished += AliceDevice_BackwardProcessFinished;
        }

        public AliceDevice AliceDevice { get; private set; }
        public BobDevice BobDevice { get; private set; }

        public event EventHandler Finished = Actions.DoNothing;

        public void Process()
        {
            AliceDevice.ProcessForward();
        }

        void AliceDeviceForwardProcessFinished(object sender, EventArgs e)
        {
            BobDevice.ProcessForward();
        }

        void BobDeviceForwardProcessFinished(object sender, EventArgs e)
        {
            BobDevice.ProcessBackward();
        }

        void BobDevice_BackwardProcessFinished(object sender, EventArgs e)
        {
            AliceDevice.ProcessBackward();
        }

        void AliceDevice_BackwardProcessFinished(object sender, EventArgs e)
        {
            Finished(this, new EventArgs());
        }

        public void Dispose()
        {
            AliceDevice.ForwardProcessFinished -= AliceDeviceForwardProcessFinished;
            BobDevice.ForwardProcessFinished -= BobDeviceForwardProcessFinished;
        }
    }
}
