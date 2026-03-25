using scriabinWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF.ViewModel
{
    public class MapProfileViewModel : INotifyPropertyChanged
    {
        private readonly MapProfileModel Model;
        public string ID
        {
            get { return Model.ID.ToString(); }
        }
        public string Name
        {
            get {  return Model.Name; }
            set { Model.Name = value; }
        }
        public MapProfileViewModel(MapProfileModel model)
        {
            Model = model;
            Links = [];
            foreach (AbstractLinkModel link in Model.Links)
            {
                switch (link.GetLinkType())
                {
                    case (byte)LinkType.NOTE_LINK:
                        Links.Add(new NoteLinkViewModel((NoteLinkModel)link));
                        break;
                    case (byte)LinkType.NOTE_ON:
                        Links.Add(new NoteOnLinkViewModel((NoteOnLinkModel)link));
                        break;
                    case (byte)LinkType.NOTE_OFF:
                        Links.Add(new NoteOffLinkViewModel((NoteOffLinkModel)link));
                        break;
                    case (byte)LinkType.CONTROL_CHANGE:
                        Links.Add(new ControlChangeLinkViewModel((ControlChangeLinkModel)link));
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<AbstractLinkViewModel> Links
        { get; private set; }

        //public ObservableCollection<AbstractLinkViewModel> Links
        //{
        //    get
        //    {
        //        ObservableCollection<AbstractLinkViewModel> viewModels = new ObservableCollection<AbstractLinkViewModel>();
        //        foreach (AbstractLinkModel link in Model.Links)
        //        {
        //            switch (link.GetLinkType())
        //            {
        //                case (byte)LinkType.NOTE_LINK:
        //                    viewModels.Add(new NoteLinkViewModel((NoteLinkModel)link));
        //                    break;
        //                case (byte)LinkType.NOTE_ON:
        //                    viewModels.Add(new NoteOnLinkViewModel((NoteOnLinkModel)link));
        //                    break;
        //                case (byte)LinkType.NOTE_OFF:
        //                    viewModels.Add(new NoteOffLinkViewModel((NoteOffLinkModel)link));
        //                    break;
        //                case (byte)LinkType.CONTROL_CHANGE:
        //                    viewModels.Add(new ControlChangeLinkViewModel((ControlChangeLinkModel)link));
        //                    break;
        //            }
        //        }
        //        return viewModels;
        //    }
        //}

        public void AddNoteLink()
        {
            Links.Add(new NoteLinkViewModel(Model.AddNoteLink()));
        }
        public void AddNoteOnLink()
        {
            Links.Add(new NoteOnLinkViewModel(Model.AddNoteOnLink()));
        }
        public void AddNoteOffLink()
        {
            Links.Add(new NoteOffLinkViewModel(Model.AddNoteOffLink()));
        }
        public void AddControlChangeLink()
        {
            Links.Add(new ControlChangeLinkViewModel(Model.AddControlChangeLink()));
        }

        internal void RemoveLink(AbstractLinkViewModel link)
        {
            Links.Remove(link);
            Model.Links.Remove(link.Model);
        }
    }
}
