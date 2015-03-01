using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cascade.Annotations;

namespace Cascade.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private string _visualState;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string VisualState
        {
            get { return _visualState; }
            set
            {
                _visualState = value;
                NotifyPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
