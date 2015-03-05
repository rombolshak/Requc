using System.Collections.Generic;
using System.Linq;

namespace Cascade.Model.ProtocolSteps
{
    public class BackTrackCorrectedErrorStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            var correctedItem = environment.BinaryEnvironment.WorkingBlock.KeyItems.Single(item => item.ErrorHere);
            correctedItem.ErrorHere = false;
            var position = correctedItem.Position;

            foreach (var block in Enumerable.Range(0,environment.Pass + 1).SelectMany(i => environment.BobBlocks[i]).Where(block => block.KeyItems.Any(item => item.Position == position)))
            {
                if (environment.OddErrorsCountBlocks.Contains(block))
                {
                    environment.OddErrorsCountBlocks.Remove(block);
                }
                else
                {
                    environment.OddErrorsCountBlocks.Add(block);
                }
            }

            return null;
        }

        public string Description { get { return "Обновление множества блоков с нечетным числом ошибок"; } }
    }
}