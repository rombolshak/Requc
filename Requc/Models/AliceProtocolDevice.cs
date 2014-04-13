using Requc.Helpers;

namespace Requc.Models
{
    public class AliceProtocolDevice : ProtocolDevice
    {
        protected override void DoForwardProcess(SimpleProtocolEventArgs args)
        {
            Devices.Laser(args.Item);
            var topState = args.Item.QuantumState;
            var bottomState = QuantumState.Vacuum;
            Devices.BeamSplit(topState, bottomState);
            Devices.Delay(bottomState);
            Devices.BeamSplit(topState, bottomState);
        }

        protected override void DoBackwardProcess(SimpleProtocolEventArgs args)
        {
            var stateTop = args.Item.QuantumState;
            var stateBottom = QuantumState.Vacuum;
            Devices.BeamSplit(stateTop, stateBottom);
            args.Item.AliceValue = RandomHelper.RandomBool();
            Devices.PhaseShift(stateBottom, 0, args.Item.AlicePhase);
            Devices.Delay(args.Item.QuantumState);
            Devices.BeamSplit(stateTop, stateBottom);
            args.Item.QuantumState = stateBottom;
        }
    }
}
