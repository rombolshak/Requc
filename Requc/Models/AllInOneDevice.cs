using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Helpers;

namespace Requc.Models
{
    public class AllInOneDevice : ProtocolDevice
    {
        protected override void DoForwardProcess(SimpleProtocolEventArgs args)
        {
            // alice forward
            Devices.Laser(args.Item);
            var topState = args.Item.QuantumState;
            var bottomState = QuantumState.Vacuum;
            Devices.BeamSplit(topState, bottomState);
            Devices.Delay(bottomState);
            Devices.BeamSplit(topState, bottomState);

            // bob
            args.Item.QuantumState.Timeslot[0] = args.Item.QuantumState.Timeslot[1] =
                                            (args.Item.QuantumState.Timeslot[0] - ProtocolAct.Params.LaserPhotonNumberMin) /
                                            (ProtocolAct.Params.LaserPhotonNumberMax - ProtocolAct.Params.LaserPhotonNumberMin);
            args.Item.BobValue = RandomHelper.RandomBool();
            Devices.PhaseShift(args.Item.QuantumState, 1, args.Item.BobPhase);

            // alice backward
            var stateTop = args.Item.QuantumState;
            var stateBottom = QuantumState.Vacuum;
            Devices.BeamSplit(stateTop, stateBottom);
            args.Item.AliceValue = RandomHelper.RandomBool();
            Devices.PhaseShift(stateBottom, 0, args.Item.AlicePhase);
            Devices.Delay(args.Item.QuantumState);
            Devices.BeamSplit(stateTop, stateBottom);
            args.Item.QuantumState = stateBottom;
        }

        protected override void DoBackwardProcess(SimpleProtocolEventArgs args)
        {
        }
    }
}
