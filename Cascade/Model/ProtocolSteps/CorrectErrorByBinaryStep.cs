using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class CorrectErrorByBinaryStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Исправление ошибки с помощью бинарного поиска"; } }
    }
}