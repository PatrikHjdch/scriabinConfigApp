using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    internal class Communicator
    {
        private static readonly Dictionary<string, string> IncomingMessages = new Dictionary<string, string>
        {
            {"Hello", "HEL" },
            {"Acknowledgement", "ACK" },
            {"Error", "ERR" },
        };

        private static readonly Dictionary<string, string> OutgoingMessages = new Dictionary<string, string>
        {
            {"Hello", "HEL" },
            {"Acknowledgment", "ACK" },
            {"StartUpload", "STA" },
            {"EndUpload", "END" },
            {"Type", "TYP" },
            {"Channel", "CHA" },
            {"Pitch", "PIT" },
        };

        private SerialPort? ComPort;
        public Communicator() { }
        public void SetComPort(string PortName)
        {
            if (ComPort != null && ComPort.IsOpen)
            {
                ComPort.Close();
            }
            if (ComPort != null)
            {
                ComPort.Dispose();
            }
            if (string.IsNullOrEmpty(PortName))
            {
                ComPort = null;
                return;
            }
            ComPort = new SerialPort(PortName, 115200, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = 2000,
                WriteTimeout = 2000
            };
        }
        public void Open()
        {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            if (!ComPort.IsOpen)
            {
                ComPort.Open();
            }
        }
        public void Close()
        {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            if (ComPort.IsOpen)
            {
                ComPort.Close();
            }
        }
        private void SendMessage(string Message)
        {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            if (!ComPort.IsOpen)
                throw new InvalidOperationException("COM port is not open.");

            byte[] data = Encoding.ASCII.GetBytes(Message);
            ComPort.Write(data, 0, data.Length);
        }

        private void AwaitAcknowledgment()
        {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            if (!ComPort.IsOpen)
                throw new InvalidOperationException("COM port is not open.");
            string response = string.Empty;
            while (true)
            {
                try
                {
                    response = ComPort.ReadLine().Trim();
                    if (response == IncomingMessages["Acknowledgement"])
                    {
                        return;
                    }
                    else if (response.StartsWith(IncomingMessages["Error"]))
                    {
                        throw new InvalidOperationException("Received error from device:\n" + response);
                    }
                }
                catch (TimeoutException)
                {
                    throw new TimeoutException("Timed out waiting for acknowledgment from device.");
                }
            }
        }

        public void UploadMap(ObservableCollection<AbstractLinkModel> Links)
        {
            if (Links == null) throw new ArgumentNullException(nameof(Links));
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            int type = -1;
            int channel = -1;
            int pitch = -1;
            List<AbstractLinkModel> linkList = Links.ToList();
            linkList.Sort(CompareLinks);
            Open();
            SendMessage(OutgoingMessages["StartUpload"]);
            AwaitAcknowledgment();

            foreach (AbstractLinkModel link in linkList)
            {
                if (type < link.GetLinkType())
                {
                    SendMessage(OutgoingMessages["Type"] + " " + link.GetLinkType().ToString());
                    AwaitAcknowledgment();
                    type = link.GetLinkType();
                    channel = -1;
                    pitch = -1;
                }   
                if (channel < link.MidiChannel)
                {
                    SendMessage(OutgoingMessages["Channel"] + " " + link.MidiChannel.ToString());
                    AwaitAcknowledgment();
                    channel = link.MidiChannel;
                    pitch = -1;
                }
                if (pitch < link.Pitch)
                {
                    SendMessage(OutgoingMessages["Pitch"] + " " + link.Pitch.ToString());
                    AwaitAcknowledgment();
                    pitch = link.Pitch;
                }
                byte[] serializedLink = link.Serialize();
                ComPort.Write(serializedLink, 0, serializedLink.Length);
                AwaitAcknowledgment();
            }

            SendMessage(OutgoingMessages["EndUpload"]);
            AwaitAcknowledgment();
            Close();
        }
        private int CompareLinks(AbstractLinkModel a, AbstractLinkModel b)
        {
           if (a.GetLinkType() != b.GetLinkType()) return a.GetLinkType() - b.GetLinkType();
            if (a.MidiChannel != b.MidiChannel) return a.MidiChannel - b.MidiChannel;
            if (a.Pitch != b.Pitch) return a.Pitch - b.Pitch;
            if (a.DmxChannel != b.DmxChannel) return a.DmxChannel - b.DmxChannel;
            return 0;
        }
    }
}