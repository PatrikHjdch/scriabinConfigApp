using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Timers;
using System.ComponentModel;
using System.Diagnostics;

namespace scriabinWPF.Model
{
    enum CommunicatorState
    {
        OFF,
        IDLE,
        UPLOAD
    }
    internal class Communicator : INotifyPropertyChanged
    {
        private SerialPort? ComPort;
        private int PingRequest;
        private readonly System.Timers.Timer pingTimer;
        private Logger? log;
        private readonly List<List<AbstractLinkModel>> MapCopy;
        public Task commsTask;
        private CommunicatorState state;
        public CommunicatorState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged();
                }
            }
        }
        private CancellationTokenSource? cts;
        public string LogContents
        {
            get { return log == null ? "No log to display." : log.CurrentLogContents; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string memberName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
        private void Log_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Logger.CurrentLogContents):
                    OnPropertyChanged(nameof(LogContents));
                    break;
            }
        }
        public int PingValue { get; private set; }
        public Communicator()
        {
            pingTimer = new System.Timers.Timer(1000);
            pingTimer.Elapsed += RequestPing;
            pingTimer.AutoReset = true;
            pingTimer.Enabled = false;
            PingRequest = 0;
            MapCopy = [];
            commsTask = Task.Run(() => { });
            State = CommunicatorState.OFF;
        }

        private void RequestPing(object? source, ElapsedEventArgs e)
        {
            PingRequest++;
        }
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
        private void Open()
        {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            if (!ComPort.IsOpen)
            {
                ComPort.Open();
                State = CommunicatorState.IDLE;
            }
        }
        private void Close()
        {
            if (ComPort == null) throw new InvalidOperationException("COM port is not initialized.");
            if (ComPort.IsOpen)
            {
                ComPort.Close();
                State = CommunicatorState.OFF;
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
            if (log == null)
            {
                throw new InvalidOperationException("Log was null.");
            }
            if (ComPort == null)
            {
                throw new InvalidOperationException("COM port is not initialized.");
            }
            if (!ComPort.IsOpen)
                throw new InvalidOperationException("COM port is not open.");
            byte[] response = new byte[3];
            while (true)
            {
                try
                {
                    response[0] = (byte)ComPort.ReadByte();
                    if (response[0] == Constants.IN_ACK)
                    {
                        log.Log("ACK received.");
                        return;
                    }
                    else
                    {
                        HandleDeviceErrors(response[0]);
                        return;
                    }
                    //else if (response[0] == Constants.IN_ERR)
                    //{
                    //    response[1] = (byte)ComPort.ReadByte();
                    //    if (response[1] == Constants.IN_ERR_I2C)
                    //    {
                    //        response[2] = (byte)ComPort.ReadByte();
                    //    }
                    //    log.Log("ERR received: " + ErrorMsgBuilder.GetErrorMessage(response));
                    //    throw new InvalidOperationException("Received error from device:\n" + ErrorMsgBuilder.GetErrorMessage(response));
                    //}
                }
                catch (TimeoutException)
                {
                    log.Log("Device timed out.");
                    throw new TimeoutException("Timed out waiting for acknowledgment from device.");
                }
            }
        }
        public void StartUpload(ObservableCollection<MapProfileModel> profiles)
        {
            if (State == CommunicatorState.UPLOAD)
            {
                MessageBox.Show("Upload already in progress", "Already in progress", MessageBoxButton.OK);
                return;
            }
            MapCopy.Clear();
            for (int i = 0; i < profiles.Count; i++)
            {
                MapCopy.Add([]);
                foreach (AbstractLinkModel linkModel in profiles[i].Links)
                {
                    MapCopy[i].Add(linkModel.Copy());
                }
                MapCopy[i].Sort(CompareLinks);
            };
            State = CommunicatorState.UPLOAD;
        }
        private int UploadMap()
        {
            if (log == null)
            {
                throw new InvalidOperationException("Logger was null.");
            }
            if (MapCopy == null) {
                log.Log("Map doesn't exist.");
                return -1;
            }
            // initializace
            byte[] outgoingData;

            // upload start
            outgoingData = [Constants.OUT_START_UPLOAD];
            log.Log("Sending START_UPLOAD...");
            SendMessage(outgoingData);
            AwaitAcknowledgment();

            // jednotlive profily
            for (byte profile = 0; profile < MapCopy.Count; profile++)
            {
                log.Log("Changing profile to " + profile.ToString());

                // update cisla profilu, vynulovani zbytku
                byte channel = 0;
                byte type = 0;
                byte pitch = 0;
                outgoingData = [Constants.OUT_PROFILE, profile];

                log.Log("Seding PROFILE...");
                SendMessage(outgoingData);
                AwaitAcknowledgment();

                foreach(AbstractLinkModel link in MapCopy[profile])
                {
                    if (link.MidiChannel - 1 > channel)
                    {
                        channel = (byte)(link.MidiChannel - 1);
                        log.Log("Changing channel to " + channel.ToString());
                        type = 0;
                        pitch = 0;
                        outgoingData = [Constants.OUT_CHANNEL, channel];

                        log.Log("Sending CHANNEL...");
                        SendMessage(outgoingData);
                        AwaitAcknowledgment();
                    }
                    if (link.GetLinkType() > type)
                    {
                        type = link.GetLinkType();
                        log.Log("Changing TYPE to " + type.ToString());
                        pitch = 0;
                        outgoingData = [Constants.OUT_TYPE, type];

                        log.Log("Sending TYPE...");
                        SendMessage(outgoingData);
                        AwaitAcknowledgment();
                    }
                    if (link.Pitch > pitch)
                    {
                        pitch = link.Pitch;
                        log.Log("Changing PITCH to " +  pitch.ToString());
                        outgoingData = [Constants.OUT_PITCH, pitch];

                        log.Log("Sending PITCH...");
                        SendMessage(outgoingData);
                        AwaitAcknowledgment();
                    }
                    outgoingData = link.Serialize();
                    string s = "Sending new link:\n\t";
                    for (int i = 0; i < outgoingData.Length; i++)
                    {
                        s += " " + outgoingData[i].ToString();
                    }
                    log.Log(s);
                    SendMessage(outgoingData);
                    AwaitAcknowledgment();
                }
            }
            outgoingData = [Constants.OUT_END_UPLOAD];
            log.Log("Sending END_UPLOAD...");
            SendMessage(outgoingData);
            AwaitAcknowledgment();
            MessageBox.Show("Map successfully uploaded.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        public void UploadMapOld(ObservableCollection<AbstractLinkModel> Links)
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
            byte[] outgoingData;
            List<AbstractLinkModel> linkList = Links.ToList();
            linkList.Sort(CompareLinks);
            try
            {
                logger.Log("Opening COM port...");
                Open();
                logger.Log("Sending START_UPLOAD message...");
                outgoingData = [Constants.OUT_START_UPLOAD];
                SendMessage(outgoingData);
                logger.Log("Awaiting acknowledgment...");
                AwaitAcknowledgment();
                logger.Log("ACK from device. Uploading links...");

                foreach (AbstractLinkModel link in linkList)
                {
                    if (channel < link.MidiChannel - 1)
                    {
                        logger.Log("Updating MIDI channel to " + link.MidiChannel);
                        outgoingData = [Constants.OUT_CHANNEL, (byte)(link.MidiChannel - 1)];
                        SendMessage(outgoingData);
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
                        outgoingData = [Constants.OUT_TYPE, link.GetLinkType()];
                        SendMessage(outgoingData);
                        logger.Log("Awaiting acknowledgment...");
                        AwaitAcknowledgment();
                        logger.Log("ACK from device.");
                        type = link.GetLinkType();
                        pitch = -1;
                    }
                    if (pitch < link.Pitch)
                    {
                        logger.Log("Updating pitch to " + link.Pitch);
                        outgoingData = [Constants.OUT_PITCH, link.Pitch ];
                        SendMessage(outgoingData);
                        logger.Log("Awaiting acknowledgment...");
                        AwaitAcknowledgment();
                        logger.Log("ACK from device.");
                        pitch = link.Pitch;
                    }
                    logger.Log("Sending link data...");
                    outgoingData = link.Serialize();
                    SendMessage(outgoingData);
                    logger.Log("Awaiting acknowledgment...");
                    AwaitAcknowledgment();
                    logger.Log("ACK from device.");
                }
                logger.Log("All links uploaded. Sending END_UPLOAD message...");
                outgoingData = [Constants.OUT_END_UPLOAD];
                SendMessage(outgoingData);
                logger.Log("Awaiting final acknowledgment...");
                AwaitAcknowledgment();
                logger.Log("ACK from device. Map upload completed successfully.");
                logger.Log("Closing COM port...");
                Close();
                logger.Dispose();
                MessageBox.Show("Upload successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
        internal string GetConnectionStatus()
        {
            if (ComPort == null)
            {
                return "Uninitialized";
            }
            return ComPort.IsOpen ? "Open" : "Closed";
        }
        private void HandleDeviceErrors(byte firstByte)
        {
            if (log == null)
            {
                throw new InvalidOperationException("Attempted to handle error when log was null.");
            }
            if (ComPort == null)
            {
                throw new InvalidOperationException("Attempted to handle error on uninitialized COM port.");
            }
            if (ComPort.IsOpen)
            {
                byte[] responseBytes = new byte[ComPort.BytesToRead];
                for (int i = 0; i < responseBytes.Length; i++)
                {
                    responseBytes[i] = (byte)ComPort.ReadByte();
                }
                switch (firstByte)
                {
                    case Constants.IN_ERR:
                        log.Log("Received ERR: " + CustomStringBuilder.GetErrorMessage(responseBytes));
                        throw new InvalidOperationException("Received error from device: " + CustomStringBuilder.GetErrorMessage(responseBytes));
                    default:
                        string es = firstByte.ToString();
                        foreach (byte b in responseBytes)
                        {
                            es += " " + b.ToString();
                        }
                        log.Log("Unexpected response: " + es);
                        throw new InvalidOperationException("Received unexpected response from device: " + es);
                }
            }
        }

        private int Ping()
        {
            if (ComPort == null) { return -1; }
            Stopwatch sw;
            sw = Stopwatch.StartNew();
            try
            {
                SendMessage([Constants.OUT_HELLO]);
                byte response = (byte)ComPort.ReadByte();
                sw.Stop();
                PingValue = (int)sw.Elapsed.TotalMilliseconds;
                if (response == Constants.IN_HELLO)
                {
                    return 0;
                }
                else if (Constants.IN_EVENTS.Contains(response))
                {
                    LogIncomingEvent([response]);
                    return 0;
                }
                else
                {
                    HandleDeviceErrors(response);
                    return -1;
                }
            }
            catch (TimeoutException)
            {
                if (sw.IsRunning) sw.Stop();
                Close();
                MessageBox.Show("Communication with device timed out.", "Timed out", MessageBoxButton.OK, MessageBoxImage.Error);
                return 1; // Timeout
            }
            catch (System.IO.IOException ex)
            {
                if (sw.IsRunning) sw.Stop();
                Close();
                MessageBox.Show("System.IO Exception: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 4;
            }
        }
        public void Stop()
        {
            if (cts == null) throw new InvalidOperationException("Attempted to stop communication that wasn't running.");
            cts.Cancel();
        }
        public async Task Run()
        {
            log = new Logger("Log");
            log.PropertyChanged += Log_PropertyChanged;
            if (ComPort == null)
            {
                MessageBox.Show("ComPort was null.", "Error", MessageBoxButton.OK);
                log.Log("ComPort was null.");
                log.Dispose();
                return;
            }
            Progress<double> progressIndicator = new();
            cts = new CancellationTokenSource();
            CancellationToken cancellationToken = cts.Token;
            try
            {
                log.Log("Opening COM port " + ComPort.PortName + "...");
                Open();
                log.Log("Opened successfully.");
            } catch (InvalidOperationException e)
            {
                log.Log("Invalid Operation: " + e);
                MessageBox.Show("Invalid Operation: " + e, "Error", MessageBoxButton.OK);
                return;
            }
            try
            {
                commsTask = Task.Run(() =>
                {
                    bool alive = true;
                    pingTimer.Start();
                    pingTimer.Enabled = true;
                    while (alive && !cts.IsCancellationRequested)
                    {
                        switch (State)
                        {
                            case CommunicatorState.IDLE:
                                if (ComPort.BytesToRead != 0)
                                {
                                    pingTimer.Stop();
                                    LogIncomingEvent();
                                    pingTimer.Start();
                                }
                                if (PingRequest > 0)
                                {
                                    PingRequest--;
                                    //if (Ping() != 0)
                                    //{
                                    //    log.Log("Device timed out.");
                                    //    alive = false;
                                    //    break;
                                    //}
                                    //log.Log("Ping.");
                                }
                                break;
                            case CommunicatorState.UPLOAD:
                                pingTimer.Stop();
                                log.Log("Upload starting...");
                                UploadMap();
                                log.Log("Upload successfully finished.");
                                State = CommunicatorState.IDLE;
                                pingTimer.Start();
                                break;
                        }
                    }
                }, cancellationToken);
                await commsTask;
            }
            catch (OperationCanceledException e)
            {
                log.Log("Operation cancelled: " + e.ToString());
                if (ComPort.IsOpen)
                {
                    ComPort.ReadExisting();
                    log.Log("Sending ABORT...");
                    SendMessage([Constants.OUT_ABORT]);
                }
            } finally
            {
                log.Log("Closing COM port...");
                if (pingTimer.Enabled) pingTimer.Stop();
                Close();
                log.Log("End.");
                State = CommunicatorState.OFF;
                log.Dispose();
                log = null;
            }
        }
        private void LogIncomingEvent(byte[] existing)
        {
            if (log == null) throw new InvalidOperationException("Attempted logging event when log was null.");
            if (ComPort == null) throw new InvalidOperationException("Attempted logging event when COM port was null.");
            List<byte> bytes = [];
            foreach (byte b in existing) { bytes.Add(b); }
            bytes.Add((byte)ComPort.ReadByte()); // typ udalosti
            switch (bytes[0])
            {
                case Constants.IN_EVENT_MIDI_RECEIVED:
                    bytes.Add((byte)ComPort.ReadByte());
                    if (Constants.MIDI_1_DATA_BYTE_MESSAGES.Contains((byte)(bytes[1] & Constants.MIDI_STATUS_BYTE_TYPE_MASK)))
                    {
                        bytes.Add((byte)ComPort.ReadByte());
                    }
                    else if (Constants.MIDI_2_DATA_BYTE_MESSAGES.Contains((byte)(bytes[1] & Constants.MIDI_STATUS_BYTE_TYPE_MASK)))
                    {
                        bytes.Add((byte)ComPort.ReadByte());
                        bytes.Add((byte)ComPort.ReadByte());
                    }
                    break;
                case Constants.IN_EVENT_DMX_CHANGED:
                    for (int i = 0; i < 3; i++) // kanal MSB, kanal LSB, hodnota
                    {
                        bytes.Add((byte)ComPort.ReadByte());
                    }
                    break;
                case Constants.IN_EVENT_PROFILE_CHANGED:
                    bytes.Add((byte)ComPort.ReadByte());
                    break;
                case Constants.IN_EVENT_LINK_ADDRESSES_READ:
                    for (int i = 0; i < 4; i++)
                    {
                        bytes.Add((byte)ComPort.ReadByte());
                    }
                    break;
            }
            log.LogDeviceEvent(bytes.ToArray());
        }

        private void LogIncomingEvent()
        {
            LogIncomingEvent([]);
        }
    }
}