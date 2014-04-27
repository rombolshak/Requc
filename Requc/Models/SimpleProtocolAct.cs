using System;

namespace Requc.Models
{
    public class SimpleProtocolAct : IDisposable
    {
        public SimpleProtocolAct()
        {
            AliceProtocolDevice = new AliceProtocolDevice {ProtocolAct = this};
            BobProtocolDevice = new BobProtocolDevice {ProtocolAct = this};
            Params = new ProtocolParams(100, 200, Math.PI/8, Math.PI/4);

            AliceProtocolDevice.ForwardProcessFinished += AliceDeviceForwardProcessFinished;
            BobProtocolDevice.ForwardProcessFinished += BobDeviceForwardProcessFinished;
            BobProtocolDevice.BackwardProcessFinished += BobDevice_BackwardProcessFinished;
            AliceProtocolDevice.BackwardProcessFinished += AliceDevice_BackwardProcessFinished;
        }

        public AliceProtocolDevice AliceProtocolDevice { get; private set; }
        public BobProtocolDevice BobProtocolDevice { get; private set; }
        public ProtocolParams Params { get; private set; }

        public event EventHandler<SimpleProtocolEventArgs> Started = (sender, args) => { };
        public event EventHandler<SimpleProtocolEventArgs> Finished = (sender, args) => { };

        public TransmissionItem Process()
        {
            _transmissionItem = new TransmissionItem(Params.Phase0, Params.Phase1);
            var e = new SimpleProtocolEventArgs(_transmissionItem);
            Started(this, e);
            AliceProtocolDevice.ProcessForward(e);
            return _transmissionItem;
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

        private TransmissionItem _transmissionItem;
    }
}
