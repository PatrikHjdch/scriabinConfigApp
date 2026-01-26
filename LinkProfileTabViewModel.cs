using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class LinkProfileTabViewModel
    {
        public LinkProfileTabModel Model { get; set; }
        public LinkProfileTabViewModel(LinkProfileTabModel model) {
            Model = model;
        }
        public ObservableCollection<AbstractLinkModel> Links
        {
            get { return Model.Links; }
        }
        public void AddNoteLink()
        {
            Model.Links.Add(new NoteLinkModel());
        }
        public void AddNoteOnLink()
        {
            Model.Links.Add(new NoteOnLinkModel());
        }
        public void AddNoteOffLink()
        {
            Model.Links.Add(new NoteOffLinkModel());
        }
        public void AddControlChangeLink()
        {
            Model.Links.Add(new ControlChangeLinkModel());
        }

        public string TabName => $"Link Profile {Model.id}";
    }
}
