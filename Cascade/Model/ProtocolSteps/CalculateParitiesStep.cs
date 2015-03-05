using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class CalculateParitiesStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            Description = "Вычисление чётностей блоков";
            return null;
        }

        public string Description { get; private set; }
    }
}