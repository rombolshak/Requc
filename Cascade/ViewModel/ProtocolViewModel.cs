using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cascade.Helpers;
using Cascade.Model;
using Cascade.Model.ProtocolSteps;
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
                        Blocks = environment.AliceBlocks[i].Select(block => new BlockViewModel(block, AliceKey)).ToList()
                    };
            }

            BobBlocks = new BlockSetViewModel[numberOfPasses];
            for (var i = 0; i < numberOfPasses; ++i)
            {
                BobBlocks[i] = new BlockSetViewModel
                {
                    Blocks = environment.BobBlocks[i].Select(block => new BlockViewModel(block, BobKey)).ToList()
                };
            }
        }

        public string CurrentStepDescription
        {
            get { return _currentStepDescription; }
            set
            {
                _currentStepDescription = value;
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<KeyItemViewModel> AliceKey
        {
            get { return _aliceKeyViewModel; }
            private set
            {
                _aliceKeyViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<KeyItemViewModel> BobKey
        {
            get { return _bobKeyViewModel; }
            private set
            {
                _bobKeyViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public BlockSetViewModel[] AliceBlocks
        {
            get { return _aliceBlocks; }
            set
            {
                _aliceBlocks = value;
                NotifyPropertyChanged();
            }
        }

        public BlockSetViewModel[] BobBlocks
        {
            get { return _bobBlocks; }
            set
            {
                _bobBlocks = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand NextStepCommand { get; private set; }

        public RelayCommand StartProcessCommand { get; private set; }

        public event EventHandler<ProtocolStepStartedEventArgs> StepStarted;

        public event EventHandler<ProtocolStepFinishedEventArgs> StepFinished;

        private void RunnerOnStepStarted(object sender, ProtocolStepStartedEventArgs protocolStepStartedEventArgs)
        {
            CurrentStepDescriptionVisualState = "Invisible";
            TypeSwitch.Do(protocolStepStartedEventArgs.Step,
                          TypeSwitch.Case<FillBlocksWithRandomPermutationStep>(step =>
                              {
                                  AliceBlocks[step.Pass].State = BlockSetViewModel.VisualStateE.Invisible;
                                  BobBlocks[step.Pass].State = BlockSetViewModel.VisualStateE.Invisible;
                              }),
                          TypeSwitch.Case<CalculateParitiesStep>(step =>
                              {
                                  foreach (
                                      var blockViewModel in
                                          AliceBlocks[step.Pass].Blocks.Concat(BobBlocks[step.Pass].Blocks))
                                  {
                                      blockViewModel.State = BlockViewModel.VisualStateE.NothingVisible;
                                  }
                              }),
                          TypeSwitch.Case<FindSmallestOddErrorBlockStep>(step =>
                              {
                                  foreach (var block in Enumerable.Range(0, 4)
                                                                  .SelectMany(i => BobBlocks[i].Blocks)
                                                                  .Where(
                                                                      block =>
                                                                      block.State ==
                                                                      BlockViewModel.VisualStateE.OddError))
                                  {
                                      block.State = BlockViewModel.VisualStateE.OddErrorNotSelected;
                                  }
                              }));

            StateManager.WaitAnimations();
            protocolStepStartedEventArgs.Handle.Set();
            // OnStepStarted(protocolStepStartedEventArgs);
        }

        private void RunnerOnStepFinished(object sender, ProtocolStepFinishedEventArgs protocolStepFinishedEventArgs)
        {
            // OnStepFinished(protocolStepFinishedEventArgs);
            CurrentStepDescription = protocolStepFinishedEventArgs.Step.Description;
            CurrentStepDescriptionVisualState = "Visible";
            TypeSwitch.Do(protocolStepFinishedEventArgs.Step,
                          TypeSwitch.Case<FillBlocksWithRandomPermutationStep>(step =>
                              {
                                  AliceBlocks[step.Pass].State = BlockSetViewModel.VisualStateE.Visible;
                                  BobBlocks[step.Pass].State = BlockSetViewModel.VisualStateE.Visible;
                              }),
                          TypeSwitch.Case<CalculateParitiesStep>(step =>
                              {
                                  foreach (
                                      var blockViewModel in
                                          AliceBlocks[step.Pass].Blocks.Concat(BobBlocks[step.Pass].Blocks))
                                  {
                                      blockViewModel.State = BlockViewModel.VisualStateE.ParityVisible;
                                  }
                              }),
                          TypeSwitch.Case<CheckParitiesStep>(step =>
                              {
                                  for (var i = 0; i < _environment.BlocksCount[step.Pass]; ++i)
                                  {
                                      if (AliceBlocks[step.Pass].Blocks[i].Parity ==
                                          BobBlocks[step.Pass].Blocks[i].Parity)
                                      {
                                          AliceBlocks[step.Pass].Blocks[i].State =
                                              BobBlocks[step.Pass].Blocks[i].State =
                                              BlockViewModel.VisualStateE.ParityMatched;
                                      }
                                      else
                                      {
                                          AliceBlocks[step.Pass].Blocks[i].State =
                                              BobBlocks[step.Pass].Blocks[i].State =
                                              BlockViewModel.VisualStateE.ParityNotMatched;
                                      }
                                  }
                              }),
                          TypeSwitch.Case<SetOddErrorsBlocksStep>(step =>
                              {
                                  foreach (var block in GetAllBlocks(BobBlocks))
                                  {
                                      block.State = _environment.OddErrorsCountBlocks.Contains(block.Model)
                                                        ? BlockViewModel.VisualStateE.OddError
                                                        : BlockViewModel.VisualStateE.Normal;
                                  }

                                  foreach (var block in GetAllBlocks(AliceBlocks))
                                  {
                                      block.State = BlockViewModel.VisualStateE.Normal;
                                  }
                              }),
                          TypeSwitch.Case<FindSmallestOddErrorBlockStep>(step =>
                              {
                                  foreach (
                                      var block in
                                          GetAllBlocks(BobBlocks)
                                              .Where(block => block.Model == _environment.WorkingBlock))
                                  {
                                      block.State = BlockViewModel.VisualStateE.OddErrorSelected;
                                  }
                              }));

            StateManager.WaitAnimations();
            protocolStepFinishedEventArgs.Handle.Set();
            Thread.Sleep(1000);
            _runner.NextStep();
        }

        private IEnumerable<BlockViewModel> GetAllBlocks(BlockSetViewModel[] blocks)
        {
            return Enumerable.Range(0, 4).SelectMany(i => blocks[i].Blocks);
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
