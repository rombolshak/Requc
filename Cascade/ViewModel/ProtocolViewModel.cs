using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
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

            _screenCapture = new ScreenCapture("images");

            _runner.StepStarted += RunnerOnStepStarted;
            _runner.StepFinished += RunnerOnStepFinished;

            AliceKey = environment.AliceKey.Select(item => new KeyItemViewModel(item)).ToList();
            BobKey = environment.BobKey.Select(item => new KeyItemViewModel(item)).ToList();
            ErrorItems =
                environment.BobKey.Where((model, i) => environment.AliceKey[i].Value != model.Value)
                           .Select(model => new KeyItemViewModel(model))
                           .ToList();

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

        public IList<KeyItemViewModel> ErrorItems
        {
            get { return _errorItems; }
            set
            {
                if (Equals(value, _errorItems)) return;
                _errorItems = value;
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

            _screenCapture.CaptureWindowToFile(Process.GetCurrentProcess().MainWindowHandle,
                                               DateTime.Now.ToString("hhmmss_") + protocolStepStartedEventArgs.Step + "_started.png",
                                               ImageFormat.Png);
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

                                  foreach (var viewModel in ErrorItems)
                                  {
                                      viewModel.State = KeyItemViewModel.VisualStateE.Normal;
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

                                  foreach (var item in GetSampleBlock().Items)
                                  {
                                      item.State = KeyItemViewModel.VisualStateE.Corrected;
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
                                      var workingBlock = GetWorkingBlock();
                                      var sampleBlock = GetSampleBlock();
                                      foreach (
                                          var item in
                                              workingBlock.Items.Except(
                                                  workingBlock.Items.Skip(_environment.BinaryEnvironment.StartPosition)
                                                              .Take(_environment.BinaryEnvironment.PositionsCount))
                                                          .Concat(
                                                              sampleBlock.Items.Except(
                                                                  sampleBlock.Items.Skip(_environment.BinaryEnvironment.StartPosition)
                                                                             .Take(_environment.BinaryEnvironment.PositionsCount))))
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

                                          var correctedPosition = _environment.BinaryEnvironment.StartPosition;
                                          workingBlock.Items[correctedPosition].Value =
                                              sampleBlock.Items[correctedPosition].Value;
                                          workingBlock.Items[correctedPosition].State =
                                              KeyItemViewModel.VisualStateE.Corrected;
                                          sampleBlock.Items[correctedPosition].State = KeyItemViewModel.VisualStateE.Normal;
                                          ErrorItems.Single(model => model.Position == workingBlock.Items[correctedPosition].Position).State =
                                              KeyItemViewModel.VisualStateE.Corrected;

                                          _environment.BinaryEnvironment.StartPosition = 0;
                                          _environment.BinaryEnvironment.PositionsCount = workingBlock.Size * 2;
                                          ChangeVisualStateInBinaryProtocol(workingBlock);
                                          ChangeVisualStateInBinaryProtocol(sampleBlock);
                                          
                                          workingBlock.ShowWorkingParity = false;
                                          sampleBlock.ShowWorkingParity = false;

                                          workingBlock.State = BlockViewModel.VisualStateE.ParityVisible;
                                          sampleBlock.State = BlockViewModel.VisualStateE.ParityVisible;
                                      }),
                                      TypeSwitch.Case<BackTrackCorrectedErrorStep>(UpdateOddErrorsBlocks),
                                      TypeSwitch.Case<OnePassStep>(() =>
                                          {
                                              foreach (var block in BobKey.Where(item => item.State == KeyItemViewModel.VisualStateE.Corrected))
                                              {
                                                  block.State = KeyItemViewModel.VisualStateE.Normal;
                                              }
                                          }),
                                      TypeSwitch.Case<WholeProtocolStep>(() =>
                                          {
                                              foreach (var blockSet in AliceBlocks.Concat(BobBlocks))
                                              {
                                                  blockSet.State = BlockSetViewModel.VisualStateE.Collapsed;
                                              }
                                          }));

            StateManager.WaitAnimations();
            protocolStepFinishedEventArgs.Handle.Set();

            _screenCapture.CaptureWindowToFile(
                Process.GetCurrentProcess().MainWindowHandle,
                DateTime.Now.ToString("hhmmss_") + protocolStepFinishedEventArgs.Step + "_finished.png", ImageFormat.Png);
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

        private readonly ProtocolRunner _runner;
        private readonly CascadeProtocolRuntimeEnvironment _environment;
        private string _currentStepDescription;
        private IList<KeyItemViewModel> _aliceKeyViewModel;
        private IList<KeyItemViewModel> _bobKeyViewModel;
        private BlockSetViewModel[] _aliceBlocks;
        private BlockSetViewModel[] _bobBlocks;
        private string _currentStepDescriptionVisualState;
        private IList<KeyItemViewModel> _errorItems;
        private readonly ScreenCapture _screenCapture;
    }
}
