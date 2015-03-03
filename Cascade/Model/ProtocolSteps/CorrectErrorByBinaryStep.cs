using System.Collections.Generic;

namespace Cascade.Model.ProtocolSteps
{
    public class CorrectErrorByBinaryStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            yield return new SayAboutBinaryCorrectionStep();

            while (environment.BinaryEnvironment.PositionsCount > 1)
            {
                yield return new LookAtFirstHalfOfWorkingPositionsStep();
                yield return new CalculateWorkingPositionsParityStep();
                yield return new CompareWorkingPositionsParityStep();
                yield return new UpdateWorkingPositionsStep();
            }

            yield return new CorrectErrorInFoundPositionStep();
        }

        public string Description { get { return ""; } }
    }

    public class CorrectErrorInFoundPositionStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return ""; } }
    }

    public class UpdateWorkingPositionsStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return ""; } }
    }

    public class CompareWorkingPositionsParityStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Сравнение чётностей позиций"; } }
    }

    public class CalculateWorkingPositionsParityStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Вычисление чётностей позиций"; } }
    }

    public class LookAtFirstHalfOfWorkingPositionsStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return ""; } }
    }

    public class SayAboutBinaryCorrectionStep : IProtocolStep
    {
        public IEnumerable<IProtocolStep> Execute(CascadeProtocolRuntimeEnvironment environment)
        {
            return null;
        }

        public string Description { get { return "Исправление ошибки с помощью бинарного поиска"; } }
    }
}