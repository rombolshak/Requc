using System;
using System.ComponentModel;
using System.Threading;
using Cascade.Model.ProtocolSteps;

namespace Cascade.Model
{
    public sealed class ProtocolRunner
    {
        public ProtocolRunner(CascadeProtocolRuntimeEnvironment environment)
        {
            _environment = environment;
            _stepHandle = new ManualResetEvent(false);
            _logger = new FileLogger("runner.log");
            _worker = new BackgroundWorker();
            _worker.DoWork += DoWorkerTask;
        }

        public event EventHandler<ProtocolStepStartedEventArgs> StepStarted;
        public event EventHandler<ProtocolStepFinishedEventArgs> StepFinished;

        public void Start()
        {
            _worker.RunWorkerAsync();
        }

        public void NextStep()
        {
            _stepHandle.Set();
        }

        private void OnStepStarted(IProtocolStep step)
        {
            _logger.Write("OnStepStarted " + step);
            var handler = StepStarted;
            if (handler == null) return;

            _logger.Write("Raising event, handler not null");
            var stepStartedEventArgs = new ProtocolStepStartedEventArgs(step);
            handler(this, stepStartedEventArgs);
            stepStartedEventArgs.Handle.WaitOne(2000);
        }

        private void OnStepFinished(IProtocolStep step)
        {
            _logger.Write("OnStepFinished " + step);
            var handler = StepFinished;
            if (handler == null) return;

            _logger.Write("Raising event, handler not null");
            var stepFinishedEventArgs = new ProtocolStepFinishedEventArgs(step);
            _logger.Write("args prepared");
            try
            {
                handler(this, stepFinishedEventArgs);
                _logger.Write("handler was called, waiting for animations");
            }
            catch (Exception exception)
            {
                _logger.Write(string.Format("Something went wrong: {0}", exception.Message));
            }
            finally
            {
                stepFinishedEventArgs.Handle.WaitOne(2000);
            }

            _logger.Write("finish processing of OnStepFinished");
        }

        private void DoWorkerTask(object sender, DoWorkEventArgs e)
        {
            _logger.Write("Start DoWorkerTask");
            DoStep(new WholeProtocolStep());
            _logger.Write("End DoWorkerTask");
        }

        private void DoStep(IProtocolStep step)
        {
            _logger.Write(string.Format("Start DoStep with step {0}", step));
            _stepHandle.WaitOne(1000);
            _logger.Write("Handle granted, executing step");
            OnStepStarted(step);
            foreach (var nextStep in step.Execute(_environment).OrEmptyIfNull())
            {
                _logger.Write(string.Format("Next step in execution chain is {0}", nextStep));
                DoStep(nextStep);
                _logger.Write(string.Format("{0} executed successfully", nextStep));
            }

            _stepHandle.Reset();
            OnStepFinished(step);
            _logger.Write(string.Format("End DoStep of step {0}", step));
        }

        private readonly BackgroundWorker _worker;
        private readonly CascadeProtocolRuntimeEnvironment _environment;
        private readonly ManualResetEvent _stepHandle;
        private readonly FileLogger _logger;
    }
}
