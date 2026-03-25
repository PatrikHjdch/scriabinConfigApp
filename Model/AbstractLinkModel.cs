using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF.Model
{
    enum LinkType
    {
        NOTE_LINK = 0,
        NOTE_ON = 1,
        NOTE_OFF = 2,
        CONTROL_CHANGE = 3
    }
    public abstract class AbstractLinkModel
    {
        public byte MidiChannel { get; set; } // 1-16
        public byte Pitch { get; set; } // 0-127
        public ushort DmxChannel { get; set; } // 1-512

        public AbstractLinkModel()
        {
            MidiChannel = 1;
            Pitch = 60;
            DmxChannel = 1;
        }
        public abstract byte[] Serialize();
        internal abstract byte GetLinkType();
        public abstract AbstractLinkModel Copy();
    }
}
