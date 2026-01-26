using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace scriabinWPF
{
    internal class Communicator
    {
        private static readonly Dictionary<string, byte> IncomingMessages = new Dictionary<string, byte>
        {
            {"HELLO", 0x01 },
            {"ACKNOWLEDGEMENT", 0x02 },
            {"ERROR", 0x03 },
        };

        private static readonly Dictionary<byte, string> ErrorStrings = new Dictionary<byte, string>
        {
            {0x01, "Links are being sent out of order." },
            {0x02, "Error when writing to EEPROM via I2C." },
        };

        private static readonly Dictionary<string, byte> OutgoingMessages = new Dictionary<string, byte>
        {
            {"HELLO", 0x01 },
            {"ACKNOWLEDGEMENT", 0x02 },
            {"START_UPLOAD", 0x03 },
            {"END_UPLOAD", 0x04 },
            {"CHANNEL", 0x05 },
            {"TYPE", 0x06 },
            {"PITCH", 0x07 },
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
            ComPort = new SerialPort(PortName, 115200)
            {
                ReadTimeout = -1,
                WriteTimeout = -1
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
        private void SendMessage(byte[] data)
        {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            if (!ComPort.IsOpen)
                throw new InvalidOperationException("COM port is not open.");

            ComPort.Write(data, 0, data.Length);
        }

        private void AwaitAcknowledgment()
        {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            if (!ComPort.IsOpen)
                throw new InvalidOperationException("COM port is not open.");
            byte[] response = new byte[2];
            while (true)
            {
                try
                {
                    response[0] = (byte)ComPort.ReadByte();
                    if (response[0] == IncomingMessages["ACKNOWLEDGEMENT"])
                    {
                        return;
                    }
                    else if (response[0] == IncomingMessages["ERROR"])
                    {
                        response[1] = (byte)ComPort.ReadByte();
                        throw new InvalidOperationException("Received error from device:\n" + ErrorStrings[response[1]]);
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
            Logger logger = new Logger("UploadMap");
            logger.Log("Starting map upload...");
            if (Links == null) {
                logger.Log("Links collection is null.");
                throw new ArgumentNullException(nameof(Links));
            }
            if (ComPort == null) {
                logger.Log("COM port is not initialized.");
                throw new InvalidOperationException("COM port is not initialized.");
            }
            int type = -1;
            int channel = -1;
            int pitch = -1;
            List<AbstractLinkModel> linkList = Links.ToList();
            linkList.Sort(CompareLinks);
            try
            {
                logger.Log("Opening COM port...");
                Open();
                logger.Log("Sending START_UPLOAD message...");
                SendMessage(new byte[] { OutgoingMessages["START_UPLOAD"] });
                logger.Log("Awaiting acknowledgment...");
                AwaitAcknowledgment();
                logger.Log("ACK from device. Uploading links...");

                foreach (AbstractLinkModel link in linkList)
                {
                    if (channel < link.MidiChannel - 1)
                    {
                        logger.Log("Updating MIDI channel to " + link.MidiChannel);
                        SendMessage(new byte[] { OutgoingMessages["CHANNEL"], (byte)(link.MidiChannel - 1) });
                        logger.Log("Awaiting acknowledgment...");
                        AwaitAcknowledgment();
                        logger.Log("ACK from device.");
                        channel = link.MidiChannel - 1;
                        type = -1;
                        pitch = -1;
                    }
                    if (type < link.GetLinkType())
                    {
                        logger.Log("Updating link type to " + link.GetLinkType());
                        SendMessage(new byte[] { OutgoingMessages["TYPE"], (byte)link.GetLinkType() });
                        logger.Log("Awaiting acknowledgment...");
                        AwaitAcknowledgment();
                        logger.Log("ACK from device.");
                        type = link.GetLinkType();
                        pitch = -1;
                    }
                    if (pitch < link.Pitch)
                    {
                        logger.Log("Updating pitch to " + link.Pitch);
                        SendMessage(new byte[] { OutgoingMessages["PITCH"], (byte)link.Pitch });
                        logger.Log("Awaiting acknowledgment...");
                        AwaitAcknowledgment();
                        logger.Log("ACK from device.");
                        pitch = link.Pitch;
                    }
                    logger.Log("Sending link data...");
                    SendMessage(link.Serialize());
                    logger.Log("Awaiting acknowledgment...");
                    AwaitAcknowledgment();
                    logger.Log("ACK from device.");
                }
                logger.Log("All links uploaded. Sending END_UPLOAD message...");
                SendMessage(new byte[] { OutgoingMessages["END_UPLOAD"] });
                logger.Log("Awaiting final acknowledgment...");
                AwaitAcknowledgment();
                logger.Log("ACK from device. Map upload completed successfully.");
                logger.Log("Closing COM port...");
                Close();
                logger.Dispose();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Upload failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                logger.Dispose();
            }
        }
        private int CompareLinks(AbstractLinkModel a, AbstractLinkModel b)
        {
            if (a.MidiChannel != b.MidiChannel) return a.MidiChannel - b.MidiChannel;
            if (a.GetLinkType() != b.GetLinkType()) return a.GetLinkType() - b.GetLinkType();
            if (a.Pitch != b.Pitch) return a.Pitch - b.Pitch;
            if (a.DmxChannel != b.DmxChannel) return a.DmxChannel - b.DmxChannel;
            return 0;
        }

        internal int TestConnection() {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            try
            {
                Open();
                SendMessage(new byte[] { OutgoingMessages["HELLO"] });
                byte response = (byte)ComPort.ReadByte();
                if (response == IncomingMessages["HELLO"])
                {
                    Close();
                    return 0; // Success
                }
                else if (response == IncomingMessages["ERROR"])
                {
                    Close();
                    return 2; // Device returned error
                }
                else
                {
                    Close();
                    return 3; // Unexpected response
                }
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show("System.IO Exception: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return 2;
            }
            catch (TimeoutException)
            {
                Close();
                return 1; // Timeout
            }
        }

        internal string GetConnectionStatus()
        {
            if (ComPort == null)
            {
                return "Uninitialized";
            }
            return ComPort.IsOpen ? "Open" : "Closed";
        }
    }
}