using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Requc.Helpers;
using Requc.Models;

namespace Requc.ViewModels
{
    public class TransmissionActViewModel : BaseViewModel
    {
        public TransmissionActViewModel()
        {
            _canNextStepExecute = true;
            _canAllStepsExecute = true;
        }

        public TransmissionActScheme TransmissionActScheme { get; set; }

        public ICommand NextStepCommand
        {
            get 
            {
                return _nextStepCommand ??
                       (_nextStepCommand = new DelegateCommand(RunNextStep, () => CanNextStepExecute));
            }
        }

        private void RunNextStep()
        {
            CanNextStepExecute = false;
            TransmissionActScheme.StepCompleted += RunNextStepCompleted;
            TransmissionActScheme.NextStep();
        }

        private void RunNextStepCompleted(object sender, EventArgs eventArgs)
        {
            TransmissionActScheme.StepCompleted -= RunNextStepCompleted;
            CanNextStepExecute = true;
        }

        private bool CanNextStepExecute
        {
            get { return _canNextStepExecute; }
            set
            {
                _canNextStepExecute = value;
                RaisePropertyChanged(() => CanNextStepExecute);
                _nextStepCommand.RaiseCanExecuteChanged();
            }
        }


        public ICommand RunAllStepsCommand
        {
            get
            {
                return _allStepsCommand ??
                       (_allStepsCommand = new DelegateCommand(RunAllSteps, () => CanAllStepsExecute));
            }
        }

        private bool CanAllStepsExecute
        {
            get { return _canAllStepsExecute; }
            set
            {
                _canAllStepsExecute = value; 
                RaisePropertyChanged(() => CanAllStepsExecute);
                _allStepsCommand.RaiseCanExecuteChanged();
            }
        }

        private void RunAllSteps()
        {
            CanNextStepExecute = false;
            CanAllStepsExecute = false;
            TransmissionActScheme.StepCompleted += StepCompleted;
            TransmissionActScheme.ActCompleted += AllStepsCompleted;
            TransmissionActScheme.NextStep();
        }

        private void AllStepsCompleted(object sender, EventArgs e)
        {
            TransmissionActScheme.StepCompleted -= StepCompleted;
            TransmissionActScheme.ActCompleted -= AllStepsCompleted;
            CanNextStepExecute = true;
            CanAllStepsExecute = true;
        }

        private void StepCompleted(object sender, EventArgs e)
        {
            try
            {
                TransmissionActScheme.NextStep();
            }
            catch (IndexOutOfRangeException)
            {//race was here
            }
        }


        private DelegateCommand _nextStepCommand;
        private DelegateCommand _allStepsCommand;
        private bool _canNextStepExecute;
        private bool _canAllStepsExecute;
    }
}