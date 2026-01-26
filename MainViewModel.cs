using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace scriabinWPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        MainModel model;
        public ObservableCollection<string> AvailableComPorts
        {
            get { return model.AvailableComPorts; }
        }

        public string SelectedComPort
        {
            get { return model.SelectedComPort; }
            set
            {
                if (model.SelectedComPort != value)
                {
                    model.SelectedComPort = value;
                }
            }
        }

        public string ConnectionStatus
        {
            get { return model.GetConnectionStatus(); }
        }

        public MainViewModel()
        {
            model = new MainModel();
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<LinkProfileTabViewModel> Tabs
        {
            get
            {
                var tabViewModels = new ObservableCollection<LinkProfileTabViewModel>();
                foreach (var tabModel in model.Tabs)
                {
                    tabViewModels.Add(new LinkProfileTabViewModel(tabModel));
                }
                return tabViewModels;
            }
        }

        public void RefreshComPorts()
        {
            model.RefreshComPorts();
        }

        internal void UpdateComPort()
        {
            model.UpdateComPort();
        }

        internal int TestConnection()
        {
            return model.TestConnection();
        }

        internal void UploadMap()
        {
            model.UploadMap();
        }
    }
}