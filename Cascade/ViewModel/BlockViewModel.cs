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

            _block.KeyItems.CollectionChanged += (o, e) => NotifyPropertyChanged(null);
        }

        public IEnumerable<KeyItemViewModel> Items
        {
            get { return _block.KeyItems.Select(item => _keyItemViewModels.First(model => model.Position == item.Position)); }
        }

        public int Parity
        {
            get { return _block.Parity; }
        }

        public int Size
        {
            get { return _block.Size; }
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

        private readonly ProtocolBlock _block;
        private readonly IEnumerable<KeyItemViewModel> _keyItemViewModels;
        private VisualStateE _state;

        public enum VisualStateE
        {
            ParityVisible,
            PositionsVisible,
            NothingVisible
        }
    }
}
