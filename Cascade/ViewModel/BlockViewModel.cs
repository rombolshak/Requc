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

            _block.KeyItems.CollectionChanged += (o, e) =>
                {
                    NotifyPropertyChanged("Items");
                    NotifyPropertyChanged("Parity");
                };
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

        private readonly ProtocolBlock _block;
        private readonly IEnumerable<KeyItemViewModel> _keyItemViewModels;
    }
}
