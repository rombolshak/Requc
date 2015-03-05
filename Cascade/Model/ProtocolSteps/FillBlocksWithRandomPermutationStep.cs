using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cascade.Model.ProtocolSteps
{
    public class FillBlocksWithRandomPermutationStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Выполнение случайной перестановки"; } }
    }
}