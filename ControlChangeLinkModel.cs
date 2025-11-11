using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    class ControlChangeLinkModel : AbstractLinkModel
    {
        public ControlChangeLinkModel() : base() { }

        public override byte[] Serialize()
        {
            return [
                (byte)(DmxChannel >> 8 & 0xFF),
                (byte)(DmxChannel & 0xFF),
                ];
        }
        internal override byte GetLinkType()
        {
            return 3;
        }
    }
}
