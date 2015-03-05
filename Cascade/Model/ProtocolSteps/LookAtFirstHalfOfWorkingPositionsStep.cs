using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class LookAtFirstHalfOfWorkingPositionsStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return ""; } }
    }
}