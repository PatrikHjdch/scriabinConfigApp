using System.Collections.ObjectModel;
using System.ComponentModel;

namespace scriabinWPF
{
	public class MainViewModel : INotifyPropertyChanged
	{
		MainModel model;
		public MainViewModel()
		{
			model = new MainModel();
		}
		public event PropertyChangedEventHandler? PropertyChanged;

		public ObservableCollection<AbstractLinkModel> Links
		{
			get { return model.Links; }
		}

		public void AddLink()
		{
			model.Links.Add(new NoteLinkModel());
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
    }
}