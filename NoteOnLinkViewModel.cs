using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class NoteOnLinkViewModel : AbstractLinkViewModel
    {
        private NoteOnLinkModel _model;
        public override AbstractLinkModel Model {
            get { return _model; }
            set { _model = (NoteOnLinkModel)value; }
        }
        public NoteOnLinkViewModel(NoteOnLinkModel model)
        {
            _model = model;
        }
        public byte DmxValue
        {
            get { return _model.DmxValue; }
            set { _model.DmxValue = value; }
        }
        public bool UsesVelocity
        {
            get { return _model.UsesVelocity; }
            set { _model.UsesVelocity = value; }
        }
    }
}
