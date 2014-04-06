﻿using System;

namespace Requc.Models
{
    public class SimpleProtocol : IDisposable
    {
        public SimpleProtocol()
        {
            AliceProtocolDevice = new AliceProtocolDevice {Protocol = this};
            BobProtocolDevice = new BobProtocolDevice {Protocol = this};
            Params = new ProtocolParams(100, 200, Math.PI/8, Math.PI/4);

            AliceProtocolDevice.ForwardProcessFinished += AliceDeviceForwardProcessFinished;
            BobProtocolDevice.ForwardProcessFinished += BobDeviceForwardProcessFinished;
            BobProtocolDevice.BackwardProcessFinished += BobDevice_BackwardProcessFinished;
            AliceProtocolDevice.BackwardProcessFinished += AliceDevice_BackwardProcessFinished;
        }

        public AliceProtocolDevice AliceProtocolDevice { get; private set; }
        public BobProtocolDevice BobProtocolDevice { get; private set; }
        public ProtocolParams Params { get; private set; }

        public event EventHandler<SimpleProtocolEventArgs> Finished = (sender, args) => { };

        public void Process()
        {
            AliceProtocolDevice.ProcessForward(new SimpleProtocolEventArgs(new QuantumState()));
        }

        void AliceDeviceForwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            BobProtocolDevice.ProcessForward(e);
        }

        void BobDeviceForwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            BobProtocolDevice.ProcessBackward(e);
        }

        void BobDevice_BackwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            AliceProtocolDevice.ProcessBackward(e);
        }

        void AliceDevice_BackwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            Finished(this, e);
        }

        public void Dispose()
        {
            AliceProtocolDevice.ForwardProcessFinished -= AliceDeviceForwardProcessFinished;
            BobProtocolDevice.ForwardProcessFinished -= BobDeviceForwardProcessFinished;
            BobProtocolDevice.BackwardProcessFinished -= BobDevice_BackwardProcessFinished;
            AliceProtocolDevice.BackwardProcessFinished -= AliceDevice_BackwardProcessFinished;
        }
    }
}
