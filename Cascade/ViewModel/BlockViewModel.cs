using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cascade.Model;

namespace Cascade.ViewModel
{
    public class BlockViewModel : ViewModelBase
    {
        public BlockViewModel(ProtocolBlock block, IEnumerable<KeyItemViewModel> keyItemViewModels)
        {
            _block = block;
            _keyItemViewModels = keyItemViewModels;
            WorkingSize = Size;
            StartPosition = 0;
        }

        public ProtocolBlock Model
        {
            get { return _block; }
        }

        public IList<KeyItemViewModel> Items
        {
            get { return _block.KeyItems.Select(item => _keyItemViewModels.First(model => model.Position == item.Position)).ToList(); }
        }

        public int Parity
        {
            get { return ShowWorkingParity ? WorkingParity : _block.Parity; }
        }

        public void UpdateParity()
        {
            NotifyPropertyChanged("Parity");
        }

        public int Size
        {
            get { return _block.Size; }
        }

        public int WorkingSize { get; set; }
        public int StartPosition { get; set; }
        public int WorkingParity
        {
            get { return _workingParity; }
            set
            {
                if (value == _workingParity) return;
                _workingParity = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Parity");
            }
        }

        public bool ShowWorkingParity
        {
            get { return _showWorkingParity; }
            set
            {
                if (value.Equals(_showWorkingParity)) return;
                _showWorkingParity = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Parity");
            }
        }

        public VisualStateE State
        {
            get { return _state; }
            set
            {
                _state = value;
                NotifyPropertyChanged();
                VisualState = _state.ToString();
            }
        }

        public enum VisualStateE
        {
            ParityVisible,
            PositionsVisible,
            NothingVisible,
            ParityMatched,
            ParityNotMatched,
            OddError,
            Normal,
            OddErrorSelected,
            OddErrorNotSelected,
            ParityTextVisible,
            ParityTextInvisible
        }

        public event EventHandler ChangeVisualStateRequested;

        protected virtual void OnChangeVisualStateRequested()
        {
            var handler = ChangeVisualStateRequested;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void ChangeBinaryVisualState()
        {
            OnChangeVisualStateRequested();
        }

        private readonly ProtocolBlock _block;
        private readonly IEnumerable<KeyItemViewModel> _keyItemViewModels;
        private VisualStateE _state;
        private bool _showWorkingParity;
        private int _workingParity;
    }
}
