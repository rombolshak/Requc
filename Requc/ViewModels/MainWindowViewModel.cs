namespace Requc.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            SimpleProtocol = new SimpleProtocolViewModel();
        }

        public SimpleProtocolViewModel SimpleProtocol { get; private set; }

        public override void Dispose()
        {
            SimpleProtocol.Dispose();
        }
    }
}