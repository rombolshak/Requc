using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class CorrectErrorByBinaryStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            yield return new InitializeBinaryCorrectionStep();

            while (environment.BinaryEnvironment.PositionsCount > 1)
            {
                yield return new LookAtFirstHalfOfWorkingPositionsStep();
                yield return new CalculateWorkingPositionsParityStep();
                yield return new CompareWorkingPositionsParityStep();
                yield return new UpdateWorkingPositionsStep();
            }

            yield return new CorrectErrorInFoundPositionStep();
        }

        public string Description { get { return ""; } }
    }
}