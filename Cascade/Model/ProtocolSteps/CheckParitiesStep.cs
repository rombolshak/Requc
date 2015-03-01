using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class CheckParitiesStep : IProtocolStep
    {
        public int Pass { get; private set; }

        public CheckParitiesStep(int pass)
        {
            Pass = pass;
        }

        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Сверка четностей блоков"; } }
    }
}