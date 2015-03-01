using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class SetOddErrorsBlocksStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(ProtocolRuntimeEnvironment environment)
        {
            environment.OddErrorsCountBlocks.Clear();
            for (var i = 0; i < environment.AliceBlocks.Count; ++i)
            {
                for (var j = 0; j < environment.AliceBlocks[i].Count; ++j)
                {
                    if (environment.AliceBlocks[i][j].Parity != environment.BobBlocks[i][j].Parity)
                    {
                        environment.OddErrorsCountBlocks.Add(environment.BobBlocks[i][j]);
                    }
                }
            }

            return null;
        }

        public string Description
        {
            get { return "Обновление множества блоков с нечетным числом ошибок"; }
        }
    }
}