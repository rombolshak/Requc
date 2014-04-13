using System;

namespace Requc.Models
{
    public class SimpleProtocolEventArgs : EventArgs
    {
        public SimpleProtocolEventArgs(TransmissionItem item)
        {
            Item = item;
        }

        public TransmissionItem Item { get; private set; }
    }
}