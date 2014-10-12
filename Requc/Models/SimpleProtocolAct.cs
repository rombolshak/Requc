using System;
using Requc.Commands;

namespace Requc.Models
{
    public class SimpleProtocolAct : IDisposable
    {
        public SimpleProtocolAct()
        {
            Params = new ProtocolParams(100, 200, Math.PI/8, 3 * Math.PI/8);
            AllInOneDevice = new AllInOneDevice {ProtocolAct = this};
            AllInOneDevice.ForwardProcessFinished += AllInOneDevice_ForwardProcessFinished;
            AllInOneDevice.BackwardProcessFinished += AllInOneDevice_BackwardProcessFinished;
        }

        void AllInOneDevice_ForwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            AllInOneDevice.ProcessBackward(e);
        }

        void AllInOneDevice_BackwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            Finished(this, e);
        }

        public AllInOneDevice AllInOneDevice { get; private set; }
        public ProtocolParams Params { get; private set; }

        public event EventHandler<SimpleProtocolEventArgs> Started = (sender, args) => { };
        public event EventHandler<SimpleProtocolEventArgs> Finished = (sender, args) => { };

        public TransmissionItem Process(ModelingMode modelingMode)
        {
            _transmissionItem = new TransmissionItem(Params.Phase0, Params.Phase1);
            var e = new SimpleProtocolEventArgs(_transmissionItem, modelingMode);
            AllInOneDevice.ProcessForward(e);
            Started(this, e);
            return _transmissionItem;
        }

        public void Dispose()
        {
            AllInOneDevice.ForwardProcessFinished -= AllInOneDevice_ForwardProcessFinished;
            AllInOneDevice.BackwardProcessFinished -= AllInOneDevice_BackwardProcessFinished;
        }

        private TransmissionItem _transmissionItem;
    }
}
