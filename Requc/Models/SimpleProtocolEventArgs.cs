using System;
using Requc.Commands;

namespace Requc.Models
{
    public class SimpleProtocolEventArgs : EventArgs
    {
        public SimpleProtocolEventArgs(TransmissionItem item, ModelingMode modelingMode)
        {
            Item = item;
            ModelingMode = modelingMode;
        }

        public ModelingMode ModelingMode { get; private set; }
        public TransmissionItem Item { get; private set; }
    }
}