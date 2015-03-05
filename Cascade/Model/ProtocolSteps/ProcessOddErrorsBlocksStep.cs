using System.Collections.Generic;
using System.Linq;

namespace Cascade.Model.ProtocolSteps
{
    public class ProcessOddErrorsBlocksStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            while (OddErrorsBlocksSetNotEmpty(environment))
            {
                yield return new FindSmallestOddErrorBlockStep();
                yield return new CorrectErrorByBinaryStep();
                yield return new BackTrackCorrectedErrorStep();
            }
        }

        private bool OddErrorsBlocksSetNotEmpty(CascadeProtocolRuntimeEnvironment environment)
        {
            return environment.OddErrorsCountBlocks.Any();
        }

        public string Description { get { return "Просмотр блоков с нечетным количеством ошибок"; } }
    }
}