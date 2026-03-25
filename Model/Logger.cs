using Accessibility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF.Model
{
    internal class Logger : INotifyPropertyChanged
    {
        private FileStream logFileStream;
        private StreamWriter logStreamWriter;
        private string currentLogContents;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string CurrentLogContents
        {
            get { return currentLogContents; }
        }
        public Logger(string name)
        {
            currentLogContents = "";
            logFileStream = File.Create("logs\\" + "Scriabin_" + name + "_log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            logStreamWriter = new StreamWriter(logFileStream);
        }
        public void Log(string message)
        {
            string s = $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {message}";
            currentLogContents += s + "\n";
            OnPropertyChanged(nameof(CurrentLogContents));
            logStreamWriter.WriteLine(s);
            logStreamWriter.Flush();
        }
        public void Dispose()
        {
            logStreamWriter.Close();
            logFileStream.Close();
        }
        public void LogDeviceEvent(byte[] msg)
        {
            switch (msg[0])
            {
                case Constants.IN_EVENT_MIDI_RECEIVED:
                    Log("[MIDI]\t\t" + CustomStringBuilder.ParseMidiMessage(msg, 1));
                    break;
                case Constants.IN_EVENT_DMX_CHANGED:
                    Log("[DMX]\t\t[" + ((msg[1] << 8) + msg[2]).ToString() + "]: " + msg[3].ToString());
                    break;
                case Constants.IN_EVENT_PROFILE_CHANGED:
                    Log("[PROFILE]\t" + msg[1].ToString());
                    break;
                case Constants.IN_EVENT_LINK_ADDRESSES_READ:
                    Log("[DEBUG]\tADDRESSES: " + ((msg[1] << 8) + msg[2]).ToString() + " " + ((msg[3] << 8) + msg[4]).ToString());
                    break;
                default:
                    string s = "[UNKNOWN]";
                    foreach (byte b in msg)
                    {
                        s += " " + b.ToString();
                    }
                    Log(s);
                    break;
            }
        }
    }
}
