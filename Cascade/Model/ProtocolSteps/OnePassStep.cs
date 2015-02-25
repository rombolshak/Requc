using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class OnePassStep : IProtocolStep
    {
        public OnePassStep(int pass)
        {
            _pass = pass;
        }

        public string Description
        {
            get { return "Проход №" + _pass; }
        }

        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            yield return new FillBlocksWithRandomPermutationStep();
            yield return new CheckParitiesStep();
            yield return new SetOddErrorsBlocksStep();
            yield return new ProcessOddErrorsBlocksStep();
        }

        private readonly int _pass;
    }
}