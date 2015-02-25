using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cascade.Model;

namespace Cascade.ViewModel
{
    public class KeyItemViewModel : ViewModelBase
    {
        private readonly KeyItem _item;

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
                NotifyPropertyChanged("Position");
            }
        }

        public int Value
        {
            get { return _item.Value; }
            set
            {
                _item.Value = value;
                NotifyPropertyChanged("Value");
            }
        }
    }
}
