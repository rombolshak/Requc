using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class BackTrackCorrectedErrorStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Обновление множества блоков с нечетным числом ошибок"; } }
    }
}