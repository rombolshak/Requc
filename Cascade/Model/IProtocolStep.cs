using System;
using System.Collections.Generic;

namespace Cascade.Model
{
    public interface IProtocolStep
    {
        IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment);
        
        string Description { get; }
    }
}
