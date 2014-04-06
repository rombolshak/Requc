using System;
using Requc.Helpers;

namespace Requc.Models
{
    public abstract class ProtocolDevice
    {
        protected abstract void DoForwardProcess(SimpleProtocolEventArgs args);
        protected abstract void DoBackwardProcess(SimpleProtocolEventArgs args);

        public event EventHandler<SimpleProtocolEventArgs> ForwardProcessFinished = (sender, args) => { }; 
        public event EventHandler<SimpleProtocolEventArgs> ForwardProcessStarted = (sender, args) => { };
        public event EventHandler<SimpleProtocolEventArgs> BackwardProcessFinished = (sender, args) => { };
        public event EventHandler<SimpleProtocolEventArgs> BackwardProcessStarted = (sender, args) => { };

        public void ProcessForward(SimpleProtocolEventArgs args)
        {
            _args = args;
            DoForwardProcess(args);
            ForwardProcessStarted(this, args);
        }

        public void ProcessBackward(SimpleProtocolEventArgs args)
        {
            _args = args;
            DoBackwardProcess(args);
            BackwardProcessStarted(this, args);
        }

        public void RequestForwardProcessFinish()
        {
            ForwardProcessFinished(this, _args);
        }

        public void RequestBackwardProcessFinish()
        {
            BackwardProcessFinished(this, _args);
        }

        public SimpleProtocol Protocol { get; set; }
        private SimpleProtocolEventArgs _args;
    }
}
