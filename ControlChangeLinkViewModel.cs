using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    public class ControlChangeLinkViewModel : AbstractLinkViewModel
    {
        private ControlChangeLinkModel _model;
        public override AbstractLinkModel Model {
            get { return _model; }
            set { _model = (ControlChangeLinkModel)value; }
        }
        public ControlChangeLinkViewModel(ControlChangeLinkModel model)
        {
            _model = model;
        }
    }
}
