using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Requc.Helpers;

namespace Requc.Models
{
    public static class Devices
    {
        public static void BeamSplit(QuantumState topState, QuantumState bottomState)
        {
            var sqrt2 = Math.Sqrt(2);
            var sum = new ObservableCollection<Complex>(new[]
                {
                    (topState.Timeslot[0] + bottomState.Timeslot[0])/sqrt2,
                    (topState.Timeslot[1] + bottomState.Timeslot[1])/sqrt2,
                    (topState.Timeslot[2] + bottomState.Timeslot[2])/sqrt2
                });
            var sub = new ObservableCollection<Complex>(new[]
                {
                    (topState.Timeslot[0] - bottomState.Timeslot[0])/sqrt2,
                    (topState.Timeslot[1] - bottomState.Timeslot[1])/sqrt2,
                    (topState.Timeslot[2] - bottomState.Timeslot[2])/sqrt2
                });

            topState.Timeslot = sum;
            bottomState.Timeslot = sub;
        }

        public static void Delay(QuantumState state)
        {
            state.Timeslot[2] = state.Timeslot[1];
            state.Timeslot[1] = state.Timeslot[0];
            state.Timeslot[0] = 0;
        }

        public static void PhaseShift(QuantumState state, int timeslot, double phase)
        {
            state.Timeslot[timeslot] *= Complex.Exp(Complex.ImaginaryOne * phase);
        }

        public static void Laser(TransmissionItem transmissionItem)
        {
            transmissionItem.QuantumState = new QuantumState();
            var phase = RandomHelper.RandomNumber(0, 6, 3);
            transmissionItem.QuantumState.Timeslot[0] = 156*Complex.Exp(phase*Complex.ImaginaryOne);
        }

        public static void Attenuator(QuantumState state, ProtocolParams protocolParams)
        {
            state.Timeslot[0] = (state.Timeslot[0] - protocolParams.LaserPhotonNumberMin)/
                                (protocolParams.LaserPhotonNumberMax - protocolParams.LaserPhotonNumberMin);
            state.Timeslot[1] = (state.Timeslot[1] - protocolParams.LaserPhotonNumberMin)/
                                (protocolParams.LaserPhotonNumberMax - protocolParams.LaserPhotonNumberMin);
            state.Timeslot[2] = (state.Timeslot[2] - protocolParams.LaserPhotonNumberMin) /
                                (protocolParams.LaserPhotonNumberMax - protocolParams.LaserPhotonNumberMin);
        }

        public static MeasurementResult EvaMeasure(TransmissionItem transmissionItem)
        {
            var c = Math.Cos(transmissionItem.Phase0 - transmissionItem.Phase1);
            var conclusive = RandomHelper.RandomBool(c);
            if (conclusive)
            {
                return Math.Abs(transmissionItem.BobPhase - transmissionItem.Phase0) < 1e-6 ? MeasurementResult.Phase0 : MeasurementResult.Phase1;
            }
            return MeasurementResult.Inconclusive;
        }
    }
}
