using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class ProcessOddErrorsBlocksStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            while (OddErrorsBlocksSetNotEmpty(environment))
            {
                yield return new FindSmallestOddErrorBlockStep();
                yield return new CorrectErrorByBinaryStep();
                yield return new BackTrackCorrectedErrorStep();
            }
        }

        private bool OddErrorsBlocksSetNotEmpty(ProtocolRuntimeEnvironment environment)
        {
            return false;
        }

        public string Description { get { return "Просмотр блоков с нечетным количеством ошибок"; } }
    }
}