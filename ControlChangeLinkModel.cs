using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class ControlChangeLinkModel : AbstractLinkModel
    {
        public ControlChangeLinkModel() : base() { }

        public override byte[] Serialize()
        {
            return [
                0x10,
                (byte)(DmxChannel >> 8 & 0xFF),
                (byte)(DmxChannel & 0xFF),
                ];
        }
        internal override byte GetLinkType()
        {
            return (byte)LinkType.CONTROL_CHANGE;
        }

        public override ControlChangeLinkModel Copy()
        {
            ControlChangeLinkModel copy = new()
            {
                MidiChannel = MidiChannel,
                Pitch = Pitch,
                DmxChannel = DmxChannel
            };
            return copy;
        }

    }
}
