using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Helpers;
using Requc.Models;

namespace Requc.Models
{
    public class BobProtocolDevice : ProtocolDevice
    {
        protected override void DoForwardProcess(SimpleProtocolEventArgs args)
        {
        }

        protected override void DoBackwardProcess(SimpleProtocolEventArgs args)
        {
            args.Item.QuantumState.Timeslot[0] = args.Item.QuantumState.Timeslot[1] =
                                            (args.Item.QuantumState.Timeslot[0] - ProtocolAct.Params.LaserPhotonNumberMin) /
                                            (ProtocolAct.Params.LaserPhotonNumberMax - ProtocolAct.Params.LaserPhotonNumberMin);
            args.Item.BobValue = RandomHelper.RandomBool();
            Devices.PhaseShift(args.Item.QuantumState, 1, args.Item.BobPhase);
        }
    }
}
