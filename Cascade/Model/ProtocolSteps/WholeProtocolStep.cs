using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class WholeProtocolStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            yield return new ShowInitialErrorsStep();
            yield return new HideInitialErrorsStep();
            for (int i = 0; i < 4; ++i)
            {
                yield return new OnePassStep(i);
            }
        }

        public string Description
        {
            get { return ""; }
        }
    }
}
