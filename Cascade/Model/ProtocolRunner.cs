using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cascade.Model.ProtocolSteps;

namespace Cascade.Model
{
    public class ProtocolRunner
    {
        public ProtocolRunner(ProtocolRuntimeEnvironment environment)
        {
            _environment = environment;
            _stepHandle = new ManualResetEvent(false);
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

        protected virtual void OnStepStarted(IProtocolStep step)
        {
            var handler = StepStarted;
            if (handler == null) return;
            
            var stepStartedEventArgs = new ProtocolStepStartedEventArgs(step);
            handler(this, stepStartedEventArgs);
            stepStartedEventArgs.Handle.WaitOne();
        }

        protected virtual void OnStepFinished(IProtocolStep step)
        {
            var handler = StepFinished;
            if (handler == null) return;
            
            var stepFinishedEventArgs = new ProtocolStepFinishedEventArgs(step);
            handler(this, stepFinishedEventArgs);
            stepFinishedEventArgs.Handle.WaitOne();
        }

        private void DoWorkerTask(object sender, DoWorkEventArgs e)
        {
            DoStep(new WholeProtocolStep());
        }

        private void DoStep(IProtocolStep step)
        {
            _stepHandle.WaitOne();
            OnStepStarted(step);
            foreach (var nextStep in step.Execute(_environment).OrEmptyIfNull())
            {
                DoStep(nextStep);
            }

            _stepHandle.Reset();
            OnStepFinished(step);
        }

        private readonly BackgroundWorker _worker;
        private readonly ProtocolRuntimeEnvironment _environment;
        private readonly ManualResetEvent _stepHandle;
    }
}
