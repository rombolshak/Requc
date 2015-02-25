using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class SetOddErrorsBlocksStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description
        {
            get { return "Обновление множества блоков с нечетным числом ошибок"; }
        }
    }
}