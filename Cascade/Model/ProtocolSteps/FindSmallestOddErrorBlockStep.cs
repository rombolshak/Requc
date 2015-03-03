using System.Collections.Generic;
using System.Linq;

namespace Cascade.Model.ProtocolSteps
{
    public class FindSmallestOddErrorBlockStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            environment.BinaryEnvironment.WorkingBlock = environment.OddErrorsCountBlocks.OrderBy(block => block.Size).First();
            return null;
        }

        public string Description { get { return "Поиск наименьшего блока"; } }
    }
}