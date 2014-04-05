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

            AliceDevice.ProcessFinished += AliceDevice_ProcessFinished;
            BobDevice.ProcessFinished += BobDevice_ProcessFinished;
        }

        public AliceDevice AliceDevice { get; private set; }
        public BobDevice BobDevice { get; private set; }

        public event EventHandler Finished = Actions.DoNothing;

        public void Process()
        {
            AliceDevice.Process();
        }

        void AliceDevice_ProcessFinished(object sender, EventArgs e)
        {
            BobDevice.Process();
        }

        void BobDevice_ProcessFinished(object sender, EventArgs e)
        {
            Finished(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            AliceDevice.ProcessFinished -= AliceDevice_ProcessFinished;
            BobDevice.ProcessFinished -= BobDevice_ProcessFinished;
        }
    }
}
