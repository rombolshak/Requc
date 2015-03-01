using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cascade.Model.ProtocolSteps
{
    public class FillBlocksWithRandomPermutationStep : IProtocolStep
    {
        public FillBlocksWithRandomPermutationStep(int pass)
        {
            Pass = pass;
        }

        public int Pass { get; private set; }

        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Выполнение случайной перестановки"; } }
    }
}