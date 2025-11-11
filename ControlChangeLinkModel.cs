using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    class ControlChangeLinkModel : AbstractLinkModel
    {
        public byte DmxValue { get; set; } // 0-255
        public ControlChangeLinkModel() : base()
        {
            DmxValue = 255;
        }
    }
}
