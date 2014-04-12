using Requc.Helpers;

namespace Requc.Models
{
    public class AliceProtocolDevice : ProtocolDevice
    {
        protected override void DoForwardProcess(SimpleProtocolEventArgs args)
        {
            // laser
            args.QuantumState.Timeslot[0] = RandomHelper.RandomNumber(ProtocolAct.Params.LaserPhotonNumberMin, ProtocolAct.Params.LaserPhotonNumberMax, 0);

            // beamsplitter + delay
            args.QuantumState.Timeslot[1] = (args.QuantumState.Timeslot[0] /= 2);
        }

        protected override void DoBackwardProcess(SimpleProtocolEventArgs args)
        {
            var stateTop = args.QuantumState;
            var stateBottom = QuantumState.Vacuum;
            Devices.BeamSplit(stateTop, stateBottom);
            args.AlicePhase = RandomHelper.RandomBool() ? ProtocolAct.Params.Phase0 : ProtocolAct.Params.Phase1;
            Devices.PhaseShift(stateBottom, 0, args.AlicePhase);
            Devices.Delay(args.QuantumState);
            Devices.BeamSplit(stateTop, stateBottom);
            args.QuantumState = stateBottom;
        }
    }
}
