using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requc.Models.Devices
{
    public class NullDevice : Device
    {
        protected override void DoProcess()
        {
            RequestProcessFinish();
        }
    }
}
