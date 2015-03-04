using Cascade.Model;

namespace Cascade.ViewModel
{
    public class KeyItemViewModel : ViewModelBase
    {
        private readonly KeyItem _item;
        private VisualStateE _state;

        public KeyItemViewModel(KeyItem item)
        {
            _item = item;
        }

        public int Position
        {
            get { return _item.Position; }
            set
            {
                _item.Position = value;
                NotifyPropertyChanged();
            }
        }

        public int Value
        {
            get { return _item.Value; }
            set
            {
                _item.Value = value;
                NotifyPropertyChanged();
            }
        }

        public bool ErrorHere
        {
            get { return _item.ErrorHere; }
            set
            {
                _item.ErrorHere = value;
                NotifyPropertyChanged();
            }
        }

        public VisualStateE State   
        {
            get { return _state; }
            set
            {
                _state = value;
                VisualState = _state.ToString();
            }
        }

        public enum VisualStateE
        {
            Error,
            Normal,
            Corrected
        }
    }
}
