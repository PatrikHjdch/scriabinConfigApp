using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class NoteOffLinkViewModel : AbstractLinkViewModel
    {
        private NoteOffLinkModel _model;
        public override AbstractLinkModel Model
        {
            get {  return _model; }
            set { _model = (NoteOffLinkModel)value; }
        }
        public NoteOffLinkViewModel(NoteOffLinkModel model)
        {
            _model = model;
        }
        public byte DmxValue
        {
            get { return _model.DmxValue; }
            set { _model.DmxValue = value; }
        }
    }
}
