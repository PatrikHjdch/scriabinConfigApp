using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class NoteLinkViewModel : AbstractLinkViewModel
    {
        private NoteLinkModel _model;
        public override AbstractLinkModel Model
        {
            get { return _model; }
            set { _model = (NoteLinkModel)value; }
        }
        public NoteLinkViewModel(NoteLinkModel model)
        {
            _model = model;
        }
        public byte DmxValueOn
        {
            get { return _model.DmxValueOn; }
            set { _model.DmxValueOn = value; }
        }
        public byte DmxValueOff
        {
            get { return _model.DmxValueOff; }
            set { _model.DmxValueOff = value; }
        }
        public ushort Timeout
        {
            get { return _model.Timeout; }
            set { _model.Timeout = value; }
        }
        public bool TimeoutEnabled
        {
            get { return _model.TimeoutEnabled; }
            set { _model.TimeoutEnabled = value; }
        }
        public bool UsesVelocity
        {
            get { return _model.UsesVelocity; }
            set { _model.UsesVelocity = value; }
        }
    }
}
