﻿using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class FindSmallestOddErrorBlockStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Поиск наименьшего блока"; } }
    }
}