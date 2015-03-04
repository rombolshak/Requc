using System.Collections.Generic;
using System.Linq;

namespace Cascade.Model.ProtocolSteps
{
    public class InitializeBinaryCorrectionStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            environment.BinaryEnvironment.StartPosition = 0;
            environment.BinaryEnvironment.PositionsCount = environment.BinaryEnvironment.WorkingBlock.Size;
            environment.BinaryEnvironment.SampleBlock =
                environment.AliceBlocks.SelectMany(list => list)
                           .Single(
                               block => block.Size == environment.BinaryEnvironment.WorkingBlock.Size &&
                                        block.KeyItems[0].Position ==
                                        environment.BinaryEnvironment.WorkingBlock.KeyItems[0].Position);
            foreach (var keyItem in environment.BinaryEnvironment.WorkingBlock.KeyItems)
            {
                keyItem.ErrorHere = true;
            }

            return null;
        }

        public string Description { get { return "Исправление ошибки с помощью бинарного поиска"; } }
    }
}