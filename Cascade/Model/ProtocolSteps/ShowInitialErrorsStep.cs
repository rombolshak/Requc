using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class ShowInitialErrorsStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            for (var i = 0; i < environment.KeyLength; i++)
            {
                if (environment.AliceKey[i].Value != environment.BobKey[i].Value)
                {
                    environment.BobKey[i].ErrorHere = true;
                }
            }

            return null;
        }

        public string Description { get { return ""; } }
    }
}