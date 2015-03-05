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
            get { return ""; }
        }

        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            environment.Pass = _pass;
            yield return new ShowCurrentPassStep(_pass);
            yield return new FillBlocksWithRandomPermutationStep();
            yield return new CalculateParitiesStep();
            yield return new CheckParitiesStep();
            yield return new SetOddErrorsBlocksStep();
            yield return new ProcessOddErrorsBlocksStep();
        }

        private readonly int _pass;
    }
}