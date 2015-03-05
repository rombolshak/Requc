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
        public ProtocolViewModel(ProtocolRunner runner, CascadeProtocolRuntimeEnvironment environment)
        {
            _runner = runner;
            _environment = environment;
            NextStepCommand = new RelayCommand(_ => _runner.NextStep());
            StartProcessCommand = new RelayCommand(_ => { _runner.Start(); _runner.NextStep(); });

            _runner.StepStarted += RunnerOnStepStarted;
            _runner.StepFinished += RunnerOnStepFinished;

            AliceKey = environment.AliceKey.Select(item => new KeyItemViewModel(item)).ToList();
            BobKey = environment.BobKey.Select(item => new KeyItemViewModel(item)).ToList();

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

        public IList<KeyItemViewModel> AliceKey
        {
            get { return _aliceKeyViewModel; }
            private set
            {
                _aliceKeyViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public IList<KeyItemViewModel> BobKey
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
                                  AliceBlocks[_environment.Pass].State = BlockSetViewModel.VisualStateE.Invisible;
                                  BobBlocks[_environment.Pass].State = BlockSetViewModel.VisualStateE.Invisible;
                              }),
                          TypeSwitch.Case<CalculateParitiesStep>(step =>
                              {
                                  foreach (
                                      var blockViewModel in
                                          AliceBlocks[_environment.Pass].Blocks.Concat(BobBlocks[_environment.Pass].Blocks))
                                  {
                                      blockViewModel.State = BlockViewModel.VisualStateE.NothingVisible;
                                  }
                              }),
                          TypeSwitch.Case<FindSmallestOddErrorBlockStep>(step =>
                              {
                                  foreach (var block in GetAllBlocks(BobBlocks)
                                      .Where(
                                          block =>
                                          block.State ==
                                          BlockViewModel.VisualStateE.OddError))
                                  {
                                      block.State = BlockViewModel.VisualStateE.OddErrorNotSelected;
                                  }
                              }),
                          TypeSwitch.Case<InitializeBinaryCorrectionStep>(step =>
                              {
                                  GetWorkingBlock().State = BlockViewModel.VisualStateE.Normal;
                              }),
                          TypeSwitch.Case<LookAtFirstHalfOfWorkingPositionsStep>(() =>
                              {
                                  GetWorkingBlock().State = BlockViewModel.VisualStateE.ParityTextInvisible;
                                  GetSampleBlock().State = BlockViewModel.VisualStateE.ParityTextInvisible;
                              }),
                              TypeSwitch.Case<CorrectErrorInFoundPositionStep>(() =>
                                  {
                                      GetWorkingBlock().State = BlockViewModel.VisualStateE.NothingVisible;
                                      GetSampleBlock().State = BlockViewModel.VisualStateE.NothingVisible;
                                      GetWorkingBlock().Items[_environment.BinaryEnvironment.StartPosition].State = KeyItemViewModel.VisualStateE.HighlightError;
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
                          TypeSwitch.Case<ShowInitialErrorsStep>(() =>
                              {
                                  foreach (var viewModel in BobKey.Where(model => model.ErrorHere))
                                  {
                                      viewModel.State = KeyItemViewModel.VisualStateE.Error;
                                  }
                              }),
                          TypeSwitch.Case<HideInitialErrorsStep>(() =>
                              {
                                  foreach (var viewModel in BobKey)
                                  {
                                      viewModel.State = KeyItemViewModel.VisualStateE.Normal;
                                  }
                              }),
                          TypeSwitch.Case<FillBlocksWithRandomPermutationStep>(step =>
                              {
                                  AliceBlocks[_environment.Pass].State = BlockSetViewModel.VisualStateE.Visible;
                                  BobBlocks[_environment.Pass].State = BlockSetViewModel.VisualStateE.Visible;
                              }),
                          TypeSwitch.Case<CalculateParitiesStep>(step =>
                              {
                                  foreach (
                                      var blockViewModel in
                                          AliceBlocks[_environment.Pass].Blocks.Concat(BobBlocks[_environment.Pass].Blocks))
                                  {
                                      blockViewModel.State = BlockViewModel.VisualStateE.ParityVisible;
                                  }
                              }),
                          TypeSwitch.Case<CheckParitiesStep>(step =>
                              {
                                  for (var i = 0; i < _environment.BlocksCount[_environment.Pass]; ++i)
                                  {
                                      if (AliceBlocks[_environment.Pass].Blocks[i].Parity ==
                                          BobBlocks[_environment.Pass].Blocks[i].Parity)
                                      {
                                          AliceBlocks[_environment.Pass].Blocks[i].State =
                                              BobBlocks[_environment.Pass].Blocks[i].State =
                                              BlockViewModel.VisualStateE.ParityMatched;
                                      }
                                      else
                                      {
                                          AliceBlocks[_environment.Pass].Blocks[i].State =
                                              BobBlocks[_environment.Pass].Blocks[i].State =
                                              BlockViewModel.VisualStateE.ParityNotMatched;
                                      }
                                  }
                              }),
                          TypeSwitch.Case<SetOddErrorsBlocksStep>(step =>
                              {
                                  UpdateOddErrorsBlocks();

                                  foreach (var block in GetAllBlocks(AliceBlocks))
                                  {
                                      block.State = BlockViewModel.VisualStateE.Normal;
                                  }
                              }),
                          TypeSwitch.Case<FindSmallestOddErrorBlockStep>(step =>
                              {
                                  var workingBlock = GetWorkingBlock();
                                  workingBlock.State = BlockViewModel.VisualStateE.OddErrorSelected;
                              }),
                          TypeSwitch.Case<InitializeBinaryCorrectionStep>(step =>
                              {
                                  foreach (var item in GetWorkingBlock().Items)
                                  {
                                      item.State = KeyItemViewModel.VisualStateE.Error;
                                  }
                              }),
                          TypeSwitch.Case<LookAtFirstHalfOfWorkingPositionsStep>(() =>
                              {
                                  ChangeVisualStateInBinaryProtocol(GetWorkingBlock());
                                  ChangeVisualStateInBinaryProtocol(GetSampleBlock());
                              }),
                          TypeSwitch.Case<CalculateWorkingPositionsParityStep>(() =>
                              {
                                  var workingBlock = GetWorkingBlock();
                                  var sampleBlock = GetSampleBlock();

                                  workingBlock.WorkingParity = _environment.BinaryEnvironment.WorkingParity;
                                  workingBlock.ShowWorkingParity = true;

                                  sampleBlock.WorkingParity = _environment.BinaryEnvironment.SampleParity;
                                  sampleBlock.ShowWorkingParity = true;

                                  workingBlock.State = BlockViewModel.VisualStateE.ParityTextVisible;
                                  sampleBlock.State = BlockViewModel.VisualStateE.ParityTextVisible;
                              }),
                          TypeSwitch.Case<CompareWorkingPositionsParityStep>(() =>
                              {
                                  var workingBlock = GetWorkingBlock();
                                  var sampleBlock = GetSampleBlock();
                                  if (_environment.BinaryEnvironment.SampleParity ==
                                      _environment.BinaryEnvironment.WorkingParity)
                                  {
                                      workingBlock.State =
                                          sampleBlock.State = BlockViewModel.VisualStateE.ParityMatched;
                                  }
                                  else
                                  {
                                      workingBlock.State =
                                          sampleBlock.State = BlockViewModel.VisualStateE.ParityNotMatched;
                                  }
                              }),
                              TypeSwitch.Case<UpdateWorkingPositionsStep>(() =>
                                  {
                                      foreach (
                                          var item in
                                              GetWorkingBlock()
                                                  .Items.Take(_environment.BinaryEnvironment.StartPosition)
                                                  .Concat(
                                                      GetWorkingBlock()
                                                          .Items.Skip(_environment.BinaryEnvironment.StartPosition +
                                                                      _environment.BinaryEnvironment.PositionsCount)))
                                      {
                                          item.State = KeyItemViewModel.VisualStateE.Normal;
                                      }

                                      GetWorkingBlock().State = BlockViewModel.VisualStateE.Normal;
                                      GetSampleBlock().State = BlockViewModel.VisualStateE.Normal;
                                  }),
                                  TypeSwitch.Case<CorrectErrorInFoundPositionStep>(() =>
                                      {
                                          var workingBlock = GetWorkingBlock();
                                          var sampleBlock = GetSampleBlock();

                                          workingBlock.Items[_environment.BinaryEnvironment.StartPosition].Value = sampleBlock.Items[_environment.BinaryEnvironment.StartPosition].Value;
                                          workingBlock.Items[_environment.BinaryEnvironment.StartPosition].State = KeyItemViewModel.VisualStateE.Corrected;

                                          _environment.BinaryEnvironment.StartPosition = 0;
                                          _environment.BinaryEnvironment.PositionsCount = workingBlock.Size * 2;
                                          ChangeVisualStateInBinaryProtocol(workingBlock);
                                          ChangeVisualStateInBinaryProtocol(sampleBlock);
                                          
                                          workingBlock.ShowWorkingParity = false;
                                          sampleBlock.ShowWorkingParity = false;

                                          workingBlock.State = BlockViewModel.VisualStateE.ParityVisible;
                                          sampleBlock.State = BlockViewModel.VisualStateE.ParityVisible;
                                      }),
                                      TypeSwitch.Case<BackTrackCorrectedErrorStep>(UpdateOddErrorsBlocks));

            StateManager.WaitAnimations();
            protocolStepFinishedEventArgs.Handle.Set();
            Thread.Sleep(1000);
            _runner.NextStep();
        }

        private void UpdateOddErrorsBlocks()
        {
            foreach (var block in GetAllBlocks(BobBlocks))
            {
                block.State = _environment.OddErrorsCountBlocks.Contains(block.Model)
                                  ? BlockViewModel.VisualStateE.OddError
                                  : BlockViewModel.VisualStateE.Normal;
                block.UpdateParity();
            }
        }

        private void ChangeVisualStateInBinaryProtocol(BlockViewModel workingBlock)
        {
            workingBlock.StartPosition = _environment.BinaryEnvironment.StartPosition;
            workingBlock.WorkingSize = _environment.BinaryEnvironment.PositionsCount;
            workingBlock.ChangeBinaryVisualState();
        }

        private BlockViewModel GetWorkingBlock()
        {
            return GetAllBlocks(BobBlocks)
                .Single(block => block.Model == _environment.BinaryEnvironment.WorkingBlock);
        }

        private BlockViewModel GetSampleBlock()
        {
            return GetAllBlocks(AliceBlocks)
                .Single(block => block.Model == _environment.BinaryEnvironment.SampleBlock);
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
        private readonly CascadeProtocolRuntimeEnvironment _environment;
        private string _currentStepDescription;
        private IList<KeyItemViewModel> _aliceKeyViewModel;
        private IList<KeyItemViewModel> _bobKeyViewModel;
        private BlockSetViewModel[] _aliceBlocks;
        private BlockSetViewModel[] _bobBlocks;
        private string _currentStepDescriptionVisualState;
    }
}
