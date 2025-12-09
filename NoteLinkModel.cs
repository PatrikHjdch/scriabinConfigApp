using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    class NoteLinkModel : AbstractLinkModel
    {
        public byte DmxValueOn { get; set; } // 0-255
        public byte DmxValueOff { get; set; } // 0-255
        public ushort Timeout { get; set; } // in milliseconds
        public bool TimeoutEnabled { get; set; } // if true, turn off after timeout
        public bool UsesVelocity { get; set; } // if true, use note velocity for DmxValueOn

        public NoteLinkModel() : base()
        {
            DmxValueOn = 255;
            DmxValueOff = 0;
            Timeout = 5000;
            TimeoutEnabled = true;
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
                DmxValueOn,
                DmxValueOff,
                TimeoutEnabled ? (byte)(Timeout >> 8 & 0xFF) : (byte)0,
                TimeoutEnabled ? (byte)(Timeout & 0xFF) : (byte)0
            ];
        }

        internal override byte GetLinkType()
        {
            return 0;
        }
    }
}
