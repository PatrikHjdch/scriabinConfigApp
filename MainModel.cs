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
        public MainModel()
        {
            Links = new ObservableCollection<AbstractLinkModel>();
            AvailableComPorts = new ObservableCollection<string>();
            RefreshComPorts();
            SelectedComPort = AvailableComPorts.FirstOrDefault() ?? string.Empty;
            communicator = new Communicator();
            UpdateComPort();
        }
        public void RefreshComPorts()
        {
            AvailableComPorts.Clear();
            SerialPort.GetPortNames().ToList().ForEach(port => AvailableComPorts.Add(port));
            SelectedComPort = AvailableComPorts.FirstOrDefault() ?? string.Empty;
            UpdateComPort();
        }
        internal void UpdateComPort()
        {
            communicator.SetComPort(SelectedComPort);
        }
    }
}
