using System;

namespace Requc.Models
{
    public class SimpleProtocolEventArgs : EventArgs
    {
        public SimpleProtocolEventArgs(QuantumState quantumState)
        {
            QuantumState = quantumState;
        }

        public QuantumState QuantumState { get; set; }
        public double AlicePhase { get; set; }
        public double BobPhase { get; set; }
    }
}