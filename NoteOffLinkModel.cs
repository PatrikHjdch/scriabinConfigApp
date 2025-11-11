using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    class NoteOffLinkModel : AbstractLinkModel
    {
        public byte DmxValue { get; set; } // 0-255
        public NoteOffLinkModel() : base()
        {
            DmxValue = 0;
        }

        public override byte[] Serialize()
        {
            return [
                (byte)(DmxChannel >> 8 & 0xFF),
                (byte)(DmxChannel & 0xFF),
                DmxValue
                ];
        }
        internal override byte GetLinkType()
        {
            return 2;
        }
    }
}
