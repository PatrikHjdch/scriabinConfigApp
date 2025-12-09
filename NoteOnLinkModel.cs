using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace scriabinWPF
{
    class NoteOnLinkModel : AbstractLinkModel
    {
        public byte DmxValue { get; set; } // 0-255
        public bool UsesVelocity { get; set; } // if true, use note velocity for DmxValueOn
        public NoteOnLinkModel() : base()
        {
            DmxValue = 255;
            UsesVelocity = false;
        }

        public override byte[] Serialize()
        {
            return
            [
                0x10,
                (byte)(DmxChannel >> 8 & 0xFF),
                (byte)(DmxChannel & 0xFF),
                UsesVelocity ? (byte)1 : (byte)0,
                DmxValue
            ];
        }

        internal override byte GetLinkType()
        {
            return 1;
        }
    }
}
