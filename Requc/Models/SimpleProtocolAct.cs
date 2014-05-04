using System;

namespace Requc.Models
{
    public class SimpleProtocolAct : IDisposable
    {
        public SimpleProtocolAct()
        {
            /*AliceProtocolDevice = new AliceProtocolDevice {ProtocolAct = this};
            BobProtocolDevice = new BobProtocolDevice {ProtocolAct = this};
            EnvironmentDevice = new TransmissionEnvironmentDevice {ProtocolAct = this};*/
            Params = new ProtocolParams(100, 200, Math.PI/8, Math.PI/4);
            AllInOneDevice = new AllInOneDevice {ProtocolAct = this};
            AllInOneDevice.ForwardProcessFinished += AllInOneDevice_ForwardProcessFinished;
            AllInOneDevice.BackwardProcessFinished += AllInOneDevice_BackwardProcessFinished;
            /*AliceProtocolDevice.ForwardProcessFinished += AliceDeviceForwardProcessFinished;
            EnvironmentDevice.ForwardProcessFinished += EnvironmentDevice_ForwardProcessFinished;
            BobProtocolDevice.ForwardProcessFinished += BobDeviceForwardProcessFinished;
            BobProtocolDevice.BackwardProcessFinished += BobDevice_BackwardProcessFinished;
            EnvironmentDevice.BackwardProcessFinished += EnvironmentDevice_BackwardProcessFinished;
            AliceProtocolDevice.BackwardProcessFinished += AliceDevice_BackwardProcessFinished;*/
        }

        void AllInOneDevice_ForwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            AllInOneDevice.ProcessBackward(e);
        }

        void AllInOneDevice_BackwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            Finished(this, e);
        }

        /*public AliceProtocolDevice AliceProtocolDevice { get; private set; }
        public BobProtocolDevice BobProtocolDevice { get; private set; }
        public TransmissionEnvironmentDevice EnvironmentDevice { get; private set; }*/
        public AllInOneDevice AllInOneDevice { get; private set; }
        public ProtocolParams Params { get; private set; }

        public event EventHandler<SimpleProtocolEventArgs> Started = (sender, args) => { };
        public event EventHandler<SimpleProtocolEventArgs> Finished = (sender, args) => { };

        public TransmissionItem Process()
        {
            _transmissionItem = new TransmissionItem(Params.Phase0, Params.Phase1);
            var e = new SimpleProtocolEventArgs(_transmissionItem);
            Started(this, e);
            AllInOneDevice.ProcessForward(e);
            //AliceProtocolDevice.ProcessForward(e);
            return _transmissionItem;
        }
/*

        void AliceDeviceForwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            EnvironmentDevice.ProcessForward(e);
        }

        void EnvironmentDevice_ForwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            BobProtocolDevice.ProcessForward(e);
        }

        void BobDeviceForwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            BobProtocolDevice.ProcessBackward(e);
        }

        void BobDevice_BackwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            EnvironmentDevice.ProcessBackward(e);
        }

        void EnvironmentDevice_BackwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            AliceProtocolDevice.ProcessBackward(e);
        }

        void AliceDevice_BackwardProcessFinished(object sender, SimpleProtocolEventArgs e)
        {
            Finished(this, e);
        }
*/

        public void Dispose()
        {
            /*AliceProtocolDevice.ForwardProcessFinished -= AliceDeviceForwardProcessFinished;
            EnvironmentDevice.ForwardProcessFinished -= EnvironmentDevice_ForwardProcessFinished;
            BobProtocolDevice.ForwardProcessFinished -= BobDeviceForwardProcessFinished;
            BobProtocolDevice.BackwardProcessFinished -= BobDevice_BackwardProcessFinished;
            EnvironmentDevice.BackwardProcessFinished -= EnvironmentDevice_BackwardProcessFinished;
            AliceProtocolDevice.BackwardProcessFinished -= AliceDevice_BackwardProcessFinished;*/

            AllInOneDevice.ForwardProcessFinished -= AllInOneDevice_ForwardProcessFinished;
            AllInOneDevice.BackwardProcessFinished -= AllInOneDevice_BackwardProcessFinished;
        }

        private TransmissionItem _transmissionItem;
    }
}
