﻿using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class CheckParitiesStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Сверка четностей блоков"; } }
    }
}