using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class ShowCurrentPassStep : IProtocolStep
    {
        public ShowCurrentPassStep(int pass)
        {
            Description = "Проход №" + (pass + 1);
        }

        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get; private set; }
    }
}