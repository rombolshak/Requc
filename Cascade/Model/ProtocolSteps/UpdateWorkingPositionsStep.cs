using System.Collections.Generic;
using System.Linq;

namespace Cascade.Model.ProtocolSteps
{
    public class UpdateWorkingPositionsStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            var parityMatch = environment.BinaryEnvironment.SampleParity ==
                              environment.BinaryEnvironment.WorkingParity;

            environment.BinaryEnvironment.PositionsCount /= 2;
            foreach (
                var item in
                    environment.BinaryEnvironment.WorkingBlock.KeyItems.Skip(
                        environment.BinaryEnvironment.StartPosition +
                        (parityMatch ? 0 : (environment.BinaryEnvironment.PositionsCount)))
                               .Take(environment.BinaryEnvironment.PositionsCount))
            {
                item.ErrorHere = false;
            }

            if (parityMatch)
            {
                environment.BinaryEnvironment.StartPosition += environment.BinaryEnvironment.PositionsCount;
            }

            return null;
        }

        public string Description { get { return ""; } }
    }
}