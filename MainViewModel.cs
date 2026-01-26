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
        public void RefreshComPorts()
        {
            model.RefreshComPorts();
        }

        public ObservableCollection<AbstractLinkModel> Links
        {
            get { return model.Links; }
        }

        public void RemoveLink(AbstractLinkModel link)
        {
            model.Links.Remove(link);
        }

        internal void AddNoteLink()
        {
            model.Links.Add(new NoteLinkModel());
        }

        internal void AddNoteOnLink()
        {
            model.Links.Add(new NoteOnLinkModel());
        }

        internal void AddNoteOffLink()
        {
            model.Links.Add(new NoteOffLinkModel());
        }

        internal void AddControlChangeLink()
        {
            model.Links.Add(new ControlChangeLinkModel());
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