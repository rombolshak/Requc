using System.Threading;

namespace Cascade.Model
{
    public class ProtocolStepFinishedEventArgs
    {
        public IProtocolStep Step { get; set; }

        public ProtocolStepFinishedEventArgs(IProtocolStep step)
        {
            Step = step;
            Handle = new AutoResetEvent(false);
        }

        public AutoResetEvent Handle { get; private set; }
    }
}