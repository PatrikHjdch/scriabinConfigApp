using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO.Ports;
using System.Windows.Controls.Primitives;

namespace scriabinWPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel MainModel;
        private Communicator CommunicatorInstance;
        private ObservableCollection<string> availableComPorts;
        public ObservableCollection<string> AvailableComPorts
        {
            get { return availableComPorts; }
            set
            {
                availableComPorts = value;
                OnPropertyChanged();
            }
        }
        private string selectedComPort;
        public string SelectedComPort
        {
            get { return selectedComPort; }
            set
            {
                if (selectedComPort != value)
                {
                    selectedComPort = value;
                    CommunicatorInstance.SetComPort(value);
                    OnPropertyChanged();
                }
            }
        }
        private string connectButtonLabel = "Connect";
        public string ConnectButtonLabel
        {
            get { return connectButtonLabel; }
            set
            {
                if (connectButtonLabel != value)
                {
                    connectButtonLabel = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool connectButtonEnabled = true;
        public bool ConnectButtonEnabled
        {
            get { return connectButtonEnabled; }
            set
            {
                if (connectButtonEnabled != value)
                {
                    connectButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool uploadButtonEnabled = false;
        public bool UploadButtonEnabled
        {
            get { return uploadButtonEnabled; }
            set
            {
                if (uploadButtonEnabled != value)
                {
                    uploadButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private string connectionStatus = "Disconnected";
        public string ConnectionStatus
        {
            get { return connectionStatus; }
            set
            {
                if (connectionStatus != value)
                {
                    connectionStatus = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LogContents
        {
            get { return CommunicatorInstance.LogContents; }
        }
        public string CommPing
        {
            get { return "Ping: " + CommunicatorInstance.PingValue.ToString(); }
        }
        public MainViewModel()
        {
            MainModel = new MainModel();
            CommunicatorInstance = new Communicator();
            CommunicatorInstance.PropertyChanged += Communicator_PropertyChanged;
            foreach (MapProfileModel profile in MainModel.MapProfiles)
            {
                MapProfiles.Add(new MapProfileViewModel(profile));
            }
            selectedComPort = "";
            availableComPorts = [];
            RefreshComPorts();
        }
        private void Communicator_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Communicator.State):
                    switch (CommunicatorInstance.State)
                    {
                        case CommunicatorState.OFF:
                            ConnectButtonEnabled = true;
                            ConnectButtonLabel = "Connect";
                            UploadButtonEnabled = false;
                            ConnectionStatus = "Disconnected";
                            break;
                        case CommunicatorState.IDLE:
                            ConnectButtonEnabled = true;
                            ConnectButtonLabel = "Disconnect";
                            UploadButtonEnabled = true;
                            ConnectionStatus = "Connected";
                            break;
                        case CommunicatorState.UPLOAD:
                            ConnectButtonEnabled = false;
                            ConnectButtonLabel = "Disconnect";
                            UploadButtonEnabled = false;
                            ConnectionStatus = "Connected - Uploading";
                            break;
                    }
                    break;
                case nameof(Communicator.LogContents):
                    OnPropertyChanged(nameof(LogContents));
                    break;
            }
        }

        public ObservableCollection<MapProfileViewModel> MapProfiles
        {
            get
            {
                ObservableCollection<MapProfileViewModel> mapProfiles = new ObservableCollection<MapProfileViewModel>();
                foreach (MapProfileModel model in MainModel.MapProfiles)
                {
                    mapProfiles.Add(new MapProfileViewModel(model));
                }
                return mapProfiles;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void RefreshComPorts()
        {
            string prevSelected = "";
            if (!selectedComPort.Equals(""))
            {
                prevSelected = selectedComPort;
            }
            string[] portNames = SerialPort.GetPortNames();
            AvailableComPorts = [];
            foreach (string portName in portNames)
            {
                AvailableComPorts.Add(portName);
            }
            if (prevSelected != null)
            {
                if (availableComPorts.Contains(prevSelected))
                {
                    SelectedComPort = prevSelected;
                    return;
                }
                
            }
            SelectedComPort = AvailableComPorts.FirstOrDefault() ?? "";
        }
        internal async void HandleConnect()
        {
            if (CommunicatorInstance.commsTask.IsCompleted)
            {
                await CommunicatorInstance.Run();
            }
            else CommunicatorInstance.Stop();
        }

        internal void StartUpload()
        {
            CommunicatorInstance.StartUpload(MainModel.MapProfiles);
        }
    }
}