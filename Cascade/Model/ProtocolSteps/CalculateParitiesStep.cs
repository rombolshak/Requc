using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class CalculateParitiesStep : IProtocolStep
    {
        public int Pass { get; set; }

        public CalculateParitiesStep(int pass)
        {
            Pass = pass;
        }

        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            Description = "Вычисление чётностей блоков";
            return null;
        }

        public string Description { get; private set; }
    }
}