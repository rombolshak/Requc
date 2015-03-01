using System.Collections.Generic;

namespace Cascade.ViewModel
{
    public class BlockSetViewModel : ViewModelBase
    {
        private IList<BlockViewModel> _blocks;
        private VisualStateE _state;

        public IList<BlockViewModel> Blocks
        {
            get { return _blocks; }
            set
            {
                _blocks = value;
                NotifyPropertyChanged();
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
            Collapsed,
            Invisible,
            Visible
        }
    }
}
