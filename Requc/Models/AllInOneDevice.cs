using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requc.Commands;
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
            Devices.Attenuator(args.Item.QuantumState, ProtocolAct.Params);
            args.Item.BobValue = RandomHelper.RandomBool();
            Devices.PhaseShift(args.Item.QuantumState, 1, args.Item.BobPhase);

            // eva
            var catched = args.ModelingMode == ModelingMode.Random
                              ? RandomHelper.RandomBool()
                              : (args.ModelingMode != ModelingMode.NoEvaWithClick &&
                                 args.ModelingMode != ModelingMode.NoEvaNoClick);
            args.Item.CatchedByEva = catched;
            if (catched)
            {
                var result = Devices.EvaMeasure(args.Item);
                args.Item.EvaResult = args.ModelingMode == ModelingMode.Random
                                          ? result
                                          : args.ModelingMode == ModelingMode.EvaUndefinedResult
                                                ? MeasurementResult.Inconclusive
                                                : args.ModelingMode == ModelingMode.EvaDefinedResultNoClick
                                                      ? args.Item.BobValue
                                                            ? MeasurementResult.Phase1
                                                            : MeasurementResult.Phase0
                                                      : args.Item.BobValue
                                                            ? MeasurementResult.Phase0
                                                            : MeasurementResult.Phase1;
                switch (args.Item.EvaResult)
                {
                    case MeasurementResult.Phase0:
                        args.Item.EvaValue = false;
                        break;
                    case MeasurementResult.Phase1:
                        args.Item.EvaValue = true;
                        break;
                    case MeasurementResult.Inconclusive: // transmission will be blocked
                        args.Item.EvaValue = RandomHelper.RandomBool();
                        break;
                }
            }

            // alice backward
            var stateTop = args.Item.QuantumState;
            var stateBottom = QuantumState.Vacuum;
            Devices.BeamSplit(stateTop, stateBottom);
            args.Item.AliceValue = args.ModelingMode == ModelingMode.Random
                                       ? RandomHelper.RandomBool()
                                       : args.ModelingMode == ModelingMode.NoEvaWithClick
                                             ? !args.Item.BobValue
                                             : args.Item.BobValue;
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
