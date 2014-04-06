using System;

namespace Requc.Models
{
    public class SimpleProtocolEventArgs : EventArgs
    {
        public SimpleProtocolEventArgs(QuantumState quantumState, double phase0, double phase1)
        {
            Phase1 = phase1;
            Phase0 = phase0;
            QuantumState = quantumState;
        }

        public QuantumState QuantumState { get; set; }
        public double AlicePhase { get; set; }
        public double BobPhase { get; set; }

        public double Phase0 { get; private set; }
        public double Phase1 { get; private set; }
    }
}