using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public abstract class AbstractLinkModel
    {
        public byte MidiChannel { get; set; } // 1-16
        public byte Pitch { get; set; } // 0-127
        public int DmxChannel { get; set; } // 1-512

        public AbstractLinkModel()
        {
            MidiChannel = 1;
            Pitch = 60;
            DmxChannel = 1;
        }
    }
}
