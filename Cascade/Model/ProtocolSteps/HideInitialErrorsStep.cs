using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class HideInitialErrorsStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            foreach (var keyItem in environment.BobKey)
            {
                keyItem.ErrorHere = false;
            }

            return null;
        }

        public string Description { get { return ""; } }
    }
}