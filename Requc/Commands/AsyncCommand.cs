using System;
using System.ComponentModel;
using System.Windows.Input;
using Requc.Helpers;

namespace Requc.Commands
{
    public abstract class AsyncCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = Actions.DoNothing;
        public event EventHandler RunWorkerStarting = Actions.DoNothing;
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        public abstract string Text { get; }
        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            private set
            {
                _isExecuting = value;
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        protected abstract void OnExecute(object parameter);

        public void Execute(object parameter)
        {
            try
            {
                OnRunWorkerStarting();

                var worker = new BackgroundWorker();
                worker.DoWork += ((sender, e) => OnExecute(e.Argument));
                worker.RunWorkerCompleted += ((sender, e) => OnRunWorkerCompleted(e));
                worker.RunWorkerAsync(parameter);
            }
            catch (Exception ex)
            {
                OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
            }
        }

        private void OnRunWorkerStarting()
        {
            IsExecuting = true;
            RunWorkerStarting(this, EventArgs.Empty);
        }

        private void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            IsExecuting = false;
            if (RunWorkerCompleted != null)
                RunWorkerCompleted(this, e);
        }

        public virtual bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }
    }
}
