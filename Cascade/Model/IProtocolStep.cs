using System.Collections.Generic;

namespace Cascade.Model
{
    public interface IProtocolStep
    {
        IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment);
        
        string Description { get; }
    }
}
