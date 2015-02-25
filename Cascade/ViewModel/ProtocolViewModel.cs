using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cascade.Helpers;
using Cascade.Model;
using FirstFloor.ModernUI.Presentation;

namespace Cascade.ViewModel
{
    public class ProtocolViewModel : ViewModelBase
    {
        public ProtocolViewModel(ProtocolRunner runner, ProtocolRuntimeEnvironment environment)
        {
            _runner = runner;
            _environment = environment;
            NextStepCommand = new RelayCommand(_ => _runner.NextStep());
            StartProcessCommand = new RelayCommand(_ => { _runner.Start(); _runner.NextStep(); });

            _runner.StepStarted += RunnerOnStepStarted;
            _runner.StepFinished += RunnerOnStepFinished;

            AliceKey = environment.AliceKey.Select(item => new KeyItemViewModel(item));
            BobKey = environment.BobKey.Select(item => new KeyItemViewModel(item));

            const int numberOfPasses = 4;
            AliceBlocks = new BlockSetViewModel[numberOfPasses];
            for (var i = 0; i < numberOfPasses; ++i)
            {
                AliceBlocks[i] = new BlockSetViewModel
                    {
                        Blocks = environment.AliceBlocks[i].Select(block => new BlockViewModel(block, AliceKey))
                    };
            }

            BobBlocks = new BlockSetViewModel[numberOfPasses];
            for (var i = 0; i < numberOfPasses; ++i)
            {
                BobBlocks[i] = new BlockSetViewModel
                {
                    Blocks = environment.BobBlocks[i].Select(block => new BlockViewModel(block, BobKey))
                };
            }
        }

        public string CurrentStepDescription
        {
            get { return _currentStepDescription; }
            set
            {
                _currentStepDescription = value;
                NotifyPropertyChanged("CurrentStepDescription");
            }
        }

        public string CurrentStepDescriptionVisualState
        {
            get { return _currentStepDescriptionVisualState; }
            set
            {
                if (_currentStepDescriptionVisualState == value)
                {
                    return;
                }

                _currentStepDescriptionVisualState = value;
                NotifyPropertyChanged("CurrentStepDescriptionVisualState");
            }
        }

        public IEnumerable<KeyItemViewModel> AliceKey
        {
            get { return _aliceKeyViewModel; }
            private set
            {
                _aliceKeyViewModel = value;
                NotifyPropertyChanged("AliceKey");
            }
        }

        public IEnumerable<KeyItemViewModel> BobKey
        {
            get { return _bobKeyViewModel; }
            private set
            {
                _bobKeyViewModel = value;
                NotifyPropertyChanged("BobKey");
            }
        }

        public BlockSetViewModel[] AliceBlocks
        {
            get { return _aliceBlocks; }
            set
            {
                _aliceBlocks = value;
                NotifyPropertyChanged("AliceBlocks");
            }
        }

        public BlockSetViewModel[] BobBlocks
        {
            get { return _bobBlocks; }
            set
            {
                _bobBlocks = value;
                NotifyPropertyChanged("BobBlocks");
            }
        }

        public RelayCommand NextStepCommand { get; private set; }

        public RelayCommand StartProcessCommand { get; private set; }

        public event EventHandler<ProtocolStepStartedEventArgs> StepStarted;

        public event EventHandler<ProtocolStepFinishedEventArgs> StepFinished;

        private void RunnerOnStepStarted(object sender, ProtocolStepStartedEventArgs protocolStepStartedEventArgs)
        {
            CurrentStepDescriptionVisualState = "Invisible";
            StateManager.WaitAnimations();
            protocolStepStartedEventArgs.Handle.Set();
            // OnStepStarted(protocolStepStartedEventArgs);
        }

        private void RunnerOnStepFinished(object sender, ProtocolStepFinishedEventArgs protocolStepFinishedEventArgs)
        {
            // OnStepFinished(protocolStepFinishedEventArgs);
            CurrentStepDescription = protocolStepFinishedEventArgs.Step.Description;
            CurrentStepDescriptionVisualState = "Visible";
            StateManager.WaitAnimations();
            protocolStepFinishedEventArgs.Handle.Set();
            _runner.NextStep();
        }

        protected virtual void OnStepStarted(ProtocolStepStartedEventArgs e)
        {
            var handler = StepStarted;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnStepFinished(ProtocolStepFinishedEventArgs e)
        {
            var handler = StepFinished;
            if (handler != null) handler(this, e);
        }

        private readonly ProtocolRunner _runner;
        private readonly ProtocolRuntimeEnvironment _environment;
        private string _currentStepDescription;
        private IEnumerable<KeyItemViewModel> _aliceKeyViewModel;
        private IEnumerable<KeyItemViewModel> _bobKeyViewModel;
        private BlockSetViewModel[] _aliceBlocks;
        private BlockSetViewModel[] _bobBlocks;
        private string _currentStepDescriptionVisualState;
    }
}
