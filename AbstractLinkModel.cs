using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    enum LinkType
    {
        NOTE_LINK = (byte)0,
        NOTE_ON = (byte)1,
        NOTE_OFF = (byte)2,
        CONTROL_CHANGE = (byte)3
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
