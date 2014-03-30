using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Requc.Models.Devices;

namespace Requc.Models
{
    public class DeviceColumn
    {
        public DeviceColumn(IDevice top, IDevice bottom)
        {
            Top = top;
            Bottom = bottom ?? new NullDevice();
            IsBeamSplitter = top.GetType() == Type.GetType("BeamSplitterDevice");
        }

        public IDevice Top { get; private set; }
        public IDevice Bottom { get; private set; }
        public bool IsBeamSplitter { get; private set; }
        
        public void Process()
        {
            Top.Process();
            Bottom.Process();
        }
    }
}
