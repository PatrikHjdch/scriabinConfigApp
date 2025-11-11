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
    }
}
