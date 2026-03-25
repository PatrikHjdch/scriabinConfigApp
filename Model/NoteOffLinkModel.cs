using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF.Model
{
    public class NoteOffLinkModel : AbstractLinkModel
    {
        public byte DmxValue { get; set; } // 0-255
        public NoteOffLinkModel() : base()
        {
            DmxValue = 0;
        }

        public override byte[] Serialize()
        {
            return [
                0x10,
                (byte)(DmxChannel >> 8 & 0xFF),
                (byte)(DmxChannel & 0xFF),
                DmxValue
                ];
        }
        internal override byte GetLinkType()
        {
            return (byte)LinkType.NOTE_OFF;
        }

        public override NoteOffLinkModel Copy()
        {
            NoteOffLinkModel copy = new()
            {
                MidiChannel = MidiChannel,
                Pitch = Pitch,
                DmxChannel = DmxChannel,
                DmxValue = DmxValue
            };
            return copy;
        }
    }
}
