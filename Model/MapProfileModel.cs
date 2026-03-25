using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF.Model
{
    public class MapProfileModel(int id, string name)
    {
        public int ID = id;
        public ObservableCollection<AbstractLinkModel> Links = [];
        public string Name = name;
        public byte defaultChannel = 1;
        public NoteLinkModel AddNoteLink()
        {
            Links.Add(new NoteLinkModel());
            return (NoteLinkModel)Links.Last();
        }
        public NoteOnLinkModel AddNoteOnLink()
        {
            Links.Add(new NoteOnLinkModel());
            return (NoteOnLinkModel)Links.Last();
        }
        public NoteOffLinkModel AddNoteOffLink()
        {
            Links.Add(new NoteOffLinkModel());
            return (NoteOffLinkModel)Links.Last();
        }
        public ControlChangeLinkModel AddControlChangeLink()
        {
            Links.Add(new ControlChangeLinkModel());
            return (ControlChangeLinkModel)Links.Last();
        }
    }
}
