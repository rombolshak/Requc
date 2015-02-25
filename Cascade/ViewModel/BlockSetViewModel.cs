using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cascade.ViewModel
{
    public class BlockSetViewModel : ViewModelBase
    {
        private IEnumerable<BlockViewModel> _blocks;

        public IEnumerable<BlockViewModel> Blocks
        {
            get { return _blocks; }
            set
            {
                _blocks = value;
                NotifyPropertyChanged("AliceBlocks");
            }
        }
    }
}
