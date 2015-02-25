using System.Threading;

namespace Cascade.Model
{
    public class ProtocolStepStartedEventArgs
    {
        public ProtocolStepStartedEventArgs(IProtocolStep step)
        {
            Step = step;
            Handle = new AutoResetEvent(false);
        }

        public IProtocolStep Step { get; private set; }
        public AutoResetEvent Handle { get; private set; }
    }
}