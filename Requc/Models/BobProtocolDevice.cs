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
            args.QuantumState.Timeslot[0] = args.QuantumState.Timeslot[1] =
                                            (args.QuantumState.Timeslot[0] - ProtocolAct.Params.LaserPhotonNumberMin) /
                                            (ProtocolAct.Params.LaserPhotonNumberMax - ProtocolAct.Params.LaserPhotonNumberMin);
            args.BobPhase = RandomHelper.RandomBool() ? ProtocolAct.Params.Phase0 : ProtocolAct.Params.Phase1;
            Devices.PhaseShift(args.QuantumState, 1, args.BobPhase);
        }
    }
}
