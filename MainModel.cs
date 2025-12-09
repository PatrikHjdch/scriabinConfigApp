using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class MainModel
    {
        public ObservableCollection<AbstractLinkModel> Links { get; set; }
        public ObservableCollection<string> AvailableComPorts { get; set; }
        internal Communicator communicator { get; set; }
        public string SelectedComPort { get; set; }
        private bool changesMade = false;
        public MainModel()
        {
            communicator = new Communicator();
            Links = new ObservableCollection<AbstractLinkModel>();
            AvailableComPorts = new ObservableCollection<string>();
            RefreshComPorts();
            SelectedComPort = AvailableComPorts.FirstOrDefault() ?? string.Empty;
            UpdateComPort();
        }
        public void RefreshComPorts()
        {
            AvailableComPorts.Clear();
            SerialPort.GetPortNames().ToList().ForEach(port => AvailableComPorts.Add(port));
            SelectedComPort = AvailableComPorts.FirstOrDefault() ?? string.Empty;

        }
        internal void UpdateComPort()
        {
            communicator.SetComPort(SelectedComPort);
        }

        internal string GetConnectionStatus()
        {
            return communicator.GetConnectionStatus();
        }

        internal int TestConnection()
        {
            return communicator.TestConnection();
        }

        internal void UploadMap()
        {
            communicator.UploadMap(Links);
        }
    }
}
