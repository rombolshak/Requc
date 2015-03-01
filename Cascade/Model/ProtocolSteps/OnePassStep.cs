using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class OnePassStep : IProtocolStep
    {
        public OnePassStep(int pass)
        {
            Pass = pass;
        }

        public string Description
        {
            get { return ""; }
        }

        public int Pass { get; private set; }

        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            yield return new ShowCurrentPassStep(Pass);
            yield return new FillBlocksWithRandomPermutationStep(Pass);
            yield return new CalculateParitiesStep(Pass);
            yield return new CheckParitiesStep();
            yield return new SetOddErrorsBlocksStep();
            yield return new ProcessOddErrorsBlocksStep();
        }
    }
}