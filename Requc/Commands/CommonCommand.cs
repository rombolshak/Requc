using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Requc.Helpers;

namespace Requc.Commands
{
    public abstract class CommonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = Actions.DoNothing;
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            protected set
            {
                _isExecuting = value;
                CanExecuteChanged(this, EventArgs.Empty);
                RaiseCanExecuteChanged();
            }
        }

        protected abstract void OnExecute(object parameter);

        public void Execute(object parameter)
        {
            OnExecute(parameter);
        }

        public virtual bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }
    }
}
