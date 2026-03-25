using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public abstract class AbstractLinkViewModel
    {
        public abstract AbstractLinkModel Model
        {
            get;
            set;
        }
        
        public byte MidiChannel
        {
            get { return Model.MidiChannel; }
            set { Model.MidiChannel = value; }
        }
        public byte Pitch
        {
            get { return Model.Pitch; }
            set { Model.Pitch = value; }
        }

        public ushort DmxChannel
        {
            get { return Model.DmxChannel; }
            set { Model.DmxChannel = value; }
        }
    }
}
